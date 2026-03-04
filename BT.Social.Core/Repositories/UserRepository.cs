using BT.Social.Core.Interfaces;
using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  // Хэрэглэгчдийн мэдээллийг санах ойд хадгалах repository
  public class UserRepository : IRepository<User>
  {
    private readonly List<User> _users = new();

    public User? GetById(Guid id) => _users.FirstOrDefault(u => u.Id == id);

    public IReadOnlyList<User> GetAll() => _users.AsReadOnly();

    public void Add(User entity) => _users.Add(entity);

    public bool Remove(Guid id)
    {
      var user = GetById(id);
      if (user == null) return false;
      return _users.Remove(user);
    }

    // нэрээр хайх
    public User? GetByUsername(string username)
    {
      return _users.FirstOrDefault(u =>
          u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
  }
}
