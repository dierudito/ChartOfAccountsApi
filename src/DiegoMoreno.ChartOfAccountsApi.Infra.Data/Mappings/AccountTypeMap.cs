using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.Data.Mappings;

public class AccountTypeMap : IEntityTypeConfiguration<AccountType>
{
    public void Configure(EntityTypeBuilder<AccountType> builder)
    {
        builder.ToTable("AccountTypes");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
    }
}
