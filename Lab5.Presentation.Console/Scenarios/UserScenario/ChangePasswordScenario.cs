using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class ChangePasswordScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Change password";

    public ChangePasswordScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        string password = AnsiConsole.Ask<string>("Enter new password");

        if (!IsPasswordValid(password))
        {
            AnsiConsole.MarkupLine("[red]Invalid password.[/]");
            return;
        }

        OperationResult result = _operationService.PasswordChange(password);

        string message = result switch
        {
            OperationResult.Success => "Success",
            OperationResult.Failure => "Not Found",
            OperationResult.AccessDenied => "Access Denied",
            _ => throw new InvalidOperationException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.Ask<string>("Ok");
    }

    private bool IsPasswordValid(string password)
    {
        return password.Length > 12 && password.Any(char.IsDigit);
    }
}