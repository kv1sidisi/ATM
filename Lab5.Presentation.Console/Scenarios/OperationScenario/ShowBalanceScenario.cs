using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Lab5.Application.Contracts.Results;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.OperationScenario;

public class ShowBalanceScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Show Balance";

    public ShowBalanceScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        Balance result = _operationService.Balance();

        string message = result.OperationResult switch
        {
            OperationResult.Success => PrintBalance(result),
            OperationResult.Failure => "Not Found",
            OperationResult.AccessDenied => "Access Denied",
            _ => throw new InvalidOperationException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }

    private string PrintBalance(Balance result)
    {
        return "Balance: " + result.BalanceValue;
    }
}