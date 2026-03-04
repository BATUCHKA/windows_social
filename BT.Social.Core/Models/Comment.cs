using BT.Social.Core.Enums;
using BT.Social.Core.Interfaces;

namespace BT.Social.Core.Models
{
  public class Comment : ContentBase, ILikeable
  {
    private readonly List<Reaction> _reactions = new();

    public Comment(Guid authorId, string text)
        : base(authorId, text, PrivacyLevel.Public) { }

    public void AddReaction(Guid userId, ReactionType type)
    {
      RemoveReaction(userId);
      _reactions.Add(new Reaction(userId, type));
    }

    public void RemoveReaction(Guid userId) => _reactions.RemoveAll(r => r.UserId == userId);

    public int GetReactionCount() => _reactions.Count;

    public override string ToString() => $"    [{GetReactionCount()} reaction] {Text}";
  }
}
