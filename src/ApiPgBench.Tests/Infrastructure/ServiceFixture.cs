using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ApiPgBench.Tests.Infrastructure;

public class ServiceFixture : WebApplicationFactory<Program>
{
    public ServiceFixture()
    {
        RestClient = CreateClient();
    }

    public HttpClient RestClient { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OverrideConfiguration:Here"] = "NewValue"
            });
        });
    }
}
