namespace ApiDotNet_WithReact.Services
{
    public interface IAuthenticateService
    {
        Task<bool> Authenticate(string email, string password);
        Task Logout();
    }
}
