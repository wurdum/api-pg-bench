namespace ApiPgBench.Web.Accounts;

public enum AccountGroup
{
    Unspecified,
    Cash,
    Bank,
    Savings,
    Investment
}

public class AccountViewModel
{
    public long Id { get; set; }
    public long FamilyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public AccountGroup AccountGroup { get; set; }
}
