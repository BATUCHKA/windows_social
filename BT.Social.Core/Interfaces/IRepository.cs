namespace BT.Social.Core.Interfaces
{
  /// <summary>
  /// Ерөнхий (Generic) repository интерфэйс.
  /// Өгөгдлийн хандалтын давхаргыг нэгдмэл болгоно.
  /// </summary>
  /// <typeparam name="T">Entity-ийн төрөл</typeparam>
  public interface IRepository<T>
  {
    /// <summary>
    /// ID-аар нэг entity олж буцаана.
    /// </summary>
    /// <param name="id">Хайх ID</param>
    /// <returns>Олдсон entity, олдохгүй бол null</returns>
    T? GetById(Guid id);

    /// <summary>
    /// Бүх entity-үүдийг буцаана.
    /// </summary>
    IReadOnlyList<T> GetAll();

    /// <summary>
    /// Шинэ entity нэмэх.
    /// </summary>
    /// <param name="entity">Нэмэх entity</param>
    void Add(T entity);

    /// <summary>
    /// Entity устгах.
    /// </summary>
    /// <param name="id">Устгах entity-ийн ID</param>
    /// <returns>Амжилттай устгасан эсэх</returns>
    bool Remove(Guid id);
  }
}
