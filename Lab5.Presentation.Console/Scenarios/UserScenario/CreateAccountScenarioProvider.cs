using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Console.Scenarios.UserScenario;

public class CreateAccountScenarioProvider : IScenarioProvider
{
    private readonly IOperationService _operationService;

    public CreateAccountScenarioProvider(IOperationService operationService)
    {
        _operationService = operationService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_operationService.LoggedAccount.Account is null || _operationService.LoggedAccount.Account.Role != AccountRole.Admin)
        {
            scenario = null;
            return false;
        }

        scenario = new CreateAccountScenario(_operationService);
        return true;
    }
}