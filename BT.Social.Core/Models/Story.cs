using BT.Social.Core.Enums;
using BT.Social.Core.Interfaces;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// Story - 24 цагийн хугацаатай контент.
  /// ContentBase-аас удамшиж, ILikeable хэрэгжүүлнэ.
  /// </summary>
  public class Story : ContentBase, ILikeable
  {
    /// <summary>Story-ийн дуусах хугацаа (үүсгэснээс 24 цаг)</summary>
    public DateTime ExpiresAt { get; }

    /// <summary>Story хүчинтэй эсэхийг шалгана</summary>
    public bool IsExpired => DateTime.Now > ExpiresAt;

    private readonly List<Reaction> _reactions = new();

    /// <summary>
    /// Шинэ story үүсгэх. Автоматаар 24 цагийн дараа дуусна.
    /// </summary>
    /// <param name="authorId">Зохиогчийн ID</param>
    /// <param name="text">Story текст</param>
    public Story(Guid authorId, string text)
        : base(authorId, text, PrivacyLevel.Public)
    {
      ExpiresAt = CreatedAt.AddHours(24);
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

    /// <summary>Story мэдээллийг текст хэлбэрээр буцаана</summary>
    public override string ToString()
    {
      string status = IsExpired ? "хугацаа дууссан" : "идэвхтэй";
      return $"[Story - {status}] \"{Text}\" | {GetReactionCount()} reactions";
    }
  }
}
