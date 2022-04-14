using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User()
        {
            UserName = model.UserName,
        };

        var creationResult = await _userManager.CreateAsync(user, model.Password);
        if (creationResult.Succeeded)
        {
            _logger.LogInformation("Пользователь {0} успешно зарегистрирован", model.UserName);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in creationResult.Errors)
            ModelState.AddModelError("",error.Description);
        
        var errorInfo = string.Join(", ", creationResult.Errors.Select(e => e.Description));
        _logger.LogWarning("Ошибка при регистрации пользователя {0}", model.UserName);

        return View(model);
    }

    public IActionResult Login(string returnUrl)
    {
        var lvm = new LoginViewModel()
        {
            ReturnUrl = returnUrl
        };
        return View(lvm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var loginResult = await _signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            model.RememberMe,
            true);

        if (loginResult.Succeeded)
        {
            _logger.LogInformation("Пользователь {0} успешно вошёл в систему", model.UserName);
            //return RedirectToAction(model.ReturnUrl); // Небезопасно!!!
            //if (Url.IsLocalUrl(model.ReturnUrl))
            //    return RedirectToAction(model.ReturnUrl);
            //return RedirectToAction("Index", "Home");

            return LocalRedirect(model.ReturnUrl ?? "/");
        }
        //else if (loginResult.RequiresTwoFactor)
        //{
        //    // Выполнить двухфакторную авторизацияю.
        //}
        //else if (!loginResult.IsLockedOut)
        //{
        //    // Отправить пользователю информацию о том, что он заблокирован.
        //}
        ModelState.AddModelError("", "Неверное имя пользователя или пароль");
        _logger.LogWarning("Ошибка входа пользователя {0}", model.UserName);
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity!.Name;
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Пользователь {0} вышел из системы", userName);
        return RedirectToAction("Index", "Home");
    } 

    public IActionResult AccessDenied() => View();

}