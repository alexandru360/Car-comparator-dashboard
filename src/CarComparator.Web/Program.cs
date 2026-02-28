using CarComparator.Shared.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CarComparator.Web;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5000";

builder.Services
    .AddRefitClient<ICarApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));

await builder.Build().RunAsync();

