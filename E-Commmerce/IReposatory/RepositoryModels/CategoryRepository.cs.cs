using E_Commmerce.Data;
using E_Commmerce.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace E_Commmerce.IReposatory.RepositoryModels;

public class CategoryRepository : ICategoryRepository
{

    private readonly ApplicationDbcontext _dbcontext;

    public CategoryRepository (ApplicationDbcontext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public IEnumerable<Category>? GetAll => _dbcontext.Categories.Include(c => c.Products).ToList();

    public void Add(Category entity)
    {
       if(entity!=null)
        {
            _dbcontext.Categories.Add(entity);
            _dbcontext.SaveChanges();
            return;
        }
       throw new ArgumentNullException(nameof(entity));
    }

    public Category? GetbyId(int id)
                        => _dbcontext.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
    

    public List<Product> GetProducts(int CategoryId)
    {
        var list = _dbcontext.Products.Where(p=>p.CategoryId == CategoryId).ToList();
        return list;
    }

    public void Remove(Category entity)
    {
        if(entity!=null)
        {
            _dbcontext.Categories.Remove(entity);
            _dbcontext.SaveChanges();
            return;
        }
        throw new ArgumentNullException(nameof(entity));
    }

    public List<Product> Search(int CategoryId, string SearchTerm)
    {
        var list = _dbcontext
            .Products
            .Where(p => p.CategoryId == CategoryId &&
                (p.Name.Contains(SearchTerm) ||
                 p.Description.Contains(SearchTerm) ||
                 p.Price.ToString().Contains(SearchTerm)
            )).ToList();
        return list;
    }

    public void Update(int Id, Category entity)
    {
       var categrory = _dbcontext.Categories.Find(Id);
        var oldPicture = categrory.ImageName;
        if(categrory!=null)
        {
            categrory.Name = entity.Name;
            categrory.Description = entity.Description;
            if(categrory.ImageName!=null || categrory.ImageName!="Defualt.png")
            {
                categrory.ImageName = oldPicture;
            }
            else
            {
                categrory.ImageName = entity.ImageName;
            }
            _dbcontext.SaveChanges();
            return;
        }
        throw new ArgumentNullException(nameof(entity));
    }
}
