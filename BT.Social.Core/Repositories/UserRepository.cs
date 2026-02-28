using BT.Social.Core.Interfaces;
using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  /// <summary>
  /// Хэрэглэгчийн мэдээллийг удирдах repository.
  /// In-memory хадгалалт ашиглана.
  /// </summary>
  public class UserRepository : IRepository<User>
  {
    private readonly List<User> _users = new();

    /// <inheritdoc/>
    public User? GetById(Guid id)
    {
      return _users.FirstOrDefault(u => u.Id == id);
    }

    /// <inheritdoc/>
    public IReadOnlyList<User> GetAll()
    {
      return _users.AsReadOnly();
    }

    /// <inheritdoc/>
    public void Add(User entity)
    {
      _users.Add(entity);
    }

    /// <inheritdoc/>
    public bool Remove(Guid id)
    {
      var user = GetById(id);
      if (user == null) return false;
      return _users.Remove(user);
    }

    /// <summary>
    /// Хэрэглэгчийн нэрээр хайх.
    /// </summary>
    /// <param name="username">Хайх нэр</param>
    public User? GetByUsername(string username)
    {
      return _users.FirstOrDefault(u =>
          u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
  }
}
