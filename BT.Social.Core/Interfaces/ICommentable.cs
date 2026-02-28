using BT.Social.Core.Models;

namespace BT.Social.Core.Interfaces
{
  /// <summary>
  /// Сэтгэгдэл бичих боломжтой контентын интерфэйс.
  /// </summary>
  public interface ICommentable
  {
    /// <summary>
    /// Сэтгэгдэл нэмэх.
    /// </summary>
    /// <param name="comment">Нэмэх сэтгэгдэл</param>
    void AddComment(Comment comment);

    /// <summary>
    /// Сэтгэгдэл устгах.
    /// </summary>
    /// <param name="commentId">Устгах сэтгэгдлийн ID</param>
    void RemoveComment(Guid commentId);

    /// <summary>
    /// Бүх сэтгэгдлүүдийг буцаана.
    /// </summary>
    IReadOnlyList<Comment> GetComments();
  }
}
