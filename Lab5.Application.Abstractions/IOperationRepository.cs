using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application.Abstractions;

public interface IOperationRepository
{
    Account? Login(AccountData accountData);

    string? PasswordChange(ILoggedAccount loggedAccount, string newPassword);

    double? Withdraw(ILoggedAccount loggedAccount, double amount);

    double? Deposit(ILoggedAccount loggedAccount, double amount);

    IEnumerable<SaveData>? History(ILoggedAccount loggedAccount);

    double? Balance(ILoggedAccount loggedAccount);

    Account? CreateAccount(AccountData accountData);

    string? SaveToHistory(ILoggedAccount loggedAccount, SaveData saveData);
}