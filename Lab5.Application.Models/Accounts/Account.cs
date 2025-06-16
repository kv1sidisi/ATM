namespace Lab5.Application.Models.Accounts;

public record Account(long Id, string Username, double Balance, string Password, AccountRole Role);