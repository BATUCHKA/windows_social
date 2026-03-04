namespace BT.Social.Core.Models
{
  // Платформын суурь класс
  // Жишээ нь Facebook, Instagram гэх мэт платформууд үүнээс удамшина
  public abstract class SocialPlatformBase
  {
    public string PlatformName { get; }
    public string Description { get; }
    public DateTime CreatedAt { get; }

    protected SocialPlatformBase(string platformName, string description)
    {
      PlatformName = platformName;
      Description = description;
      CreatedAt = DateTime.Now;
    }

    public abstract User CreateUser(string username, string email, byte age);
    public abstract Post CreatePost(Guid authorId, string text);

    public override string ToString() => $"[{PlatformName}] - {Description}";
  }
}
