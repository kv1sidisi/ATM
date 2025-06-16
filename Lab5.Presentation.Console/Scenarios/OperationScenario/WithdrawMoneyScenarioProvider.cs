using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;
using Lab5.Application.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Console.Scenarios.OperationScenario;

public class WithdrawMoneyScenarioProvider : IScenarioProvider
{
    private readonly IOperationService _operationService;

    public WithdrawMoneyScenarioProvider(IOperationService operationService)
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

        scenario = new WithdrawMoneyScenario(_operationService);
        return true;
    }
}