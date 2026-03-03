using BT.Social.Core.Enums;
using BT.Social.Core.Models;

namespace BT.Social.ConsoleApp
{
  /// <summary>
  /// BT Social платформын демо програм.
  /// DLL (BT.Social.Core) сангаас reference хийж ашиглаж байна.
  /// </summary>
  class Program
  {
    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      var platform = new BtSocialPlatform();
      PrintHeader($"Платформ үүслээ: {platform}");

      PrintHeader("ХЭРЭГЛЭГЧИД БҮРТГЭХ");

      var bat = platform.CreateUser("Bat", "bat@email.com", 22);
      var sarnai = platform.CreateUser("Sarnai", "sarnai@email.com", 19);
      var bold = platform.CreateUser("Bold", "bold@email.com", 25);

      Console.WriteLine($"  + {bat}");
      Console.WriteLine($"  + {sarnai}");
      Console.WriteLine($"  + {bold}");

      Console.WriteLine($"\n  Bat-ын нас (byte): {bat.Age}, төрөл: {bat.Age.GetType().Name}");

      PrintHeader("НАЙЗУУД НЭМЭХ");

      platform.UserService.AddFriend(bat.Id, sarnai.Id);
      platform.UserService.AddFriend(bat.Id, bold.Id);

      Console.WriteLine($"  {bat.Username} <-> {sarnai.Username} найз боллоо");
      Console.WriteLine($"  {bat.Username} <-> {bold.Username} найз боллоо");
      Console.WriteLine($"  {bat.Username}-н найзууд: {bat.FriendIds.Count}");

      PrintHeader("НИЙТЛЭЛ ҮҮСГЭХ");

      var post1 = platform.CreatePost(bat.Id, "Өнөөдөр маш сайхан цаг агаар байна!");
      var post2 = platform.CreatePost(sarnai.Id, "Шинэ ном уншиж байна. Маш сонирхолтой!");
      var post3 = platform.CreatePost(bold.Id, "Кодинг бол урлаг.");

      Console.WriteLine($"  [{bat.Username}] {post1}");
      Console.WriteLine($"  [{sarnai.Username}] {post2}");
      Console.WriteLine($"  [{bold.Username}] {post3}");

      PrintHeader("REACTION ӨГӨХ");

      platform.InteractionService.ReactToPost(post1.Id, sarnai.Id, ReactionType.Love);
      platform.InteractionService.ReactToPost(post1.Id, bold.Id, ReactionType.Like);
      platform.InteractionService.ReactToPost(post2.Id, bat.Id, ReactionType.Haha);
      platform.InteractionService.ReactToPost(post3.Id, bat.Id, ReactionType.Wow);
      platform.InteractionService.ReactToPost(post3.Id, sarnai.Id, ReactionType.Love);

      Console.WriteLine($"  Post1 reactions: {post1.GetReactionCount()}");
      Console.WriteLine($"  Post2 reactions: {post2.GetReactionCount()}");
      Console.WriteLine($"  Post3 reactions: {post3.GetReactionCount()}");

      PrintHeader("СЭТГЭГДЭЛ БИЧИХ");

      var comment1 = platform.InteractionService.CommentOnPost(
          post1.Id, sarnai.Id, "Тийм ээ, маш гоё өдөр!");
      var comment2 = platform.InteractionService.CommentOnPost(
          post1.Id, bold.Id, "Гадаа алхахад таатай байх аа.");
      var comment3 = platform.InteractionService.CommentOnPost(
          post3.Id, bat.Id, "100% зөвшөөрч байна!");

      Console.WriteLine($"  Post1 сэтгэгдлүүд ({post1.GetComments().Count}):");
      foreach (var c in post1.GetComments())
        Console.WriteLine($"  {c}");

      Console.WriteLine($"  Post3 сэтгэгдлүүд ({post3.GetComments().Count}):");
      foreach (var c in post3.GetComments())
        Console.WriteLine($"  {c}");

      platform.InteractionService.ReactToComment(post1.Id, comment1.Id, bat.Id, ReactionType.Like);
      Console.WriteLine($"\n  Comment1 reactions: {comment1.GetReactionCount()}");

      PrintHeader("ХУВААЛЦАХ");

      platform.InteractionService.SharePost(post3.Id, bat.Id);
      platform.InteractionService.SharePost(post3.Id, sarnai.Id);

      Console.WriteLine($"  Post3 shares: {post3.GetShareCount()}");

      PrintHeader("FEED (бүх нийтлэлүүд)");

      var feed = platform.PostService.GetFeed();
      int index = 1;
      foreach (var post in feed)
      {
        var author = platform.UserService.GetProfile(post.AuthorId);
        Console.WriteLine($"  {index}. [{author?.Username}] {post}");

        if (post.GetComments().Count > 0)
        {
          foreach (var comment in post.GetComments())
          {
            var commentAuthor = platform.UserService.GetProfile(comment.AuthorId);
            Console.WriteLine($"       - {commentAuthor?.Username}: {comment.Text}");
          }
        }
        index++;
      }

      PrintHeader("STORY");

      var story = new Story(bat.Id, "Өнөөдрийн мөч...");
      story.AddReaction(sarnai.Id, ReactionType.Love);

      Console.WriteLine($"  {story}");
      Console.WriteLine($"  Дуусах хугацаа: {story.ExpiresAt}");

      PrintHeader("ЗУРВАС");

      var msg = new Message(bat.Id, sarnai.Id, "Сайн уу, юу хийж байна?");
      Console.WriteLine($"  {bat.Username} -> {sarnai.Username}: {msg}");
      msg.IsRead = true;
      Console.WriteLine($"  Уншсан: {msg}");

      PrintHeader("ДЕМО ДУУСЛАА");
    }

    /// <summary>
    /// Хэсгийн гарчгийг форматтайгаар хэвлэнэ.
    /// </summary>
    static void PrintHeader(string title)
    {
      Console.WriteLine();
      Console.WriteLine(new string('=', 50));
      Console.WriteLine($"  {title}");
      Console.WriteLine(new string('=', 50));
    }
  }
}
