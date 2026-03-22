using BT.Social.Core.Models;

namespace BT.Social.Core.Repositories
{
  public class StoryRepository
  {
    private readonly List<Story> _stories = new();

    public void Add(Story story) => _stories.Add(story);

    public IReadOnlyList<Story> GetActiveStories()
    {
      return _stories
          .Where(s => !s.IsExpired)
          .OrderByDescending(s => s.CreatedAt)
          .ToList()
          .AsReadOnly();
    }

    public IReadOnlyList<Story> GetUserStories(Guid userId)
    {
      return _stories
          .Where(s => s.AuthorId == userId && !s.IsExpired)
          .OrderByDescending(s => s.CreatedAt)
          .ToList()
          .AsReadOnly();
    }
  }
}
