using ApiPgBench.Tests.Infrastructure;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ApiPgBench.Tests;

public class TransactionTests : ServiceTest
{
    public TransactionTests(TestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public async Task CreateTransaction_Always_Creates()
    {
        var family = await Fixture.CreateFamilyAsync(FakeData.CreateFamily());
        var account = await Fixture.CreateAccountAsync(FakeData.CreateAccount(family.Id));
        var transaction = FakeData.CreateTransaction(account.Id);

        var createdTransaction = await Fixture.CreateTransactionAsync(transaction);

        createdTransaction.Should().BeEquivalentTo(transaction, o => o.ForEntity().Excluding(u => u.Id));
    }

    [Fact]
    public async Task FindTransaction_HasTransaction_ReturnsTransaction()
    {
        var family = await Fixture.CreateFamilyAsync(FakeData.CreateFamily());
        var account = await Fixture.CreateAccountAsync(FakeData.CreateAccount(family.Id));
        var transaction = await Fixture.CreateTransactionAsync(FakeData.CreateTransaction(account.Id));

        var foundTransaction = await Fixture.FindTransaction(transaction.Id);

        foundTransaction.Should().BeEquivalentTo(transaction, o => o.ForEntity());
    }
}
