using E_Commmerce.IReposatory;
using E_Commmerce.IReposatory.RepositoryModels;
using E_Commmerce.Models;
using E_Commmerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Cryptography.Xml;

namespace E_Commmerce.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var listVm = _productRepository.GetAll.Select(
                p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Category = p.Category,
                    CategoryId = p.CategoryId,
                    ImageViewModel = new UpdateImageViewModel() { ImageName = p.ImageName ?? "Defualt.png" },
                }
                ).ToList();
            ViewBag.Categories = CategoryDropDownList(_categoryRepository?.GetAll!);
            ViewBag.CategoryIds = _categoryRepository?.GetAll?.Select(c => c.Id).ToList();
            return View(listVm);
        }

        [HttpPost]
        public IActionResult Index(int CatId)
        {
            var listVm = _productRepository?.GetAll?.Select(
                p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Category = p.Category,
                    CategoryId = p.CategoryId,
                    ImageViewModel = new UpdateImageViewModel() { ImageName = p.ImageName ?? "Defualt.png" }
                }
                ).Where(p => p.CategoryId == CatId).ToList();
            ViewBag.Categories = CategoryDropDownList(_categoryRepository?.GetAll!);
            return View(listVm);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = CategoryDropDownList(_categoryRepository.GetAll!);
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Category = _categoryRepository.GetbyId((int)model.CategoryId!)
                };
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = CategoryDropDownList(_categoryRepository.GetAll!);
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var product = _productRepository.GetbyId(id);
            if (product == null)
            {
                return NotFound();
            }
            var ProdcutVm = new ProductViewModel()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Category = _categoryRepository.GetbyId((int)product.CategoryId!),
                ImageViewModel = new UpdateImageViewModel() { ImageName = product.ImageName ?? "Defualt.png" }
            };
            return View(ProdcutVm);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var Product = _productRepository.GetbyId(Id);
            if (Product == null)
            {
                return NotFound();
            }
            var ProductVm = new ProductViewModel()
            {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                Description = Product.Description,
                CategoryId = Product.CategoryId,
                Category = _categoryRepository.GetbyId((int)Product.CategoryId),
                ImageViewModel = new UpdateImageViewModel() { ImageName = Product.ImageName ?? "Defualt.png" }
            };
            ViewBag.Categories = CategoryDropDownList(_categoryRepository.GetAll!);
            return View(ProductVm);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Category = _categoryRepository.GetbyId((int)model.CategoryId!)
                };
                _productRepository.Update(model.Id, product);
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePhoto(ProductImage model)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.GetbyId(model.Id);
                if (product != null)
                {
                    if (model.Image != null)
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages", model.Image.FileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        model.Image.CopyTo(stream);
                        product.ImageName = model.Image.FileName;
                        _productRepository.Update(model.Id, product);
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "No Image Selected !!");
            }
            return RedirectToAction(nameof(Edit), new { Id = model.Id });
        }



        public IActionResult Delete(int Id)
        {
            var product = _productRepository.GetbyId(Id);
            if (product == null)
            {
                return NotFound();
            }
            _productRepository.Remove(product);
            return RedirectToAction("Index");

        }



        private IEnumerable<SelectListItem> CategoryDropDownList(IEnumerable<Category> categories)
        {
            return categories.Select(c => new SelectListItem(text: c.Name, value: c.Id.ToString()));
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserIndex()
        {
            var products = _productRepository?.GetAll?.Select(
                p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Category = _categoryRepository.GetbyId((int)p.CategoryId)
                }).ToList();
            return View(products);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListByCategory(int CategoryId)
        {
            var products = _categoryRepository.GetProducts(CategoryId)
                .Select(p => new ProductViewModel()
                {
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Price = p.Price,
                    Category = _categoryRepository.GetbyId((int)p.CategoryId!),
                    ImageViewModel = new UpdateImageViewModel() { ImageName = p.ImageName ?? "Defualt.png" }
                }).ToList();
            ViewBag.CatName = _categoryRepository?.GetbyId(CategoryId)?.Name;
            ViewBag.CatId = CategoryId;
            ViewBag.Products = products;
            return View(nameof(UserIndex), products);
        }



    }
}
