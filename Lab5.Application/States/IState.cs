using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application.States;

public interface IState
{
    Account? Login(AccountData accountData);

    string? PasswordChange(ILoggedAccount loggedAccount, string newPassword);

    double? Withdraw(ILoggedAccount loggedAccount, double amount);

    double? Deposit(ILoggedAccount loggedAccount, double amount);

    IEnumerable<SaveData>? History(ILoggedAccount loggedAccount);

    double? Balance(ILoggedAccount loggedAccount);

    Account? CreateAccount(AccountData accountData);
}