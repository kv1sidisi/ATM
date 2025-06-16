using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;
using Spectre.Console;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class CreateAccountScenario : IScenario
{
    private readonly IOperationService _operationService;

    public string Name => "Create account";

    public CreateAccountScenario(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public void Run()
    {
        string username = AnsiConsole.Ask<string>("Enter your username");
        string password = AnsiConsole.Ask<string>("Enter your password");
        string role = AnsiConsole.Ask<string>("Enter your role");

        AccountRole? parsedRole = DeductRole(role);

        if (parsedRole == null)
        {
            AnsiConsole.MarkupLine("[red]Invalid role.[/]");
            return;
        }

        if (!IsPasswordValid(password))
        {
            AnsiConsole.MarkupLine("[red]Invalid password[/]");
            return;
        }

        OperationResult result = _operationService.CreateAccount(username, password, parsedRole);

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

    private AccountRole? DeductRole(string role)
    {
        return role switch
        {
            "Admin" => AccountRole.Admin,
            "Client" => AccountRole.Client,
            _ => null,
        };
    }

    private bool IsPasswordValid(string password)
    {
        return password.Length > 12 && password.Any(char.IsDigit);
    }
}