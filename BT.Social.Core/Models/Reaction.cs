using BT.Social.Core.Enums;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// Хариу үйлдэл (Like, Love, Haha гэх мэт).
  /// </summary>
  public class Reaction
  {
    /// <summary>Хариу үйлдэл өгсөн хэрэглэгчийн ID</summary>
    public Guid UserId { get; }

    /// <summary>Хариу үйлдлийн төрөл</summary>
    public ReactionType Type { get; }

    /// <summary>Үүсгэсэн огноо</summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Шинэ хариу үйлдэл үүсгэх.
    /// </summary>
    public Reaction(Guid userId, ReactionType type)
    {
      UserId = userId;
      Type = type;
      CreatedAt = DateTime.Now;
    }
  }
}
