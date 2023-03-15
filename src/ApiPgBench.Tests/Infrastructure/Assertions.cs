using FluentAssertions;
using FluentAssertions.Equivalency;

namespace ApiPgBench.Tests.Infrastructure;

public static class Assertions
{
    public static EquivalencyAssertionOptions<TEntity> ForEntity<TEntity>(this EquivalencyAssertionOptions<TEntity> options)
    {
        return options
            .Using<DateTime>(context => context.Subject.Should().BeCloseTo(context.Expectation, TimeSpan.FromTicks(100)))
            .WhenTypeIs<DateTime>();
    }
}
