using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Lab5.Application.Abstractions;
using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;
using Npgsql;

namespace Lab5.Infrastructure.DataAccess.Repository;

public class OperationRepository : IOperationRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public OperationRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public Account? Login(AccountData accountData)
    {
        const string sql = $"""
                            select user_id, user_name, user_balance, user_pin, user_role
                            from users
                            where user_name = @username and user_pin = @user_pin
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", accountData.Username)
            .AddParameter("user_pin", accountData.Password);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() is false)
            return null;

        return new Account(
            Id: reader.GetInt64(0),
            Username: reader.GetString(1),
            Balance: reader.GetDouble(2),
            Password: reader.GetString(3),
            Role: ParseAccountRole(reader.GetString(4)));
    }

    public string? PasswordChange(ILoggedAccount loggedAccount, string newPassword)
    {
        const string sql = $"""
                            update users
                            set user_pin = @newPassword
                            where user_id = @userId
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        if (loggedAccount.Account == null)
        {
            return null;
        }

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("newPassword", newPassword)
            .AddParameter("userId", loggedAccount.Account.Id);

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected == 0 ? null : newPassword;
    }

    public double? Withdraw(ILoggedAccount loggedAccount, double amount)
    {
        double? currentBalance = Balance(loggedAccount);

        if (currentBalance < amount || currentBalance == null)
        {
            return null;
        }

        const string sql = $"""
                            update users
                            set user_balance = user_balance - @amount
                            where user_id = @userId
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        if (loggedAccount.Account == null)
        {
            return null;
        }

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("amount", amount)
            .AddParameter("userId", loggedAccount.Account.Id);

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected == 0 ? null : amount;
    }

    public double? Deposit(ILoggedAccount loggedAccount, double amount)
    {
        const string sql = $"""
                            update users
                            set user_balance = user_balance + @amount
                            where user_id = @userId
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        if (loggedAccount.Account == null)
        {
            return null;
        }

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("amount", amount)
            .AddParameter("userId", loggedAccount.Account.Id);

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected == 0 ? null : amount;
    }

    public IEnumerable<SaveData>? History(ILoggedAccount loggedAccount)
    {
        const string sql = $"""
                            select operation_type, user_operation_amount_of_money
                            from operations
                            where user_id = @userId
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        if (loggedAccount.Account == null)
        {
            return null;
        }

        List<SaveData> history = new();
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("userId", loggedAccount.Account.Id);

        using NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            history.Add(new SaveData(
                OperationType: (OperationType)Enum.Parse(typeof(OperationType), reader.GetString(0)),
                Value: reader.GetInt64(1)));
        }

        return history;
    }

    public double? Balance(ILoggedAccount loggedAccount)
    {
        const string sql = $"""
                            select user_balance
                            from users
                            where user_id = @userId
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        if (loggedAccount.Account == null)
        {
            return null;
        }

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("userId", loggedAccount.Account.Id);

        using NpgsqlDataReader reader = command.ExecuteReader();

        return reader.Read() is false ? null : reader.GetDouble(0);
    }

    public Account? CreateAccount(AccountData accountData)
    {
        if (!IsUsernameAvailable(accountData.Username))
        {
            return null;
        }

        const string sql = $"""
                            insert into users (user_name, user_balance, user_pin, user_role)
                            values (@username, @balance, @pin, @role)
                            returning user_id, user_name, user_balance, user_pin, user_role
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", accountData.Username)
            .AddParameter("balance", 0)
            .AddParameter("pin", accountData.Password)
            .AddParameter("role", accountData.Role.ToString());

        using NpgsqlDataReader reader = command.ExecuteReader();
        return reader.Read()
            ? new Account(
                Id: reader.GetInt64(0),
                Username: reader.GetString(1),
                Balance: reader.GetDouble(2),
                Password: reader.GetString(3),
                Role: ParseAccountRole(reader.GetString(4)))
            : null;
    }

    public string? SaveToHistory(ILoggedAccount loggedAccount, SaveData saveData)
    {
        const string sql = """
                           INSERT INTO operations (operation_type, user_operation_amount_of_money, user_id)
                           VALUES (@operationType, @amount, @user_id)
                           RETURNING operation_id;
                           """;
        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        if (loggedAccount.Account == null)
        {
            return null;
        }

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("operationType", saveData.OperationType.ToString())
            .AddParameter("amount", saveData.Value)
            .AddParameter("user_id", loggedAccount.Account.Id);

        object? result = command.ExecuteScalar();
        return result != null && result != DBNull.Value ? result.ToString() : null;
    }

    private AccountRole ParseAccountRole(string roleString)
    {
        return roleString switch
        {
            "Admin" => AccountRole.Admin,
            "User" => AccountRole.Client,
            _ => throw new ArgumentException($"Invalid account role: {roleString}"),
        };
    }

    private bool IsUsernameAvailable(string username)
    {
        const string sql = $"""
                            select count(*)
                            from users
                            where user_name = @username
                            """;

        NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("username", username);

        int count = Convert.ToInt32(command.ExecuteScalar());
        return count == 0;
    }
}