namespace ApiPgBench.Web.Transactions;

public class TransactionViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
