
using Microsoft.AspNetCore.Identity;

namespace ApiDotNet_WithReact.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    public async Task<bool> Authenticate(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

        return result.Succeeded;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> RegisterUser(string email, string password)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        } 

        return result.Succeeded;
    }
}
