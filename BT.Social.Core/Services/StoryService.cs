using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  public class StoryService
  {
    private readonly StoryRepository _storyRepo;
    private readonly UserRepository _userRepo;

    public StoryService(StoryRepository storyRepo, UserRepository userRepo)
    {
      _storyRepo = storyRepo;
      _userRepo = userRepo;
    }

    public Story CreateStory(Guid authorId, string text)
    {
      if (_userRepo.GetById(authorId) == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");

      var story = new Story(authorId, text);
      _storyRepo.Add(story);
      return story;
    }

    public IReadOnlyList<Story> GetActiveStories() => _storyRepo.GetActiveStories();

    public IReadOnlyList<Story> GetUserStories(Guid userId) => _storyRepo.GetUserStories(userId);
  }
}
