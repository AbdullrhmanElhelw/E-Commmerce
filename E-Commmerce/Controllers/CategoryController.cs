using E_Commmerce.Helper;
using E_Commmerce.IReposatory;
using E_Commmerce.Migrations;
using E_Commmerce.Models;
using E_Commmerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Packaging.Core;
using System.ComponentModel.DataAnnotations;

namespace E_Commmerce.Controllers
{

    [Authorize(Roles = nameof(Roles.Admin))]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(int PageNumber = 1)
        {
            var list = _categoryRepository.GetAll.Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageViewModel = new UpdateImageViewModel() { ImageName = c.ImageName ?? "Defualt.png" },
                ProductsCount = c.Products.Count
            }).ToList();

            PageList<CategoryViewModel> Categories = PageList<CategoryViewModel>.Create(list, PageNumber, 5);
            return View(Categories);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                _categoryRepository.Add(category);

                var count = _categoryRepository.GetAll.Count();
                var PageNumber = (int)Math.Ceiling(count / 5d);
                ViewBag.TotalPages = PageNumber;
                return RedirectToAction(nameof(Index), new { PageNumber = PageNumber });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int Id, int PageNumber)
        {
            var Category = _categoryRepository.GetbyId(Id);
            if (Category != null)
            {
                var model = new CategoryViewModel()
                {
                    Id = Category.Id,
                    Name = Category.Name,
                    Description = Category.Description,
                    ImageViewModel = new UpdateImageViewModel() { ImageName = Category.ImageName ?? "Defualt.png" }
                };
                ViewBag.pageIndex = PageNumber;
                return View(model);
            }
            return NotFound();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryViewModel model, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description
                };
                _categoryRepository.Update(model.Id, category);
                return RedirectToAction(nameof(Index), new { PageNumber = PageNumber });
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ChangePhoto(CategoryImage model, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var Category = _categoryRepository.GetbyId(model.Id);
                if (Category != null)
                {
                    if (model.Image != null)
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "CategoryImages", model.Image.FileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        model.Image.CopyTo(stream);
                        Category.ImageName = model.Image.FileName;
                        _categoryRepository.Update(model.Id, Category);
                        return RedirectToAction(nameof(Index), new { PageNumber = PageNumber });
                    }
                }
                ModelState.AddModelError("", "No Image Selected !!");
            }
            return RedirectToAction(nameof(Edit), new { Id = model.Id });
        }


        public IActionResult Delete(int Id, int PageNumber)
        {
            var Category = _categoryRepository.GetbyId(Id);
            if (Category != null)
            {
                _categoryRepository.Remove(Category);
                return RedirectToAction(nameof(Index), new { PageNumber = PageNumber });
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var Category = _categoryRepository.GetbyId(Id);
            if (Category != null)
            {
                var CategoryViewModel = new CategoryViewModel()
                {
                    Id = Category.Id,
                    Name = Category.Name,
                    Description = Category.Description,
                    ImageViewModel = new UpdateImageViewModel { ImageName = Category.ImageName ?? "Defualt.png" },
                    ProductsCount = Category.Products.Count
                };

                return View(CategoryViewModel);
            }
            return NotFound();
        }

        /* public IActionResult        {
             var result = _categoryRepository.Search(term);
             return View("Index",result);
         }*/

        [HttpGet]
        public IActionResult ShowAll(int CategoryId)
        {
            var products = _categoryRepository.GetProducts(CategoryId);
            ViewBag.CatName = _categoryRepository?.GetbyId(CategoryId)?.Name;
            ViewBag.CatId = CategoryId;
            return View(products);
        }


        [HttpPost]
        public IActionResult ShowAll(int catid, string term)
        {
            var list = _categoryRepository.Search(catid, term);
            ViewBag.CatName = _categoryRepository?.GetbyId(catid)?.Name;
            return View(list);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserIndex(int PageNumber = 1)
        {
            var categories = _categoryRepository.GetAll.Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageViewModel = new UpdateImageViewModel() { ImageName = c.ImageName ?? "Defualt.png" }
            }).ToList();

            PageList<CategoryViewModel> categoryViews = PageList<CategoryViewModel>.Create(categories, PageNumber, 3);

            return View(categoryViews);
        }




    }
}
