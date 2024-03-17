namespace Labb2_DbFirst_Template.Interfaces;

public interface IRepository<T, in TId>
{
    T GetById(TId id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}