using E_Commmerce.Models;

namespace E_Commmerce.IReposatory;

public interface ICategoryRepository : IRepository<Category>
{
    List<Product> GetProducts(int CategoryId);
    List<Product> Search(int CategoryId, string SearchTerm);
}
