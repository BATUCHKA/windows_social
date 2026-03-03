using BT.Social.Core.Enums;
using BT.Social.Core.Models;
using BT.Social.Core.Repositories;

namespace BT.Social.Core.Services
{
  /// <summary>
  /// Like, Comment, Share зэрэг харилцан үйлдлүүдийг удирдах service.
  /// </summary>
  public class InteractionService
  {
    private readonly PostRepository _postRepo;
    private readonly UserRepository _userRepo;

    /// <summary>
    /// InteractionService үүсгэх.
    /// </summary>
    public InteractionService(PostRepository postRepo, UserRepository userRepo)
    {
      _postRepo = postRepo;
      _userRepo = userRepo;
    }

    /// <summary>
    /// Нийтлэл дээр reaction өгөх.
    /// </summary>
    /// <param name="postId">Нийтлэлийн ID</param>
    /// <param name="userId">Хэрэглэгчийн ID</param>
    /// <param name="type">Reaction-ий төрөл</param>
    public void ReactToPost(Guid postId, Guid userId, ReactionType type)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(userId);
      post.AddReaction(userId, type);
    }

    /// <summary>
    /// Нийтлэл дээр сэтгэгдэл бичих.
    /// </summary>
    /// <param name="postId">Нийтлэлийн ID</param>
    /// <param name="authorId">Сэтгэгдэл бичигчийн ID</param>
    /// <param name="text">Сэтгэгдлийн текст</param>
    /// <returns>Үүсгэсэн сэтгэгдэл</returns>
    public Comment CommentOnPost(Guid postId, Guid authorId, string text)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(authorId);

      var comment = new Comment(authorId, text);
      post.AddComment(comment);
      return comment;
    }

    /// <summary>
    /// Нийтлэлийг хуваалцах.
    /// </summary>
    /// <param name="postId">Нийтлэлийн ID</param>
    /// <param name="userId">Хуваалцаж буй хэрэглэгчийн ID</param>
    public void SharePost(Guid postId, Guid userId)
    {
      var post = GetPostOrThrow(postId);
      ValidateUserExists(userId);
      post.Share(userId);
    }

    /// <summary>
    /// Сэтгэгдэл дээр reaction өгөх.
    /// </summary>
    /// <param name="postId">Нийтлэлийн ID (сэтгэгдэл агуулсан)</param>
    /// <param name="commentId">Сэтгэгдлийн ID</param>
    /// <param name="userId">Хэрэглэгчийн ID</param>
    /// <param name="type">Reaction-ий төрөл</param>
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
