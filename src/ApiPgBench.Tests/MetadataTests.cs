using System.Net;
using System.Net.Http.Json;
using ApiPgBench.Tests.Infrastructure;
using ApiPgBench.Web.Metadata;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ApiPgBench.Tests;

public class MetadataTests : ServiceTest
{
    public MetadataTests(TestFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Theory]
    [InlineData("families")]
    [InlineData("accounts")]
    [InlineData("transactions")]
    public async Task GetCount_TableNameIsValid_ReturnsValidValue(string tableName)
    {
        var response = await Fixture.Client.GetAsync($"/counts/{tableName}");

        response.EnsureSuccessStatusCode();

        var count = await response.Content.ReadFromJsonAsync<CountViewModel>();

        count!.Value.Should().BeGreaterOrEqualTo(0);
    }

    [Fact]
    public async Task GetCount_TableNameIsInvalid_ReturnsProblem()
    {
        var response = await Fixture.Client.GetAsync("/counts/foo");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
