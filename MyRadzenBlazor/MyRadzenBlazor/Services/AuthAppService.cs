namespace MyRadzenBlazor.Services
{
    public class AuthAppService
    {
        private readonly string _validUsername = "admin";
        private readonly string _validPassword = "admin";

        public Task<bool> ValidateLoginAsync(string username, string password)
        {
            bool isValid = username == _validUsername && password == _validPassword;
            return Task.FromResult(isValid);
        }
    }
}
