using BT.Social.Core.Repositories;
using BT.Social.Core.Services;

namespace BT.Social.Core.Models
{
  public class BtSocialPlatform : SocialPlatformBase
  {
    public UserService UserService { get; }
    public PostService PostService { get; }
    public InteractionService InteractionService { get; }
    public MessageService MessageService { get; }
    public StoryService StoryService { get; }
    public NotificationService NotificationService { get; }

    private readonly UserRepository _userRepo;
    private readonly PostRepository _postRepo;

    public BtSocialPlatform()
        : base("BT Social", "Монголын нийгмийн сүлжээний платформ")
    {
      _userRepo = new UserRepository();
      _postRepo = new PostRepository();
      var messageRepo = new MessageRepository();
      var storyRepo = new StoryRepository();
      var notifRepo = new NotificationRepository();

      UserService = new UserService(_userRepo);
      PostService = new PostService(_postRepo, _userRepo);
      InteractionService = new InteractionService(_postRepo, _userRepo);
      MessageService = new MessageService(messageRepo, _userRepo);
      StoryService = new StoryService(storyRepo, _userRepo);
      NotificationService = new NotificationService(notifRepo);
    }

    public override User CreateUser(string username, string email, byte age)
    {
      return UserService.Register(username, email, age);
    }

    public override Post CreatePost(Guid authorId, string text)
    {
      return PostService.CreatePost(authorId, text);
    }

    public User? GetUserByUsername(string username)
    {
      return UserService.GetAllUsers().FirstOrDefault(u =>
          u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
  }
}
