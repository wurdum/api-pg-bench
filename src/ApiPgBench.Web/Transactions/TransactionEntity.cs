using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Transactions;

public class TransactionEntity : IEntity
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }

    public TransactionViewModel ToViewModel()
    {
        return new()
        {
            Id = Id,
            AccountId = AccountId,
            Description = Description,
            Category = Category,
            Amount = Amount,
            CreatedAt = CreatedAt
        };
    }

    public static TransactionEntity From(TransactionViewModel viewModel)
    {
        return new()
        {
            Id = viewModel.Id,
            AccountId = viewModel.AccountId,
            Description = viewModel.Description,
            Category = viewModel.Category,
            Amount = viewModel.Amount,
            CreatedAt = viewModel.CreatedAt
        };
    }
}
