using Lab5.Application.Contracts;
using Lab5.Application.Models.Accounts;

namespace Lab5.Application;

public class LoggedAccount : ILoggedAccount
{
    public Account? Account { get; set; }
}