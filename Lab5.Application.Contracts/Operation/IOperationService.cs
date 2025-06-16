using Lab5.Application.Contracts.Results;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application.Contracts;

public interface IOperationService
{
    public ILoggedAccount LoggedAccount { get; }

    LoginResult Login(string username, string password);

    LoginResult Logout();

    OperationResult PasswordChange(string password);

    TransactionResult Withdraw(double amount);

    TransactionResult Deposit(double amount);

    History History();

    Balance Balance();

    OperationResult CreateAccount(string username, string password, AccountRole? role);
}