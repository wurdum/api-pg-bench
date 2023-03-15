using ApiPgBench.Tests.Infrastructure;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ApiPgBench.Tests;

public class AccountTests : ServiceTest
{
    public AccountTests(TestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public async Task CreateAccount_Always_Creates()
    {
        var family = await Fixture.CreateFamilyAsync(FakeData.CreateFamily());
        var user = FakeData.CreateAccount(family.Id);

        var createdAccount = await Fixture.CreateAccountAsync(user);

        createdAccount.Should().BeEquivalentTo(user, o => o.Excluding(u => u.Id));
    }

    [Fact]
    public async Task FindAccount_HasAccount_ReturnsAccount()
    {
        var family = await Fixture.CreateFamilyAsync(FakeData.CreateFamily());
        var account = await Fixture.CreateAccountAsync(FakeData.CreateAccount(family.Id));

        var foundAccount = await Fixture.FindAccount(account.Id);

        foundAccount.Should().BeEquivalentTo(account);
    }
}
