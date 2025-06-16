namespace Lab5.Application.Contracts.Results;

public record Balance(OperationResult OperationResult, double? BalanceValue = null);