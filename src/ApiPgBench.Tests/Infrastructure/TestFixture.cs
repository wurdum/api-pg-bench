using System.Net.Http.Json;
using ApiPgBench.Web.Accounts;
using ApiPgBench.Web.Families;
using ApiPgBench.Web.Transactions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ApiPgBench.Tests.Infrastructure;

public class TestFixture : WebApplicationFactory<Program>
{
    public TestFixture()
    {
        Client = CreateClient();
    }

    public HttpClient Client { get; }

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

    public async Task<FamilyViewModel> FindFamilyAsync(long id)
    {
        var viewModel = await Client.GetFromJsonAsync<FamilyViewModel>($"/families/{id}");
        return viewModel!;
    }

    public async Task<FamilyViewModel> CreateFamilyAsync(FamilyViewModel family)
    {
        var response = await Client.PostAsJsonAsync("/families", family);
        var viewModel = await response.Content.ReadFromJsonAsync<FamilyViewModel>();
        return viewModel!;
    }

    public async Task<AccountViewModel> FindAccount(long id)
    {
        var viewModel = await Client.GetFromJsonAsync<AccountViewModel>($"/accounts/{id}");
        return viewModel!;
    }

    public async Task<AccountViewModel> CreateAccountAsync(AccountViewModel account)
    {
        var response = await Client.PostAsJsonAsync("/accounts", account);
        var viewModel = await response.Content.ReadFromJsonAsync<AccountViewModel>();
        return viewModel!;
    }

    public async Task<TransactionViewModel> FindTransaction(long transactionId)
    {
        var viewModel = await Client.GetFromJsonAsync<TransactionViewModel>($"/transactions/{transactionId}");
        return viewModel!;
    }

    public async Task<TransactionViewModel> CreateTransactionAsync(TransactionViewModel transaction)
    {
        var response = await Client.PostAsJsonAsync("/transactions", transaction);
        var viewModel = await response.Content.ReadFromJsonAsync<TransactionViewModel>();
        return viewModel!;
    }
}
