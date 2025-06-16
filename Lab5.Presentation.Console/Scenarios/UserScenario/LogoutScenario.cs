using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class LogoutScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Logout";

    public LogoutScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        LoginResult result = _operationService.Logout();

        string message = result switch
        {
            LoginResult.Success => "Succeess",
            LoginResult.Failure => "Server failure",
            _ => throw new InvalidOperationException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}