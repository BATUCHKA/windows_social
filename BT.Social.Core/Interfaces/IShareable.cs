namespace BT.Social.Core.Interfaces
{
  /// <summary>
  /// Хуваалцах боломжтой контентын интерфэйс.
  /// </summary>
  public interface IShareable
  {
    /// <summary>
    /// Контентыг хуваалцах.
    /// </summary>
    /// <param name="userId">Хуваалцаж буй хэрэглэгчийн ID</param>
    void Share(Guid userId);

    /// <summary>
    /// Нийт хуваалцсан тоог буцаана.
    /// </summary>
    int GetShareCount();
  }
}
