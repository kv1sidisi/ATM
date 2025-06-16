using Lab5.Application.Models.Accounts;

namespace Lab5.Application.Contracts;

public record AccountData(string Username, string Password, AccountRole? Role);