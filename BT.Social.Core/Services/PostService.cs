using BT.Social.Core.Enums;
using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  /// <summary>
  /// Нийтлэлтэй холбоотой бизнес логикийг удирдах service.
  /// </summary>
  public class PostService
  {
    private readonly PostRepository _postRepo;
    private readonly UserRepository _userRepo;

    /// <summary>
    /// PostService үүсгэх.
    /// </summary>
    public PostService(PostRepository postRepo, UserRepository userRepo)
    {
      _postRepo = postRepo;
      _userRepo = userRepo;
    }

    /// <summary>
    /// Шинэ нийтлэл үүсгэх.
    /// </summary>
    /// <param name="authorId">Зохиогчийн ID</param>
    /// <param name="text">Нийтлэлийн текст</param>
    /// <param name="privacy">Нууцлалын түвшин</param>
    /// <returns>Үүсгэсэн нийтлэл</returns>
    public Post CreatePost(Guid authorId, string text, PrivacyLevel privacy = PrivacyLevel.Public)
    {
      if (_userRepo.GetById(authorId) == null)
        throw new InvalidOperationException("Зохиогч олдсонгүй.");

      var post = new Post(authorId, text, privacy);
      _postRepo.Add(post);
      return post;
    }

    /// <summary>
    /// Нийтлэл устгах.
    /// </summary>
    public bool DeletePost(Guid postId)
    {
      return _postRepo.Remove(postId);
    }

    /// <summary>
    /// Нийтийн feed - бүх нийтлэлийг шинэ нь эхэнд байхаар буцаана.
    /// </summary>
    public IReadOnlyList<Post> GetFeed()
    {
      return _postRepo.GetAll()
          .OrderByDescending(p => p.CreatedAt)
          .ToList()
          .AsReadOnly();
    }

    /// <summary>
    /// Тодорхой хэрэглэгчийн нийтлэлүүдийг буцаана.
    /// </summary>
    public IReadOnlyList<Post> GetUserPosts(Guid authorId)
    {
      return _postRepo.GetByAuthorId(authorId);
    }
  }
}
