using DiegoMoreno.ChartOfAccountsApi.Domain.Entities.Base;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
public class Account : Entity
{
    public Guid? IdParentAccount { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public Guid IdAccountType { get; set; }
    public bool AcceptEntries { get; set; }

    public virtual AccountType AccountType { get; set; }
}
