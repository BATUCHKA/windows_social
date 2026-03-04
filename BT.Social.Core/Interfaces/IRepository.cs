namespace BT.Social.Core.Interfaces
{
  public interface IRepository<T>
  {
    T? GetById(Guid id);
    IReadOnlyList<T> GetAll();
    void Add(T entity);
    bool Remove(Guid id);
  }
}
