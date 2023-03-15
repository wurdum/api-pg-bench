using ApiPgBench.Web.Accounts;
using ApiPgBench.Web.Families;
using ApiPgBench.Web.Transactions;
using Bogus;

namespace ApiPgBench.Tests.Infrastructure;

public static class FakeData
{
    private static readonly Faker<FamilyViewModel> FamilyFaker = new Faker<FamilyViewModel>()
        .RuleFor(x => x.Id, f => f.Random.Long())
        .RuleFor(x => x.Name, f => f.Name.FullName());

    private static readonly Faker<AccountViewModel> AccountFaker = new Faker<AccountViewModel>()
        .RuleFor(x => x.Id, f => f.Random.Long())
        .RuleFor(x => x.Name, f => f.Random.String2(30))
        .RuleFor(x => x.Currency, f => f.Finance.Currency().Code)
        .RuleFor(x => x.AccountGroup, f => f.Random.Enum<AccountGroup>());

    private static readonly Faker<TransactionViewModel> TransactionFaker = new Faker<TransactionViewModel>()
        .RuleFor(x => x.Id, f => f.Random.Long())
        .RuleFor(x => x.AccountId, f => f.Random.Long())
        .RuleFor(x => x.Description, f => f.Random.String2(30))
        .RuleFor(x => x.Category, f => f.Random.String2(30))
        .RuleFor(x => x.Amount, f => f.Random.Decimal(10, 1000))
        .RuleFor(x => x.CreatedAt, f => f.Date.Past());

    public static FamilyViewModel CreateFamily()
    {
        return FamilyFaker.Generate();
    }

    public static AccountViewModel CreateAccount(long familyId)
    {
        var viewModel = AccountFaker.Generate();
        viewModel.FamilyId = familyId;
        return viewModel;
    }

    public static TransactionViewModel CreateTransaction(long accountId)
    {
        var viewModel = TransactionFaker.Generate();
        viewModel.AccountId = accountId;
        return viewModel;
    }
}
