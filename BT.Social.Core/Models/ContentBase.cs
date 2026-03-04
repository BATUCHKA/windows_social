using BT.Social.Core.Enums;

namespace BT.Social.Core.Models
{
  // Контентын суурь класс (Post, Comment, Story удамшина)
  public abstract class ContentBase
  {
    public Guid Id { get; }
    public Guid AuthorId { get; }
    public string Text { get; set; }
    public PrivacyLevel Privacy { get; set; }
    public DateTime CreatedAt { get; }

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
