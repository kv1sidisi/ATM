using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Lab5.Application.Contracts.Results;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.OperationScenario;

public class ShowHistoryScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Show History";

    public ShowHistoryScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        History result = _operationService.History();

        string message = result.OperationResult switch
        {
            OperationResult.Success => PrintHistory(result),
            OperationResult.Failure => "Not Found",
            OperationResult.AccessDenied => "Access Denied",
            _ => throw new InvalidOperationException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }

    private string PrintHistory(History result)
    {
        return result.Data is null
            ? "No data"
            : string.Join(
            Environment.NewLine,
            result.Data.Select(operation => $"{operation.OperationType} ({operation.Value ?? 0:F2})"));
    }
}