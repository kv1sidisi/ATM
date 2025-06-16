using Lab5.Application.Abstractions;
using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application.States;

public class InitState : IState
{
    private readonly IOperationRepository _operationRepository;

    public InitState(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
    }

    public Account? Login(AccountData accountData)
    {
        return _operationRepository.Login(accountData);
    }

    public string? PasswordChange(ILoggedAccount loggedAccount, string newPassword)
    {
        return null;
    }

    public double? Withdraw(ILoggedAccount loggedAccount, double amount)
    {
        return null;
    }

    public double? Deposit(ILoggedAccount loggedAccount, double amount)
    {
        return null;
    }

    public IEnumerable<SaveData>? History(ILoggedAccount loggedAccount)
    {
        return null;
    }

    public double? Balance(ILoggedAccount loggedAccount)
    {
        return null;
    }

    public Account? CreateAccount(AccountData accountData)
    {
        return null;
    }
}