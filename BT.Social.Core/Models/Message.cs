namespace BT.Social.Core.Models
{
  // Хувийн зурвас
  public class Message
  {
    public Guid Id { get; }
    public Guid SenderId { get; }
    public Guid ReceiverId { get; }
    public string Content { get; }
    public DateTime SentAt { get; }
    public bool IsRead { get; set; }

    public Message(Guid senderId, Guid receiverId, string content)
    {
      Id = Guid.NewGuid();
      SenderId = senderId;
      ReceiverId = receiverId;
      Content = content;
      SentAt = DateTime.Now;
      IsRead = false;
    }

    public override string ToString()
    {
      string s = IsRead ? "уншсан" : "шинэ";
      return $"[{s}] {Content}";
    }
  }
}
