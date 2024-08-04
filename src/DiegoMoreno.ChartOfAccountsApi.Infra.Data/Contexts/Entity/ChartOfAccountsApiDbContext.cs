using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.Data.Contexts.Entity;
public class ChartOfAccountsApiDbContext : DbContext
{
    public ChartOfAccountsApiDbContext()
    {
    }

    public ChartOfAccountsApiDbContext(DbContextOptions<ChartOfAccountsApiDbContext> options) : base(options)
    {

    }

    public DbSet<Account> Account { get; set; } = null!;
    public DbSet<AccountType> AccountType { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
