using BT.Social.Core.Enums;
using BT.Social.Core.Interfaces;

namespace BT.Social.Core.Models
{
  // 24 цагийн дараа устдаг контент (Instagram story шиг)
  public class Story : ContentBase, ILikeable
  {
    public DateTime ExpiresAt { get; }
    public bool IsExpired => DateTime.Now > ExpiresAt;

    private readonly List<Reaction> _reactions = new();

    public Story(Guid authorId, string text)
        : base(authorId, text, PrivacyLevel.Public)
    {
      ExpiresAt = CreatedAt.AddHours(24);
    }

    public void AddReaction(Guid userId, ReactionType type)
    {
      RemoveReaction(userId);
      _reactions.Add(new Reaction(userId, type));
    }

    public void RemoveReaction(Guid userId) => _reactions.RemoveAll(r => r.UserId == userId);
    public int GetReactionCount() => _reactions.Count;

    public override string ToString()
    {
      string status = IsExpired ? "хугацаа дууссан" : "идэвхтэй";
      return $"[Story - {status}] \"{Text}\" | {GetReactionCount()} reactions";
    }
  }
}
