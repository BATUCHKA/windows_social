using BT.Social.Core.Enums;
using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  // Like, comment, share үйлдлүүд
  public class InteractionService
  {
    private readonly PostRepository _postRepo;
    private readonly UserRepository _userRepo;

    public InteractionService(PostRepository postRepo, UserRepository userRepo)
    {
      _postRepo = postRepo;
      _userRepo = userRepo;
    }

    public void ReactToPost(Guid postId, Guid userId, ReactionType type)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(userId);
      post.AddReaction(userId, type);
    }

    public Comment CommentOnPost(Guid postId, Guid authorId, string text)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(authorId);

      var comment = new Comment(authorId, text);
      post.AddComment(comment);
      return comment;
    }

    public void SharePost(Guid postId, Guid userId)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(userId);
      post.Share(userId);
    }

    public void ReactToComment(Guid postId, Guid commentId, Guid userId, ReactionType type)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(userId);

      var comment = post.GetComments().FirstOrDefault(c => c.Id == commentId);
      if (comment == null)
        throw new InvalidOperationException("Сэтгэгдэл олдсонгүй.");

      comment.AddReaction(userId, type);
    }

    private Post GetPostOrThrow(Guid postId)
    {
      return _postRepo.GetById(postId)
          ?? throw new InvalidOperationException("Нийтлэл олдсонгүй.");
    }

    private void ValidateUserExists(Guid userId)
    {
      if (_userRepo.GetById(userId) == null)
        throw new InvalidOperationException("Хэрэглэгч олдсонгүй.");
    }
  }
}
