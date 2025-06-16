using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class LoginScenarioProvider : IScenarioProvider
{
    private readonly IOperationService _operationService;

    public LoginScenarioProvider(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_operationService.LoggedAccount.Account is not null)
        {
            scenario = null;
            return false;
        }

        scenario = new LoginScenario(_operationService);
        return true;
    }
}