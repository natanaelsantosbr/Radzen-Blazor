using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyRadzenBlazor.Client.Services;
using Radzen;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();