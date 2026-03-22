using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  public class MessageService
  {
    private readonly MessageRepository _messageRepo;
    private readonly UserRepository _userRepo;

    public MessageService(MessageRepository messageRepo, UserRepository userRepo)
    {
      _messageRepo = messageRepo;
      _userRepo = userRepo;
    }

    public Message SendMessage(Guid senderId, Guid receiverId, string content)
    {
      if (_userRepo.GetById(senderId) == null || _userRepo.GetById(receiverId) == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");

      var message = new Message(senderId, receiverId, content);
      _messageRepo.Add(message);
      return message;
    }

    public IReadOnlyList<Message> GetConversation(Guid userId1, Guid userId2)
        => _messageRepo.GetConversation(userId1, userId2);

    public IReadOnlyList<Guid> GetConversationPartners(Guid userId)
        => _messageRepo.GetConversationPartners(userId);

    public int GetUnreadCount(Guid userId) => _messageRepo.GetUnreadCount(userId);

    public void MarkConversationRead(Guid currentUserId, Guid otherUserId)
    {
      var messages = _messageRepo.GetConversation(currentUserId, otherUserId);
      foreach (var msg in messages)
      {
        if (msg.ReceiverId == currentUserId)
          msg.IsRead = true;
      }
    }
  }
}
