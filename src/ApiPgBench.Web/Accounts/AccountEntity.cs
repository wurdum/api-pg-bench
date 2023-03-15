using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Accounts;

public class AccountEntity : IEntity
{
    public long Id { get; set; }
    public long FamilyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public AccountGroup AccountGroup { get; set; }

    public AccountViewModel ToViewModel()
    {
        return new()
        {
            Id = Id,
            FamilyId = FamilyId,
            Name = Name,
            Currency = Currency,
            AccountGroup = AccountGroup
        };
    }

    public static AccountEntity From(AccountViewModel viewModel)
    {
        return new()
        {
            Id = viewModel.Id,
            FamilyId = viewModel.FamilyId,
            Name = viewModel.Name,
            Currency = viewModel.Currency,
            AccountGroup = viewModel.AccountGroup
        };
    }
}
