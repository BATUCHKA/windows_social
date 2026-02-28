using BT.Social.Core.Enums;

namespace BT.Social.Core.Interfaces
{
  /// <summary>
  /// Хариу үйлдэл (reaction) өгөх боломжтой контентын интерфэйс.
  /// Post, Comment зэрэг классууд хэрэгжүүлнэ.
  /// </summary>
  public interface ILikeable
  {
    /// <summary>
    /// Хариу үйлдэл нэмэх.
    /// </summary>
    /// <param name="userId">Хэрэглэгчийн ID</param>
    /// <param name="type">Хариу үйлдлийн төрөл</param>
    void AddReaction(Guid userId, ReactionType type);

    /// <summary>
    /// Хариу үйлдэл устгах.
    /// </summary>
    /// <param name="userId">Хэрэглэгчийн ID</param>
    void RemoveReaction(Guid userId);

    /// <summary>
    /// Нийт хариу үйлдлийн тоог буцаана.
    /// </summary>
    int GetReactionCount();
  }
}
