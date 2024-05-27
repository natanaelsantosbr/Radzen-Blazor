namespace MyRadzenBlazor.Client.Services
{
    public class MockAuthService
    {
        public bool Login(string username, string password)
        {
            return username == "admin" && password == "123456789";
        }
    }
}
