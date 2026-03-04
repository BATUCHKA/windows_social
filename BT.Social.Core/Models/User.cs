using BT.Social.Core.Enums;

namespace BT.Social.Core.Models
{
  public class User
  {
    public Guid Id { get; }
    public string Username { get; set; }
    public string Email { get; set; }
    public byte Age { get; }  // byte: 0-255 хүртэл, нас хадгалахад тохиромжтой
    public string Bio { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime CreatedAt { get; }

    private readonly List<Guid> _friendIds = new();
    public IReadOnlyList<Guid> FriendIds => _friendIds.AsReadOnly();

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

    public void AddFriend(Guid friendId)
    {
      if (!_friendIds.Contains(friendId))
        _friendIds.Add(friendId);
    }

    public void RemoveFriend(Guid friendId) => _friendIds.Remove(friendId);

    public override string ToString() => $"{Username} (нас: {Age}, найз: {_friendIds.Count})";
  }
}
