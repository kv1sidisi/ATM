using Lab5.Application.Abstractions;
using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application.States;

public class ProxyState : IState
{
    private readonly IOperationRepository _operationRepository;

    private readonly IState _innerState;

    public ProxyState(IOperationRepository operationRepository, IState innerState)
    {
        _operationRepository = operationRepository;
        _innerState = innerState;
    }

    public Account? Login(AccountData accountData)
    {
        return _innerState.Login(accountData);
    }

    public string? PasswordChange(ILoggedAccount loggedAccount, string newPassword)
    {
        return _innerState.PasswordChange(loggedAccount, newPassword);
    }

    public double? Withdraw(ILoggedAccount loggedAccount, double amount)
    {
        double? result = _innerState.Withdraw(loggedAccount, amount);

        if (result is null)
        {
            return null;
        }

        var saveData = new SaveData(OperationType.Withdraw, amount);
        _operationRepository.SaveToHistory(loggedAccount, saveData);
        return result;
    }

    public double? Deposit(ILoggedAccount loggedAccount, double amount)
    {
        double? result = _innerState.Deposit(loggedAccount, amount);

        if (result is null)
        {
            return null;
        }

        var saveData = new SaveData(OperationType.Deposit, amount);
        _operationRepository.SaveToHistory(loggedAccount, saveData);
        return result;
    }

    public IEnumerable<SaveData>? History(ILoggedAccount loggedAccount)
    {
        IEnumerable<SaveData>? result = _innerState.History(loggedAccount);

        if (result is null)
        {
            return null;
        }

        return result;
    }

    public double? Balance(ILoggedAccount loggedAccount)
    {
        double? result = _innerState.Balance(loggedAccount);

        if (result is null)
        {
            return null;
        }

        return result;
    }

    public Account? CreateAccount(AccountData accountData)
    {
        return _innerState.CreateAccount(accountData);
    }
}