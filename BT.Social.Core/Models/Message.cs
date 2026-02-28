namespace BT.Social.Core.Models
{
  /// <summary>
  /// Хэрэглэгчид хоорондын хувийн зурвас.
  /// </summary>
  public class Message
  {
    /// <summary>Зурвасын өвөрмөц танигч</summary>
    public Guid Id { get; }

    /// <summary>Илгээгчийн ID</summary>
    public Guid SenderId { get; }

    /// <summary>Хүлээн авагчийн ID</summary>
    public Guid ReceiverId { get; }

    /// <summary>Зурвасын агуулга</summary>
    public string Content { get; }

    /// <summary>Илгээсэн огноо</summary>
    public DateTime SentAt { get; }

    /// <summary>Уншсан эсэх</summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Шинэ зурвас үүсгэх.
    /// </summary>
    /// <param name="senderId">Илгээгчийн ID</param>
    /// <param name="receiverId">Хүлээн авагчийн ID</param>
    /// <param name="content">Зурвасын агуулга</param>
    public Message(Guid senderId, Guid receiverId, string content)
    {
      Id = Guid.NewGuid();
      SenderId = senderId;
      ReceiverId = receiverId;
      Content = content;
      SentAt = DateTime.Now;
      IsRead = false;
    }

    /// <summary>Зурвасыг текст хэлбэрээр буцаана</summary>
    public override string ToString()
    {
      string readStatus = IsRead ? "уншсан" : "шинэ";
      return $"[{readStatus}] {Content}";
    }
  }
}
