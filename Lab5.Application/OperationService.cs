using Lab5.Application.Abstractions;
using Lab5.Application.Contracts;
using Lab5.Application.Contracts.Results;
using Lab5.Application.Models.Accounts;
using Lab5.Application.States;

namespace Lab5.Application;

public class OperationService : IOperationService
{
    public ILoggedAccount LoggedAccount { get; private set; }

    private readonly IOperationRepository _operationRepository;

    private IState _state;

    public OperationService(ILoggedAccount loggedAccount, IOperationRepository operationRepository)
    {
        LoggedAccount = loggedAccount;
        _operationRepository = operationRepository;
        _state = new InitState(_operationRepository);
    }

    public void MockSetState(IState state)
    {
        _state = state;
    }

    public LoginResult Login(string username, string password)
    {
        var accountData = new AccountData(username, password, null);
        Account? loginResult = _state.Login(accountData);
        if (loginResult is null)
        {
            return new LoginResult.NotFound();
        }

        LoggedAccount.Account = loginResult;

        if (LoggedAccount.Account.Role == AccountRole.Client)
        {
            _state = new ProxyState(_operationRepository, new ClientState(_operationRepository));
        }
        else if (LoggedAccount.Account.Role == AccountRole.Admin)
        {
            _state = new ProxyState(_operationRepository, new AdminState(_operationRepository));
        }
        else
        {
            return new LoginResult.Failure();
        }

        return new LoginResult.Success();
    }

    public LoginResult Logout()
    {
        LoggedAccount.Account = null;
        _state = new InitState(_operationRepository);
        return new LoginResult.Success();
    }

    public OperationResult PasswordChange(string password)
    {
        if (LoggedAccount.Account == null)
        {
            return new OperationResult.AccessDenied();
        }

        string? passwordChangeResult = _state.PasswordChange(LoggedAccount, password);

        return passwordChangeResult is null ? new OperationResult.Failure() : new OperationResult.Success();
    }

    public TransactionResult Withdraw(double amount)
    {
        if (LoggedAccount.Account == null)
        {
            return new TransactionResult.AccessDenied();
        }

        double? withdrawResult = _state.Withdraw(LoggedAccount, amount);

        return withdrawResult is null ? new TransactionResult.Failure() : new TransactionResult.Success();
    }

    public TransactionResult Deposit(double amount)
    {
        if (LoggedAccount.Account == null)
        {
            return new TransactionResult.AccessDenied();
        }

        double? depositResult = _state.Deposit(LoggedAccount, amount);

        return depositResult is null ? new TransactionResult.Failure() : new TransactionResult.Success();
    }

    public History History()
    {
        if (LoggedAccount.Account == null)
        {
            return new History(new OperationResult.AccessDenied());
        }

        IEnumerable<SaveData>? historyResult = _state.History(LoggedAccount);

        return historyResult is null
            ? new History(new OperationResult.Failure())
            : new History(new OperationResult.Success(), historyResult);
    }

    public Balance Balance()
    {
        if (LoggedAccount.Account == null)
        {
            return new Balance(new OperationResult.AccessDenied());
        }

        double? balanceResult = _state.Balance(LoggedAccount);

        return balanceResult is null
            ? new Balance(new OperationResult.Failure())
            : new Balance(new OperationResult.Success(), balanceResult);
    }

    public OperationResult CreateAccount(string username, string password, AccountRole? role)
    {
        var newAccountData = new AccountData(username, password, role);
        Account? createAccountResult = _state.CreateAccount(newAccountData);

        return createAccountResult is null ? new OperationResult.Failure() : new OperationResult.Success();
    }
}