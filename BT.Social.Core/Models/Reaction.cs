using BT.Social.Core.Enums;

namespace BT.Social.Core.Models
{
  public class Reaction
  {
    public Guid UserId { get; }
    public ReactionType Type { get; }
    public DateTime CreatedAt { get; }

    public Reaction(Guid userId, ReactionType type)
    {
      UserId = userId;
      Type = type;
      CreatedAt = DateTime.Now;
    }
  }
}
