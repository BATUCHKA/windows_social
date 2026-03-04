using BT.Social.Core.Enums;
using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  public class PostService
  {
    private readonly PostRepository _postRepo;
    private readonly UserRepository _userRepo;

    public PostService(PostRepository postRepo, UserRepository userRepo)
    {
      _postRepo = postRepo;
      _userRepo = userRepo;
    }

    public Post CreatePost(Guid authorId, string text, PrivacyLevel privacy = PrivacyLevel.Public)
    {
      if (_userRepo.GetById(authorId) == null)
        throw new InvalidOperationException("Зохиогч олдсонгүй.");

      var post = new Post(authorId, text, privacy);
      _postRepo.Add(post);
      return post;
    }

    public bool DeletePost(Guid postId) => _postRepo.Remove(postId);

    // feed - шинэ нийтлэл эхэнд
    public IReadOnlyList<Post> GetFeed()
    {
      return _postRepo.GetAll()
          .OrderByDescending(p => p.CreatedAt)
          .ToList()
          .AsReadOnly();
    }

    public IReadOnlyList<Post> GetUserPosts(Guid authorId) => _postRepo.GetByAuthorId(authorId);
  }
}
