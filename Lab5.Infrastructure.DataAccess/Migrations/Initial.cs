using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Lab5.Infrastructure.DataAccess.Migrations;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        return $"""
                create table if not exists users
                (
                    user_id bigint primary key generated always as identity,
                    user_name text not null,
                    user_balance bigint not null,
                    user_pin text not null,
                    user_role text not null
                );
                
                create table if not exists operations
                (
                    operation_id bigint primary key generated always as identity,
                    operation_type text not null,
                    user_operation_amount_of_money bigint not null,
                    user_id bigint not null,
                    foreign key (user_id) references users(user_id)
                );
                """;
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        return $"""
                drop table users;
                drop table operations;

                drop type operation_type;
                """;
    }
}