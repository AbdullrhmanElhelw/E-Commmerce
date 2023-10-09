namespace E_Commmerce.IReposatory;

public interface IRepository<T>where T:class
{
    T? GetbyId (int id);
    IEnumerable<T>? GetAll { get;  }

    void Add(T entity);
    void Remove(T entity);
    void Update(int Id ,T entity);
}
