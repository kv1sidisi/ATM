using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class LoginScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Login";

    public LoginScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        string username = AnsiConsole.Ask<string>("Enter your username");

        string password = AnsiConsole.Ask<string>("Enter your password");

        LoginResult result = _operationService.Login(username, password);

        string message = result switch
        {
            LoginResult.Success => "Success",
            LoginResult.Failure => "Account was not found",
            LoginResult.NotFound => "User was not found",
            _ => throw new InvalidOperationException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }
}