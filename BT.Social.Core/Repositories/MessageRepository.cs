using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  public class MessageRepository
  {
    private readonly List<Message> _messages = new();

    public void Add(Message message) => _messages.Add(message);

    public IReadOnlyList<Message> GetConversation(Guid userId1, Guid userId2)
    {
      return _messages
          .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2)
                   || (m.SenderId == userId2 && m.ReceiverId == userId1))
          .OrderBy(m => m.SentAt)
          .ToList()
          .AsReadOnly();
    }

    public IReadOnlyList<Message> GetInbox(Guid userId)
    {
      return _messages
          .Where(m => m.ReceiverId == userId)
          .OrderByDescending(m => m.SentAt)
          .ToList()
          .AsReadOnly();
    }

    public int GetUnreadCount(Guid userId)
    {
      return _messages.Count(m => m.ReceiverId == userId && !m.IsRead);
    }

    public IReadOnlyList<Guid> GetConversationPartners(Guid userId)
    {
      return _messages
          .Where(m => m.SenderId == userId || m.ReceiverId == userId)
          .Select(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
          .Distinct()
          .ToList()
          .AsReadOnly();
    }
  }
}
