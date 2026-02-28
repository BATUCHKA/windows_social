namespace BT.Social.Core.Models
{
  /// <summary>
  /// Нийгмийн сүлжээний платформын суурь abstract класс.
  /// Facebook, Instagram, X зэрэг платформууд үүнээс удамшиж,
  /// өөрийн хэрэгжүүлэлтийг бичнэ.
  /// </summary>
  public abstract class SocialPlatformBase
  {
    /// <summary>Платформын нэр</summary>
    public string PlatformName { get; }

    /// <summary>Платформын тайлбар</summary>
    public string Description { get; }

    /// <summary>Платформ үүссэн огноо</summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// SocialPlatformBase-ийн constructor.
    /// </summary>
    /// <param name="platformName">Платформын нэр</param>
    /// <param name="description">Платформын тайлбар</param>
    protected SocialPlatformBase(string platformName, string description)
    {
      PlatformName = platformName;
      Description = description;
      CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Шинэ хэрэглэгч бүртгэх. Удамшсан класс хэрэгжүүлнэ.
    /// </summary>
    public abstract User CreateUser(string username, string email, byte age);

    /// <summary>
    /// Шинэ нийтлэл үүсгэх. Удамшсан класс хэрэгжүүлнэ.
    /// </summary>
    public abstract Post CreatePost(Guid authorId, string text);

    /// <summary>
    /// Платформын мэдээллийг текст хэлбэрээр буцаана.
    /// </summary>
    public override string ToString()
    {
      return $"[{PlatformName}] - {Description}";
    }
  }
}
