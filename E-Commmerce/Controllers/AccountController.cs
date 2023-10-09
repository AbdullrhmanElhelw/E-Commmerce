using E_Commmerce.Helper;
using E_Commmerce.Models;
using E_Commmerce.ViewModels;
using E_Commmerce.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.Diagnostics;
using System.Net.Mail;

namespace E_Commmerce.Controllers;


[Authorize(Roles = nameof(Roles.Admin))]
public  class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager,
        IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index(int? PageNumber)
    {
        var list = _userManager.Users.ToList().Select(u => new UserViewModel()
        {
            Id = u.Id,
            Name = u.FirstName + " " + u.LastName,
            Email = u.Email,
            UserName = u.UserName,
            Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "User"
        }
        ).ToList();

        PageList<UserViewModel> users = PageList<UserViewModel>.Create(list, PageNumber ?? 1, 3); 
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.LastPage = LastPage();
        return View();
    }


    [HttpPost]
    public IActionResult Create(CreateUserViewModel model)
    {
        if(ModelState.IsValid)
        {
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ImageName = new UpdateImageViewModel().ImageName ?? "Defualt.jpg",
                UserName = new MailAddress(model.Email).User
            };
            var result = _userManager.CreateAsync(user, model.Password).Result;
            if(result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, model.IsAdmin ? nameof(Roles.Admin):nameof(Roles.User)).Wait();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(model);
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult Register(UserRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ImageName = "Defualt.jpg",
                UserName = new MailAddress(model.Email).User,
            };
            var result =_userManager.CreateAsync(user, model.Password).Result;
            if(result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, nameof(Roles.User)).Wait();
                   _signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(Login));
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
      return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Profile()
    {
        var user = _userManager.FindByNameAsync(User?.Identity?.Name!).Result;
        if(user!=null)
        {
            var model = new ProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                ImageName = user.ImageName ?? "Defualt.jpg"
            };
            
            return View(model);
        }
        return NotFound();
        
    }
    
    // create and save picture 
    // assign this pictuer to the user 
    // redirt to an action

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public  IActionResult Profile(ProfileViewModel model)
    {
        var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
        if(user!=null)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Username;

            var result = _userManager.UpdateAsync(user).Result;
            if(result.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult UpdatePhoto(UpdateImageViewModel model)
    {
        var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
        if(ModelState.IsValid)
        {
            if(model.Image!=null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "UserProfileImages", model.Image.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(stream);
                user.ImageName = model.Image.FileName;
                var result = _userManager.UpdateAsync(user).Result;
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Profile));

                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            ModelState.AddModelError("", "File Not Found");
        }
        return RedirectToAction(nameof(Profile));
    }

    [AllowAnonymous]
    public IActionResult RemovePhoto()
    {
        var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
        if(user!=null)
        {
            if(user.ImageName!=null || user.ImageName != "Default.jpg")
            {
                user.ImageName = "Defualt.jpg";
                var result = _userManager.UpdateAsync(user).Result;
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    foreach(var error in result.Errors)
                        ModelState.AddModelError("" , error.Description);
                }
            }
        }
        return RedirectToAction(nameof(Profile));
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]

    public async Task<IActionResult> Login(UserLoginViewModel model)
    {
        if(ModelState.IsValid)
        {
           var user =  await _userManager.FindByEmailAsync(model.Email);
           if(user!=null)
            {
                bool found = await _userManager.CheckPasswordAsync(user, model.Password);
                if(found)
                {
                    await _signInManager.SignInAsync(user, false);
                    user.ImageName = new UpdateImageViewModel().ImageName ?? "Defualt.jpg";
                    return RedirectToAction("UserIndex", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Password");
                }
            }
           else
            {
                ModelState.AddModelError("", "Invalid Email Or Username");
            }
        }
        return View(model);
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Delete(string Id)
    {
        var user = _userManager.FindByIdAsync(Id).Result;
        if(user!= null)
        {
            
            var result =  _userManager.DeleteAsync(user).Result;
            if(result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
        }
        return NotFound();
    }




    private int LastPage()
    {
        int count = _userManager.Users.Count();
        return (int)Math.Ceiling(count / 3d);
    }


}
