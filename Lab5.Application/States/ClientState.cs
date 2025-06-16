using Lab5.Application.Abstractions;
using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application.States;

public class ClientState : IState
{
    private readonly IOperationRepository _operationRepository;

    public ClientState(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
    }

    public Account? Login(AccountData accountData)
    {
        return null;
    }

    public string? PasswordChange(ILoggedAccount loggedAccount, string newPassword)
    {
        return _operationRepository.PasswordChange(loggedAccount, newPassword);
    }

    public double? Withdraw(ILoggedAccount loggedAccount, double amount)
    {
        return _operationRepository.Withdraw(loggedAccount, amount);
    }

    public double? Deposit(ILoggedAccount loggedAccount, double amount)
    {
        return _operationRepository.Deposit(loggedAccount, amount);
    }

    public IEnumerable<SaveData>? History(ILoggedAccount loggedAccount)
    {
        return _operationRepository.History(loggedAccount);
    }

    public double? Balance(ILoggedAccount loggedAccount)
    {
        return _operationRepository.Balance(loggedAccount);
    }

    public Account? CreateAccount(AccountData accountData)
    {
        return null;
    }
}