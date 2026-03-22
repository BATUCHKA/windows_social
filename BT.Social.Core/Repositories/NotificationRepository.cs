using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  public class NotificationRepository
  {
    private readonly List<Notification> _notifications = new();

    public void Add(Notification notification) => _notifications.Add(notification);

    public IReadOnlyList<Notification> GetForUser(Guid userId)
    {
      return _notifications
          .Where(n => n.UserId == userId)
          .OrderByDescending(n => n.CreatedAt)
          .ToList()
          .AsReadOnly();
    }

    public int GetUnreadCount(Guid userId)
    {
      return _notifications.Count(n => n.UserId == userId && !n.IsRead);
    }

    public void MarkAllRead(Guid userId)
    {
      foreach (var n in _notifications.Where(n => n.UserId == userId))
        n.IsRead = true;
    }
  }
}
