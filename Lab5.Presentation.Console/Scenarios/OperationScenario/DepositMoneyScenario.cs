using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.OperationScenario;

public class DepositMoneyScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Deposit Money";

    public DepositMoneyScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        double amount = AnsiConsole.Ask<double>("Enter amount of money to deposit");

        if (amount <= 0)
        {
            AnsiConsole.MarkupLine("[red]Invalid amount.[/]");
            return;
        }

        TransactionResult resultType = _operationService.Deposit(amount);

        string message = resultType switch
        {
            TransactionResult.Success => "Success",
            TransactionResult.Failure => "Not Found",
            TransactionResult.AccessDenied => "Access Denied",
            _ => throw new InvalidOperationException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}