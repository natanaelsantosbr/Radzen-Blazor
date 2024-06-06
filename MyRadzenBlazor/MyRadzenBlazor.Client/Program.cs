using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyRadzenBlazor.Client.Services;
using Radzen;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddSingleton<MenuService>();
builder.Services.AddSingleton<NavigationStateService>();

await builder.Build().RunAsync();