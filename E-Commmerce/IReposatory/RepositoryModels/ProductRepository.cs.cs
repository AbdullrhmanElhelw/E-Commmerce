using E_Commmerce.Data;
using E_Commmerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commmerce.IReposatory.RepositoryModels
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbcontext _dbcontext;

        public ProductRepository(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Product>? GetAll => _dbcontext.Products.Include(p => p.Category).ToList();

        public void Add(Product entity)
        {
            if (entity != null)
            {
                _dbcontext.Products.Add(entity);
                _dbcontext.SaveChanges();
                return;
            }
            throw new NullReferenceException(nameof(entity));
        }

        public Product? GetbyId(int id)
        {
            return _dbcontext.Products.Find(id);
        }

        public void Remove(Product entity)
        {
            if (entity != null)
            {
                _dbcontext.Remove(entity);
                _dbcontext.SaveChanges();
                return;
            }
            throw new NullReferenceException(nameof(entity));

        }



        public void Update(int Id, Product entity)
        {
            var product = _dbcontext.Products.Find(Id);
            var oldPicture = product.ImageName;
            if (product != null)
            {
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.Description = entity.Description;
                product.CategoryId = entity.CategoryId;
                product.Category = entity.Category;
                product.CategoryId = entity.CategoryId;
                if (product.ImageName != null || product.ImageName != "Defualt.png")
                {
                    product.ImageName = oldPicture;
                }
                else
                {
                    product.ImageName = entity.ImageName;
                }
                _dbcontext.SaveChanges();
                return;
            }
            throw new NullReferenceException(nameof(entity));
        }
    }
}
