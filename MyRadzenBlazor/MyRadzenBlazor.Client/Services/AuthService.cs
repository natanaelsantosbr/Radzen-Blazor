using Microsoft.AspNetCore.Components;
using MyRadzenBlazor.Client.Models;
using Radzen;
using System.Net.Http.Json;

namespace MyRadzenBlazor.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly NotificationService _notificationService;

        public AuthService(HttpClient httpClient, NavigationManager navigationManager, NotificationService notificationService)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _notificationService = notificationService;
        }

        public async Task LoginAsync(LoginArgs args)
        {
            var loginRequest = new LoginRequest { Username = args.Username, Password = args.Password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Login Successful",
                    Detail = $"Welcome {args.Username}!",
                    Duration = 1000
                });

                _navigationManager.NavigateTo("/");
            }
            else
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Login Failed",
                    Detail = "Invalid username or password",
                    Duration = 1000
                });
            }
        }
    }
}
