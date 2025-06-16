namespace Lab5.Application.Contracts;

public abstract record OperationResult
{
    private OperationResult() { }

    public sealed record Success : OperationResult;

    public sealed record AccessDenied : OperationResult;

    public sealed record Failure : OperationResult;
}