using BT.Social.Core.Models;

namespace BT.Social.Core.Interfaces
{
  public interface ICommentable
  {
    void AddComment(Comment comment);
    void RemoveComment(Guid commentId);
    IReadOnlyList<Comment> GetComments();
  }
}
