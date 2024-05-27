using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyRadzenBlazor.Client.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();

builder.Services.AddSingleton<MockAuthService>();

await builder.Build().RunAsync();
