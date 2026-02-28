using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  /// <summary>
  /// Хэрэглэгчтэй холбоотой бизнес логикийг удирдах service.
  /// </summary>
  public class UserService
  {
    private readonly UserRepository _userRepo;

    /// <summary>
    /// UserService үүсгэх.
    /// </summary>
    /// <param name="userRepo">Хэрэглэгчийн repository</param>
    public UserService(UserRepository userRepo)
    {
      _userRepo = userRepo;
    }

    /// <summary>
    /// Шинэ хэрэглэгч бүртгэх.
    /// </summary>
    /// <param name="username">Хэрэглэгчийн нэр</param>
    /// <param name="email">Имэйл хаяг</param>
    /// <param name="age">Нас</param>
    /// <returns>Үүсгэсэн хэрэглэгч</returns>
    public User Register(string username, string email, byte age)
    {
      // Нэр давхцаж байгаа эсэхийг шалгана
      if (_userRepo.GetByUsername(username) != null)
        throw new InvalidOperationException($"'{username}' нэр аль хэдийн бүртгэлтэй байна.");

      var user = new User(username, email, age);
      _userRepo.Add(user);
      return user;
    }

    /// <summary>
    /// Хэрэглэгчийн мэдээлэл авах.
    /// </summary>
    /// <param name="userId">Хэрэглэгчийн ID</param>
    public User? GetProfile(Guid userId)
    {
      return _userRepo.GetById(userId);
    }

    /// <summary>
    /// Хоёр хэрэглэгчийг найз болгох (харилцан).
    /// </summary>
    /// <param name="userId1">Эхний хэрэглэгчийн ID</param>
    /// <param name="userId2">Хоёр дахь хэрэглэгчийн ID</param>
    public void AddFriend(Guid userId1, Guid userId2)
    {
      var user1 = _userRepo.GetById(userId1);
      var user2 = _userRepo.GetById(userId2);

      if (user1 == null || user2 == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");

      // Харилцан найз болгоно
      user1.AddFriend(userId2);
      user2.AddFriend(userId1);
    }

    /// <summary>
    /// Найзын холбоосыг устгах (харилцан).
    /// </summary>
    public void RemoveFriend(Guid userId1, Guid userId2)
    {
      var user1 = _userRepo.GetById(userId1);
      var user2 = _userRepo.GetById(userId2);

      if (user1 == null || user2 == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");

      user1.RemoveFriend(userId2);
      user2.RemoveFriend(userId1);
    }

    /// <summary>
    /// Бүх хэрэглэгчдийг буцаана.
    /// </summary>
    public IReadOnlyList<User> GetAllUsers() => _userRepo.GetAll();
  }
}
