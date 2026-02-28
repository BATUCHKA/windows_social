using BT.Social.Core.Enums;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// Бүх контентын суурь abstract класс.
  /// Post, Comment, Story зэрэг классууд үүнээс удамшина.
  /// </summary>
  public abstract class ContentBase
  {
    /// <summary>Контентын өвөрмөц танигч</summary>
    public Guid Id { get; }

    /// <summary>Контент зохиогчийн ID</summary>
    public Guid AuthorId { get; }

    /// <summary>Контентын текст</summary>
    public string Text { get; set; }

    /// <summary>Нууцлалын түвшин</summary>
    public PrivacyLevel Privacy { get; set; }

    /// <summary>Үүсгэсэн огноо (өөрчлөх боломжгүй)</summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// ContentBase-ийн constructor.
    /// </summary>
    /// <param name="authorId">Зохиогчийн ID</param>
    /// <param name="text">Контентын текст</param>
    /// <param name="privacy">Нууцлалын түвшин</param>
    protected ContentBase(Guid authorId, string text, PrivacyLevel privacy = PrivacyLevel.Public)
    {
      Id = Guid.NewGuid();
      AuthorId = authorId;
      Text = text;
      Privacy = privacy;
      CreatedAt = DateTime.Now;
    }
  }
}
