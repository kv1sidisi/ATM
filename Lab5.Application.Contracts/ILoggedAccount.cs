using Lab5.Application.Models.Accounts;

namespace Lab5.Application.Contracts;

public interface ILoggedAccount
{
    Account? Account { get; set; }
}