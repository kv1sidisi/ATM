using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class ChangePasswordScenarioProvider : IScenarioProvider
{
    private readonly IOperationService _operationService;

    public ChangePasswordScenarioProvider(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_operationService.LoggedAccount.Account is null)
        {
            scenario = null;
            return false;
        }

        scenario = new ChangePasswordScenario(_operationService);
        return true;
    }
}