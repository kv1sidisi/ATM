namespace Lab5.Application.Contracts.Results;

public record History(OperationResult OperationResult, IEnumerable<SaveData>? Data = null);