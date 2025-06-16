using Lab5.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IOperationService, OperationService>();

        collection.AddScoped<LoggedAccount>();
        collection.AddScoped<ILoggedAccount>(
            p => p.GetRequiredService<LoggedAccount>());

        return collection;
    }
}