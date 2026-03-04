using BT.Social.Core.Enums;

namespace BT.Social.Core.Interfaces
{
  public interface ILikeable
  {
    void AddReaction(Guid userId, ReactionType type);
    void RemoveReaction(Guid userId);
    int GetReactionCount();
  }
}
