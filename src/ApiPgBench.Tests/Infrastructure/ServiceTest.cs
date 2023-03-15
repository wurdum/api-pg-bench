using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace ApiPgBench.Tests.Infrastructure;

[Collection(nameof(ServiceTestCollection))]
public abstract class ServiceTest
{
    protected ServiceTest(TestFixture fixture, ITestOutputHelper output)
    {
        Fixture = fixture;
        Output = output;
    }

    public TestFixture Fixture { get; }
    public ITestOutputHelper Output { get; }

    public ILogger<T> GetLogger<T>()
    {
        return new OutputLogger<T>(this);
    }

    private class OutputLogger<T> : ILogger<T>
    {
        private readonly ServiceTest _serviceTest;
        private readonly LogLevel _minimumLogLevel;

        public OutputLogger(ServiceTest serviceTest, LogLevel minimumLogLevel = LogLevel.Information)
        {
            _serviceTest = serviceTest;
            _minimumLogLevel = minimumLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return new DummyDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minimumLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _serviceTest.Output.WriteLine($"{DateTime.UtcNow:O} [{logLevel}]: {formatter(state, exception)}");
        }

        private class DummyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}

[CollectionDefinition(nameof(ServiceTestCollection))]
public class ServiceTestCollection : ICollectionFixture<TestFixture>
{
}
