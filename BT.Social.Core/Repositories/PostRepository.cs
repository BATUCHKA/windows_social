using BT.Social.Core.Interfaces;
using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  // Нийтлэлүүдийг хадгалах repository
  public class PostRepository : IRepository<Post>
  {
    private readonly List<Post> _posts = new();

    public Post? GetById(Guid id) => _posts.FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<Post> GetAll() => _posts.AsReadOnly();

    public void Add(Post entity) => _posts.Add(entity);

    public bool Remove(Guid id)
    {
      var post = GetById(id);
      if (post == null) return false;
      return _posts.Remove(post);
    }

    // тухайн хэрэглэгчийн бүх постыг авах
    public IReadOnlyList<Post> GetByAuthorId(Guid authorId)
    {
      return _posts.Where(p => p.AuthorId == authorId).ToList().AsReadOnly();
    }
  }
}
