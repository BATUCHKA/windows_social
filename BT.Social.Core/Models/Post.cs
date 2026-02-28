using BT.Social.Core.Enums;
using BT.Social.Core.Interfaces;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// Нийтлэл. ContentBase-аас удамшиж, ILikeable, ICommentable, IShareable хэрэгжүүлнэ.
  /// Энэ нь abstract class + олон interface-ийг хослуулсан жишээ.
  /// </summary>
  public class Post : ContentBase, ILikeable, ICommentable, IShareable
  {
    private readonly List<Reaction> _reactions = new();
    private readonly List<Comment> _comments = new();
    private readonly List<Guid> _sharedByUserIds = new();

    /// <summary>
    /// Шинэ нийтлэл үүсгэх.
    /// </summary>
    /// <param name="authorId">Зохиогчийн ID</param>
    /// <param name="text">Нийтлэлийн текст</param>
    /// <param name="privacy">Нууцлалын түвшин</param>
    public Post(Guid authorId, string text, PrivacyLevel privacy = PrivacyLevel.Public)
        : base(authorId, text, privacy)
    {
    }

    // ==================== ILikeable ====================

    /// <inheritdoc/>
    public void AddReaction(Guid userId, ReactionType type)
    {
      // Нэг хэрэглэгч нэг л reaction өгнө - хуучныг устгаад шинээр нэмнэ
      RemoveReaction(userId);
      _reactions.Add(new Reaction(userId, type));
    }

    /// <inheritdoc/>
    public void RemoveReaction(Guid userId)
    {
      _reactions.RemoveAll(r => r.UserId == userId);
    }

    /// <inheritdoc/>
    public int GetReactionCount() => _reactions.Count;

    // ==================== ICommentable ====================

    /// <inheritdoc/>
    public void AddComment(Comment comment)
    {
      _comments.Add(comment);
    }

    /// <inheritdoc/>
    public void RemoveComment(Guid commentId)
    {
      _comments.RemoveAll(c => c.Id == commentId);
    }

    /// <inheritdoc/>
    public IReadOnlyList<Comment> GetComments() => _comments.AsReadOnly();

    // ==================== IShareable ====================

    /// <inheritdoc/>
    public void Share(Guid userId)
    {
      if (!_sharedByUserIds.Contains(userId))
        _sharedByUserIds.Add(userId);
    }

    /// <inheritdoc/>
    public int GetShareCount() => _sharedByUserIds.Count;

    /// <summary>Нийтлэлийг текст хэлбэрээр буцаана</summary>
    public override string ToString()
    {
      return $"\"{Text}\" | {GetReactionCount()} reactions, {_comments.Count} comments, {GetShareCount()} shares";
    }
  }
}
