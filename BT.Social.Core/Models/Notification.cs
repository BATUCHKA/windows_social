namespace BT.Social.Core.Models
{
  public enum NotificationType
  {
    Reaction,
    Comment,
    Share,
    FriendRequest,
    Message,
    StoryReaction
  }

  public class Notification
  {
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid FromUserId { get; }
    public NotificationType Type { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }
    public bool IsRead { get; set; }

    public Notification(Guid userId, Guid fromUserId, NotificationType type, string text)
    {
      Id = Guid.NewGuid();
      UserId = userId;
      FromUserId = fromUserId;
      Type = type;
      Text = text;
      CreatedAt = DateTime.Now;
      IsRead = false;
    }
  }
}
