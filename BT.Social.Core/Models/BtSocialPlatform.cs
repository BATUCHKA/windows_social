using BT.Social.Core.Repositories;
using BT.Social.Core.Services;

namespace BT.Social.Core.Models
{
  // Манай платформын гол класс
  public class BtSocialPlatform : SocialPlatformBase
  {
    public UserService UserService { get; }
    public PostService PostService { get; }
    public InteractionService InteractionService { get; }

    private readonly UserRepository _userRepo;
    private readonly PostRepository _postRepo;

    public BtSocialPlatform()
        : base("BT Social", "Монголын нийгмийн сүлжээний платформ")
    {
      _userRepo = new UserRepository();
      _postRepo = new PostRepository();

      UserService = new UserService(_userRepo);
      PostService = new PostService(_postRepo, _userRepo);
      InteractionService = new InteractionService(_postRepo, _userRepo);
    }

    public override User CreateUser(string username, string email, byte age)
    {
      return UserService.Register(username, email, age);
    }

    public override Post CreatePost(Guid authorId, string text)
    {
      return PostService.CreatePost(authorId, text);
    }
  }
}
