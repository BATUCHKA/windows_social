using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  public class NotificationService
  {
    private readonly NotificationRepository _notifRepo;

    public NotificationService(NotificationRepository notifRepo)
    {
      _notifRepo = notifRepo;
    }

    public void Notify(Guid userId, Guid fromUserId, NotificationType type, string text)
    {
      if (userId == fromUserId) return; // don't notify yourself
      _notifRepo.Add(new Notification(userId, fromUserId, type, text));
    }

    public IReadOnlyList<Notification> GetNotifications(Guid userId)
        => _notifRepo.GetForUser(userId);

    public int GetUnreadCount(Guid userId) => _notifRepo.GetUnreadCount(userId);

    public void MarkAllRead(Guid userId) => _notifRepo.MarkAllRead(userId);
  }
}
