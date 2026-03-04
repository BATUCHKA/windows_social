using BT.Social.Core.Enums;
using BT.Social.Core.Interfaces;

namespace BT.Social.Core.Models
{
  // Нийтлэл - like, comment, share хийх боломжтой
  public class Post : ContentBase, ILikeable, ICommentable, IShareable
  {
    private readonly List<Reaction> _reactions = new();
    private readonly List<Comment> _comments = new();
    private readonly List<Guid> _sharedByUserIds = new();

    public Post(Guid authorId, string text, PrivacyLevel privacy = PrivacyLevel.Public)
        : base(authorId, text, privacy) { }

    // ILikeable
    public void AddReaction(Guid userId, ReactionType type)
    {
      // хуучин reaction-г арилгаад шинээр нэмнэ
      RemoveReaction(userId);
      _reactions.Add(new Reaction(userId, type));
    }

    public void RemoveReaction(Guid userId) => _reactions.RemoveAll(r => r.UserId == userId);
    public int GetReactionCount() => _reactions.Count;

    // ICommentable
    public void AddComment(Comment comment) => _comments.Add(comment);

    public void RemoveComment(Guid commentId) => _comments.RemoveAll(c => c.Id == commentId);

    public IReadOnlyList<Comment> GetComments() => _comments.AsReadOnly();

    // IShareable
    public void Share(Guid userId)
    {
      if (!_sharedByUserIds.Contains(userId))
        _sharedByUserIds.Add(userId);
    }

    public int GetShareCount() => _sharedByUserIds.Count;

    public override string ToString()
    {
      return $"\"{Text}\" | {GetReactionCount()} reactions, {_comments.Count} comments, {GetShareCount()} shares";
    }
  }
}
