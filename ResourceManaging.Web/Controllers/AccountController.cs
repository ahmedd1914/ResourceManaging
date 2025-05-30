using Microsoft.AspNetCore.Mvc;
using ResourceManaging.Services.DTOs.Authentication;
using ResourceManaging.Services.Interfaces.Authentication;
using ResourceManaging.Web.Models;

namespace ResourceManaging.Web.Controllers
{
    public class AccountController : Controller
    {
       private readonly IAuthenticationService _authenticationService;


       public AccountController(IAuthenticationService authenticationService){
        _authenticationService = authenticationService;
       }

       [HttpGet]
       public IActionResult Login(string returnUrl = "/"){
        return View(new LoginViewModel{
            ReturnUrl = returnUrl
        });
       }

       [HttpPost]
       public async Task<IActionResult> Login(LoginViewModel model){
        if(!ModelState.IsValid)
          return View(model);

          var result = await _authenticationService.LoginAsync(new LoginRequest{
                Username = model.Username,
                Password = model.Password
          });
          if(result.Success){
            HttpContext.Session.SetInt32("UserId", result.EmployeeId);
            HttpContext.Session.SetString("UserName", result.FullName);

           if (!string.IsNullOrEmpty(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }
         ViewData["ErrorMessage"] = result.Message ?? "Invalid username or password";
        return View(model);    
       }


    [HttpGet]
    public IActionResult Register(){
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model){
        if(!ModelState.IsValid)
          return View(model);

        var result = await _authenticationService.RegisterAsync(new RegisterRequest {
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
            FullName = model.FullName,
            DateOfBirth = model.DateOfBirth
        });

        if(result.Success) {
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        ViewData["ErrorMessage"] = result.Message ?? "Registration failed";
        return View(model);
    }
    

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }       
    
}
}


