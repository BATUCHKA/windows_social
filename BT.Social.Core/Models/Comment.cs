using BT.Social.Core.Enums;
using BT.Social.Core.Interfaces;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// Сэтгэгдэл. ContentBase-аас удамшиж, ILikeable хэрэгжүүлнэ.
  /// </summary>
  public class Comment : ContentBase, ILikeable
  {
    private readonly List<Reaction> _reactions = new();

    /// <summary>
    /// Шинэ сэтгэгдэл үүсгэх.
    /// </summary>
    /// <param name="authorId">Зохиогчийн ID</param>
    /// <param name="text">Сэтгэгдлийн текст</param>
    public Comment(Guid authorId, string text)
        : base(authorId, text, PrivacyLevel.Public)
    {
    }

    /// <inheritdoc/>
    public void AddReaction(Guid userId, ReactionType type)
    {
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

    /// <summary>Сэтгэгдлийг текст хэлбэрээр буцаана</summary>
    public override string ToString()
    {
      return $"    [{GetReactionCount()} reaction] {Text}";
    }
  }
}
