using BT.Social.Core.Enums;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// Нийгмийн сүлжээний хэрэглэгч.
  /// </summary>
  public class User
  {
    /// <summary>Хэрэглэгчийн өвөрмөц танигч</summary>
    public Guid Id { get; }

    /// <summary>Хэрэглэгчийн нэр</summary>
    public string Username { get; set; }

    /// <summary>Имэйл хаяг</summary>
    public string Email { get; set; }

    /// <summary>
    /// Хэрэглэгчийн нас. readonly, byte төрөл (0-255).
    /// Нас нь бүртгүүлэх үед тогтоогдоно.
    /// </summary>
    public byte Age { get; }

    /// <summary>Хэрэглэгчийн товч танилцуулга</summary>
    public string Bio { get; set; }

    /// <summary>Бүртгэлийн төлөв</summary>
    public AccountStatus Status { get; set; }

    /// <summary>Бүртгүүлсэн огноо</summary>
    public DateTime CreatedAt { get; }

    private readonly List<Guid> _friendIds = new();

    /// <summary>Найзуудын ID жагсаалт (зөвхөн унших)</summary>
    public IReadOnlyList<Guid> FriendIds => _friendIds.AsReadOnly();

    /// <summary>
    /// Шинэ хэрэглэгч үүсгэх.
    /// </summary>
    /// <param name="username">Хэрэглэгчийн нэр</param>
    /// <param name="email">Имэйл хаяг</param>
    /// <param name="age">Нас (byte)</param>
    public User(string username, string email, byte age)
    {
      Id = Guid.NewGuid();
      Username = username;
      Email = email;
      Age = age;
      Bio = string.Empty;
      Status = AccountStatus.Active;
      CreatedAt = DateTime.Now;
    }

    /// <summary>Найз нэмэх</summary>
    /// <param name="friendId">Найзын ID</param>
    public void AddFriend(Guid friendId)
    {
      if (!_friendIds.Contains(friendId))
        _friendIds.Add(friendId);
    }

    /// <summary>Найз устгах</summary>
    /// <param name="friendId">Найзын ID</param>
    public void RemoveFriend(Guid friendId)
    {
      _friendIds.Remove(friendId);
    }

    /// <summary>Хэрэглэгчийн мэдээллийг текст хэлбэрээр буцаана</summary>
    public override string ToString()
    {
      return $"{Username} (нас: {Age}, найз: {_friendIds.Count})";
    }
  }
}
