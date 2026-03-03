using BT.Social.Core.Repositories;
using BT.Social.Core.Services;

namespace BT.Social.Core.Models
{
  /// <summary>
  /// BT Social платформ - SocialPlatformBase-аас удамшсан бодит хэрэгжүүлэлт.
  /// Service, Repository-уудыг нэгтгэн удирдана.
  /// Өөр платформ үүсгэхдээ мөн SocialPlatformBase-аас удамшуулна.
  /// </summary>
  public class BtSocialPlatform : SocialPlatformBase
  {
    /// <summary>Хэрэглэгчийн service</summary>
    public UserService UserService { get; }

    /// <summary>Нийтлэлийн service</summary>
    public PostService PostService { get; }

    /// <summary>Харилцан үйлдлийн service</summary>
    public InteractionService InteractionService { get; }

    private readonly UserRepository _userRepo;
    private readonly PostRepository _postRepo;

    /// <summary>
    /// BT Social платформ үүсгэх.
    /// Repository болон Service-үүдийг автоматаар тохируулна.
    /// </summary>
    public BtSocialPlatform()
        : base("BT Social", "Монголын нийгмийн сүлжээний платформ")
    {
      _userRepo = new UserRepository();
      _postRepo = new PostRepository();

      UserService = new UserService(_userRepo);
      PostService = new PostService(_postRepo, _userRepo);
      InteractionService = new InteractionService(_postRepo, _userRepo);
    }

    /// <inheritdoc/>
    public override User CreateUser(string username, string email, byte age)
    {
      return UserService.Register(username, email, age);
    }

    /// <inheritdoc/>
    public override Post CreatePost(Guid authorId, string text)
    {
      return PostService.CreatePost(authorId, text);
    }
  }
}
