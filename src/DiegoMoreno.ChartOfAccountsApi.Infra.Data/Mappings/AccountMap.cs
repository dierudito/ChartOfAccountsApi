using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.Data.Mappings;
public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AcceptEntries).IsRequired(true);
        builder.Property(a => a.IdParentAccount).IsRequired(false);
        builder.Property(a => a.IdAccountType).IsRequired(true);
        builder.Property(a => a.Name).HasColumnType("varchar").HasMaxLength(255).IsRequired(true);
        builder.Property(a => a.Code).IsRequired(true);

        builder
            .HasOne(a => a.AccountType)
            .WithMany()
            .HasForeignKey(a => a.IdAccountType);
    }
}
