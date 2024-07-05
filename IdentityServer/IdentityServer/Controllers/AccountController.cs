using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityServerInteractionService _interaction;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IIdentityServerInteractionService interaction)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _interaction = interaction;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect("~/"); // Default redirect
                }
                if (Url.IsLocalUrl(model.ReturnUrl) || IsValidRedirectUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    throw new InvalidOperationException("Invalid return URL");
                }
            }
        }
        return View(model);
    }

    private bool IsValidRedirectUrl(string returnUrl)
    {
        // Add logic to validate the return URL
        return true;
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        var model = new RegisterViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect(model.ReturnUrl ?? "~/");
            }
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        var logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);

        await _signInManager.SignOutAsync();
        if (logoutRequest?.PostLogoutRedirectUri != null)
        {
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        return Redirect("~/");
    }
}
