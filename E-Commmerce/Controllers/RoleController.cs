using E_Commmerce.Models;
using E_Commmerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace E_Commmerce.Controllers

{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleRepository;
        public RoleController(RoleManager<IdentityRole> roleRepository )
        {
            _roleRepository = roleRepository;
        }

        public IActionResult Index()
        {
            var list = _roleRepository.Roles.Select(r => new RoleViewModel()
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return View(list);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.Name
                };
                var result = _roleRepository.CreateAsync(identityRole).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public  IActionResult Delete(string Id)
        {
           var role = _roleRepository.FindByIdAsync(Id).Result;
            if(role != null)
            {
                var result = _roleRepository.DeleteAsync(role).Result;
                if(result.Succeeded)
                {
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
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var role = _roleRepository.FindByIdAsync(Id).Result;
            var model = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = _roleRepository.FindByIdAsync(model.Id).Result;
                if(role!=null)
                {
                    role.Name = model.Name;
                    var result = _roleRepository.UpdateAsync(role).Result;
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
            }
            return View(model);
        }




    }
}
