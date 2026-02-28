using BT.Social.Core.Interfaces;
using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  /// <summary>
  /// Нийтлэлийн мэдээллийг удирдах repository.
  /// In-memory хадгалалт ашиглана.
  /// </summary>
  public class PostRepository : IRepository<Post>
  {
    private readonly List<Post> _posts = new();

    /// <inheritdoc/>
    public Post? GetById(Guid id)
    {
      return _posts.FirstOrDefault(p => p.Id == id);
    }

    /// <inheritdoc/>
    public IReadOnlyList<Post> GetAll()
    {
      return _posts.AsReadOnly();
    }

    /// <inheritdoc/>
    public void Add(Post entity)
    {
      _posts.Add(entity);
    }

    /// <inheritdoc/>
    public bool Remove(Guid id)
    {
      var post = GetById(id);
      if (post == null) return false;
      return _posts.Remove(post);
    }

    /// <summary>
    /// Тодорхой хэрэглэгчийн бүх нийтлэлийг буцаана.
    /// </summary>
    /// <param name="authorId">Зохиогчийн ID</param>
    public IReadOnlyList<Post> GetByAuthorId(Guid authorId)
    {
      return _posts.Where(p => p.AuthorId == authorId).ToList().AsReadOnly();
    }
  }
}
