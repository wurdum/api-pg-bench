using ApiPgBench.Tests.Infrastructure;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ApiPgBench.Tests;

public class FamilyTests : ServiceTest
{
    public FamilyTests(TestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public async Task CreateUser_Always_Creates()
    {
        var family = FakeData.CreateFamily();

        var createdFamily = await Fixture.CreateFamilyAsync(family);

        createdFamily.Should().BeEquivalentTo(family, o => o.Excluding(u => u.Id));
    }

    [Fact]
    public async Task FindUser_HasUser_ReturnsUser()
    {
        var family = await Fixture.CreateFamilyAsync(FakeData.CreateFamily());

        var foundFamily = await Fixture.FindFamilyAsync(family.Id);

        foundFamily.Should().BeEquivalentTo(family);
    }
}
