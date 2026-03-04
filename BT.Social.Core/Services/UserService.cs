using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  public class UserService
  {
    private readonly UserRepository _userRepo;

    public UserService(UserRepository userRepo)
    {
      _userRepo = userRepo;
    }

    // шинэ хэрэглэгч бүртгэх
    public User Register(string username, string email, byte age)
    {
      if (_userRepo.GetByUsername(username) != null)
        throw new InvalidOperationException($"'{username}' нэр аль хэдийн бүртгэлтэй байна.");

      var user = new User(username, email, age);
      _userRepo.Add(user);
      return user;
    }

    public User? GetProfile(Guid userId) => _userRepo.GetById(userId);

    // хоёр хэрэглэгчийг найз болгоно (2 талдаа нэмнэ)
    public void AddFriend(Guid userId1, Guid userId2)
    {
      var user1 = _userRepo.GetById(userId1);
      var user2 = _userRepo.GetById(userId2);

      if (user1 == null || user2 == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");

      user1.AddFriend(userId2);
      user2.AddFriend(userId1);
    }

    public void RemoveFriend(Guid userId1, Guid userId2)
    {
      var user1 = _userRepo.GetById(userId1);
      var user2 = _userRepo.GetById(userId2);

      if (user1 == null || user2 == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");

      user1.RemoveFriend(userId2);
      user2.RemoveFriend(userId1);
    }

    public IReadOnlyList<User> GetAllUsers() => _userRepo.GetAll();
  }
}
