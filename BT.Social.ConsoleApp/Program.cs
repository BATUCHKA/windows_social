using BT.Social.Core.Enums;
using BT.Social.Core.Models;

namespace BT.Social.ConsoleApp
{
  class Program
  {
    static BtSocialPlatform platform = new();
    static User? currentUser = null;

    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      // Seed some demo data
      SeedData();

      while (true)
      {
        if (currentUser == null)
          ShowLoginScreen();
        else
          ShowMainMenu();
      }
    }

    static void SeedData()
    {
      var bat = platform.CreateUser("Bat", "bat@email.com", 22);
      bat.Bio = "Программист, кофе дуртай";
      var sarnai = platform.CreateUser("Sarnai", "sarnai@email.com", 19);
      sarnai.Bio = "Номын хорхой, аялагч";
      var bold = platform.CreateUser("Bold", "bold@email.com", 25);
      bold.Bio = "Гитар тоглогч, кодер";
      var oyuka = platform.CreateUser("Oyuka", "oyuka@email.com", 21);
      oyuka.Bio = "Зурагчин, дизайнер";

      platform.UserService.AddFriend(bat.Id, sarnai.Id);
      platform.UserService.AddFriend(bat.Id, bold.Id);
      platform.UserService.AddFriend(sarnai.Id, oyuka.Id);

      var p1 = platform.CreatePost(bat.Id, "Өнөөдөр маш сайхан цаг агаар байна! Гадаа алхах уу?");
      var p2 = platform.CreatePost(sarnai.Id, "Шинэ ном уншиж байна. 'Atomic Habits' - маш сонирхолтой!");
      var p3 = platform.CreatePost(bold.Id, "Кодинг бол урлаг. Шинэ проект эхлүүллээ!");
      var p4 = platform.CreatePost(oyuka.Id, "Өнөөдөр гоё зураг авлаа. Байгаль маш гоё байна.");

      platform.InteractionService.ReactToPost(p1.Id, sarnai.Id, ReactionType.Love);
      platform.InteractionService.ReactToPost(p1.Id, bold.Id, ReactionType.Like);
      platform.InteractionService.ReactToPost(p2.Id, bat.Id, ReactionType.Wow);
      platform.InteractionService.ReactToPost(p3.Id, bat.Id, ReactionType.Like);
      platform.InteractionService.ReactToPost(p3.Id, sarnai.Id, ReactionType.Love);
      platform.InteractionService.ReactToPost(p4.Id, sarnai.Id, ReactionType.Love);

      platform.InteractionService.CommentOnPost(p1.Id, sarnai.Id, "Тийм ээ, маш гоё өдөр!");
      platform.InteractionService.CommentOnPost(p1.Id, bold.Id, "Гадаа алхахад таатай байх аа.");
      platform.InteractionService.CommentOnPost(p2.Id, bat.Id, "Би бас уншмаар байна!");
      platform.InteractionService.CommentOnPost(p3.Id, oyuka.Id, "Ямар проект вэ?");

      platform.MessageService.SendMessage(sarnai.Id, bat.Id, "Сайн уу Бат! Юу хийж байна?");
      platform.MessageService.SendMessage(bold.Id, bat.Id, "Маргааш уулзах уу?");

      platform.StoryService.CreateStory(sarnai.Id, "Өнөөдрийн мөч - кофе шоп дээр");
      platform.StoryService.CreateStory(bold.Id, "Шинэ гитар авлаа!");
    }

    // ==================== LOGIN SCREEN ====================

    static void ShowLoginScreen()
    {
      Console.Clear();
      PrintLogo();
      Console.WriteLine("  1. Нэвтрэх");
      Console.WriteLine("  2. Бүртгүүлэх");
      Console.WriteLine("  0. Гарах");
      PrintLine();

      switch (ReadChoice())
      {
        case "1": Login(); break;
        case "2": Register(); break;
        case "0": Environment.Exit(0); break;
      }
    }

    static void Login()
    {
      Console.Write("\n  Хэрэглэгчийн нэр: ");
      var username = Console.ReadLine()?.Trim() ?? "";

      var user = platform.GetUserByUsername(username);
      if (user == null)
      {
        PrintError("Хэрэглэгч олдсонгүй!");
        WaitForKey();
        return;
      }

      currentUser = user;
      PrintSuccess($"Тавтай морил, {user.Username}!");
      WaitForKey();
    }

    static void Register()
    {
      Console.Write("\n  Хэрэглэгчийн нэр: ");
      var username = Console.ReadLine()?.Trim() ?? "";
      Console.Write("  И-мэйл: ");
      var email = Console.ReadLine()?.Trim() ?? "";
      Console.Write("  Нас: ");
      if (!byte.TryParse(Console.ReadLine()?.Trim(), out byte age))
      {
        PrintError("Нас буруу байна!");
        WaitForKey();
        return;
      }

      try
      {
        currentUser = platform.CreateUser(username, email, age);
        PrintSuccess($"Амжилттай бүртгэгдлээ! Тавтай морил, {currentUser.Username}!");
      }
      catch (Exception ex)
      {
        PrintError(ex.Message);
      }
      WaitForKey();
    }

    // ==================== MAIN MENU ====================

    static void ShowMainMenu()
    {
      Console.Clear();
      int unreadMsg = platform.MessageService.GetUnreadCount(currentUser!.Id);
      int unreadNotif = platform.NotificationService.GetUnreadCount(currentUser.Id);

      PrintHeader($"BT SOCIAL - {currentUser.Username}");
      Console.WriteLine($"  1. News Feed");
      Console.WriteLine($"  2. Нийтлэл бичих");
      Console.WriteLine($"  3. Миний профайл");
      Console.WriteLine($"  4. Найзууд");
      Console.WriteLine($"  5. Зурвас {(unreadMsg > 0 ? $"({unreadMsg} шинэ)" : "")}");
      Console.WriteLine($"  6. Stories");
      Console.WriteLine($"  7. Хэрэглэгч хайх");
      Console.WriteLine($"  8. Мэдэгдэл {(unreadNotif > 0 ? $"({unreadNotif} шинэ)" : "")}");
      Console.WriteLine($"  9. Тохиргоо");
      Console.WriteLine($"  0. Гарах");
      PrintLine();

      switch (ReadChoice())
      {
        case "1": ShowNewsFeed(); break;
        case "2": CreatePost(); break;
        case "3": ShowMyProfile(); break;
        case "4": ShowFriends(); break;
        case "5": ShowMessages(); break;
        case "6": ShowStories(); break;
        case "7": SearchUsers(); break;
        case "8": ShowNotifications(); break;
        case "9": ShowSettings(); break;
        case "0": Logout(); break;
      }
    }

    // ==================== NEWS FEED ====================

    static void ShowNewsFeed()
    {
      Console.Clear();
      PrintHeader("NEWS FEED");

      var feed = platform.PostService.GetFeed();
      if (feed.Count == 0)
      {
        Console.WriteLine("  Нийтлэл алга байна.");
        WaitForKey();
        return;
      }

      for (int i = 0; i < feed.Count; i++)
      {
        var post = feed[i];
        var author = platform.UserService.GetProfile(post.AuthorId);
        PrintPost(i + 1, post, author);
      }

      PrintLine();
      Console.WriteLine("  Нийтлэлийн дугаар оруулж reaction/comment хийх (0 = буцах):");
      var choice = ReadChoice();

      if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= feed.Count)
      {
        ShowPostActions(feed[idx - 1]);
      }
    }

    static void ShowPostActions(Post post)
    {
      var author = platform.UserService.GetProfile(post.AuthorId);

      Console.Clear();
      PrintHeader("НИЙТЛЭЛ");
      PrintPost(1, post, author);

      // Show comments
      var comments = post.GetComments();
      if (comments.Count > 0)
      {
        Console.WriteLine("  --- Сэтгэгдлүүд ---");
        foreach (var c in comments)
        {
          var cAuthor = platform.UserService.GetProfile(c.AuthorId);
          Console.WriteLine($"    {cAuthor?.Username}: {c.Text}  [{c.GetReactionCount()} reaction]");
        }
      }

      PrintLine();
      Console.WriteLine("  1. Reaction өгөх");
      Console.WriteLine("  2. Сэтгэгдэл бичих");
      Console.WriteLine("  3. Хуваалцах");
      Console.WriteLine("  0. Буцах");
      PrintLine();

      switch (ReadChoice())
      {
        case "1":
          ReactToPost(post);
          break;
        case "2":
          CommentOnPost(post);
          break;
        case "3":
          SharePost(post);
          break;
      }
    }

    static void ReactToPost(Post post)
    {
      Console.WriteLine("\n  Reaction сонгох:");
      Console.WriteLine("  1. Like    2. Love    3. Haha");
      Console.WriteLine("  4. Wow     5. Sad     6. Angry");

      var choice = ReadChoice();
      ReactionType? type = choice switch
      {
        "1" => ReactionType.Like,
        "2" => ReactionType.Love,
        "3" => ReactionType.Haha,
        "4" => ReactionType.Wow,
        "5" => ReactionType.Sad,
        "6" => ReactionType.Angry,
        _ => null
      };

      if (type != null)
      {
        platform.InteractionService.ReactToPost(post.Id, currentUser!.Id, type.Value);
        var author = platform.UserService.GetProfile(post.AuthorId);
        platform.NotificationService.Notify(post.AuthorId, currentUser.Id,
            NotificationType.Reaction,
            $"{currentUser.Username} таны нийтлэлд {type} reaction өглөө");
        PrintSuccess("Reaction амжилттай!");
      }
      WaitForKey();
    }

    static void CommentOnPost(Post post)
    {
      Console.Write("\n  Сэтгэгдэл: ");
      var text = Console.ReadLine()?.Trim() ?? "";
      if (string.IsNullOrEmpty(text)) return;

      platform.InteractionService.CommentOnPost(post.Id, currentUser!.Id, text);
      platform.NotificationService.Notify(post.AuthorId, currentUser.Id,
          NotificationType.Comment,
          $"{currentUser.Username} таны нийтлэлд сэтгэгдэл бичлээ: \"{Truncate(text, 30)}\"");
      PrintSuccess("Сэтгэгдэл нэмэгдлээ!");
      WaitForKey();
    }

    static void SharePost(Post post)
    {
      platform.InteractionService.SharePost(post.Id, currentUser!.Id);
      platform.NotificationService.Notify(post.AuthorId, currentUser.Id,
          NotificationType.Share,
          $"{currentUser.Username} таны нийтлэлийг хуваалцлаа");
      PrintSuccess("Хуваалцлаа!");
      WaitForKey();
    }

    // ==================== CREATE POST ====================

    static void CreatePost()
    {
      Console.Clear();
      PrintHeader("ШИНЭ НИЙТЛЭЛ");
      Console.Write("  Юу бодож байна, {0}?\n  > ", currentUser!.Username);
      var text = Console.ReadLine()?.Trim() ?? "";
      if (string.IsNullOrEmpty(text)) return;

      Console.WriteLine("\n  Нууцлал:");
      Console.WriteLine("  1. Нийтийн (Public)");
      Console.WriteLine("  2. Зөвхөн найзууд (Friends Only)");
      Console.WriteLine("  3. Хувийн (Private)");

      var privacy = ReadChoice() switch
      {
        "2" => PrivacyLevel.FriendsOnly,
        "3" => PrivacyLevel.Private,
        _ => PrivacyLevel.Public
      };

      platform.PostService.CreatePost(currentUser.Id, text, privacy);
      PrintSuccess("Нийтлэл амжилттай нийтлэгдлээ!");
      WaitForKey();
    }

    // ==================== PROFILE ====================

    static void ShowMyProfile()
    {
      Console.Clear();
      ShowProfile(currentUser!);
    }

    static void ShowProfile(User user)
    {
      PrintHeader($"{user.Username}-н ПРОФАЙЛ");
      Console.WriteLine($"  Нэр:      {user.Username}");
      Console.WriteLine($"  И-мэйл:   {user.Email}");
      Console.WriteLine($"  Нас:       {user.Age}");
      Console.WriteLine($"  Bio:       {(string.IsNullOrEmpty(user.Bio) ? "(хоосон)" : user.Bio)}");
      Console.WriteLine($"  Найзууд:   {user.FriendIds.Count}");
      Console.WriteLine($"  Статус:    {user.Status}");
      Console.WriteLine($"  Бүртгүүлсэн: {user.CreatedAt:yyyy-MM-dd}");

      // Show user's posts
      var posts = platform.PostService.GetUserPosts(user.Id);
      Console.WriteLine($"\n  --- Нийтлэлүүд ({posts.Count}) ---");
      foreach (var post in posts)
      {
        Console.WriteLine($"  \"{post.Text}\"");
        Console.WriteLine($"    {post.GetReactionCount()} reactions | {post.GetComments().Count} comments | {post.GetShareCount()} shares");
      }

      PrintLine();

      if (user.Id == currentUser!.Id)
      {
        Console.WriteLine("  1. Bio засах");
        Console.WriteLine("  0. Буцах");
        PrintLine();

        if (ReadChoice() == "1")
        {
          Console.Write("  Шинэ bio: ");
          user.Bio = Console.ReadLine()?.Trim() ?? "";
          PrintSuccess("Bio шинэчлэгдлээ!");
          WaitForKey();
        }
      }
      else
      {
        bool isFriend = currentUser.FriendIds.Contains(user.Id);
        if (isFriend)
          Console.WriteLine("  1. Найзаас хасах");
        else
          Console.WriteLine("  1. Найз нэмэх");
        Console.WriteLine("  2. Зурвас илгээх");
        Console.WriteLine("  0. Буцах");
        PrintLine();

        switch (ReadChoice())
        {
          case "1":
            if (isFriend)
            {
              platform.UserService.RemoveFriend(currentUser.Id, user.Id);
              PrintSuccess($"{user.Username}-г найзаас хаслаа.");
            }
            else
            {
              platform.UserService.AddFriend(currentUser.Id, user.Id);
              platform.NotificationService.Notify(user.Id, currentUser.Id,
                  NotificationType.FriendRequest,
                  $"{currentUser.Username} таныг найзаар нэмлээ");
              PrintSuccess($"{user.Username}-г найзаар нэмлээ!");
            }
            WaitForKey();
            break;
          case "2":
            Console.Write("  Зурвас: ");
            var msg = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(msg))
            {
              platform.MessageService.SendMessage(currentUser.Id, user.Id, msg);
              platform.NotificationService.Notify(user.Id, currentUser.Id,
                  NotificationType.Message,
                  $"{currentUser.Username} танд зурвас илгээлээ");
              PrintSuccess("Зурвас илгээгдлээ!");
            }
            WaitForKey();
            break;
        }
      }
    }

    // ==================== FRIENDS ====================

    static void ShowFriends()
    {
      Console.Clear();
      PrintHeader("НАЙЗУУД");

      if (currentUser!.FriendIds.Count == 0)
      {
        Console.WriteLine("  Найз алга байна.");
        WaitForKey();
        return;
      }

      for (int i = 0; i < currentUser.FriendIds.Count; i++)
      {
        var friend = platform.UserService.GetProfile(currentUser.FriendIds[i]);
        if (friend != null)
          Console.WriteLine($"  {i + 1}. {friend.Username} - {(string.IsNullOrEmpty(friend.Bio) ? "" : friend.Bio)}");
      }

      PrintLine();
      Console.WriteLine("  Дугаар оруулж профайл харах (0 = буцах):");
      var choice = ReadChoice();

      if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= currentUser.FriendIds.Count)
      {
        var friend = platform.UserService.GetProfile(currentUser.FriendIds[idx - 1]);
        if (friend != null) ShowProfile(friend);
      }
    }

    // ==================== MESSAGES ====================

    static void ShowMessages()
    {
      Console.Clear();
      PrintHeader("ЗУРВАС");

      var partners = platform.MessageService.GetConversationPartners(currentUser!.Id);

      if (partners.Count == 0)
      {
        Console.WriteLine("  Зурвас алга байна.");
        Console.WriteLine("\n  1. Шинэ зурвас илгээх");
        Console.WriteLine("  0. Буцах");
        PrintLine();

        if (ReadChoice() == "1") SendNewMessage();
        return;
      }

      for (int i = 0; i < partners.Count; i++)
      {
        var partner = platform.UserService.GetProfile(partners[i]);
        var conv = platform.MessageService.GetConversation(currentUser.Id, partners[i]);
        var lastMsg = conv.LastOrDefault();
        var unread = conv.Count(m => m.ReceiverId == currentUser.Id && !m.IsRead);
        Console.WriteLine($"  {i + 1}. {partner?.Username} - \"{Truncate(lastMsg?.Content ?? "", 30)}\" {(unread > 0 ? $"[{unread} шинэ]" : "")}");
      }

      PrintLine();
      Console.WriteLine("  Дугаар оруулж харилцаа харах, N = шинэ зурвас, 0 = буцах:");
      var choice = ReadChoice();

      if (choice.Equals("N", StringComparison.OrdinalIgnoreCase))
      {
        SendNewMessage();
      }
      else if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= partners.Count)
      {
        ShowConversation(partners[idx - 1]);
      }
    }

    static void ShowConversation(Guid partnerId)
    {
      var partner = platform.UserService.GetProfile(partnerId);
      platform.MessageService.MarkConversationRead(currentUser!.Id, partnerId);

      Console.Clear();
      PrintHeader($"Харилцаа: {partner?.Username}");

      var messages = platform.MessageService.GetConversation(currentUser.Id, partnerId);
      foreach (var msg in messages)
      {
        var sender = msg.SenderId == currentUser.Id ? "Та" : partner?.Username ?? "?";
        Console.WriteLine($"  [{msg.SentAt:HH:mm}] {sender}: {msg.Content}");
      }

      PrintLine();
      Console.Write("  Хариу бичих (хоосон = буцах): ");
      var reply = Console.ReadLine()?.Trim() ?? "";
      if (!string.IsNullOrEmpty(reply))
      {
        platform.MessageService.SendMessage(currentUser.Id, partnerId, reply);
        platform.NotificationService.Notify(partnerId, currentUser.Id,
            NotificationType.Message,
            $"{currentUser.Username} танд зурвас илгээлээ");
        PrintSuccess("Илгээгдлээ!");
        WaitForKey();
      }
    }

    static void SendNewMessage()
    {
      Console.Write("\n  Хэн рүү: ");
      var username = Console.ReadLine()?.Trim() ?? "";
      var user = platform.GetUserByUsername(username);
      if (user == null || user.Id == currentUser!.Id)
      {
        PrintError("Хэрэглэгч олдсонгүй!");
        WaitForKey();
        return;
      }

      Console.Write("  Зурвас: ");
      var text = Console.ReadLine()?.Trim() ?? "";
      if (string.IsNullOrEmpty(text)) return;

      platform.MessageService.SendMessage(currentUser.Id, user.Id, text);
      platform.NotificationService.Notify(user.Id, currentUser.Id,
          NotificationType.Message,
          $"{currentUser.Username} танд зурвас илгээлээ");
      PrintSuccess("Зурвас илгээгдлээ!");
      WaitForKey();
    }

    // ==================== STORIES ====================

    static void ShowStories()
    {
      Console.Clear();
      PrintHeader("STORIES");

      var stories = platform.StoryService.GetActiveStories();

      if (stories.Count == 0)
        Console.WriteLine("  Идэвхтэй story алга байна.");

      for (int i = 0; i < stories.Count; i++)
      {
        var author = platform.UserService.GetProfile(stories[i].AuthorId);
        var remaining = stories[i].ExpiresAt - DateTime.Now;
        Console.WriteLine($"  {i + 1}. [{author?.Username}] \"{stories[i].Text}\"  ({remaining.Hours}ц {remaining.Minutes}м үлдсэн) | {stories[i].GetReactionCount()} reactions");
      }

      PrintLine();
      Console.WriteLine("  S = Шинэ story нэмэх, 0 = Буцах");
      Console.WriteLine("  Дугаар оруулж reaction өгөх:");
      var choice = ReadChoice();

      if (choice.Equals("S", StringComparison.OrdinalIgnoreCase))
      {
        Console.Write("  Story: ");
        var text = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(text))
        {
          platform.StoryService.CreateStory(currentUser!.Id, text);
          PrintSuccess("Story нэмэгдлээ! (24 цагийн дараа устна)");
        }
        WaitForKey();
      }
      else if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= stories.Count)
      {
        Console.WriteLine("  1. Like  2. Love  3. Haha  4. Wow  5. Sad  6. Angry");
        var rc = ReadChoice();
        ReactionType? type = rc switch
        {
          "1" => ReactionType.Like,
          "2" => ReactionType.Love,
          "3" => ReactionType.Haha,
          "4" => ReactionType.Wow,
          "5" => ReactionType.Sad,
          "6" => ReactionType.Angry,
          _ => null
        };
        if (type != null)
        {
          stories[idx - 1].AddReaction(currentUser!.Id, type.Value);
          var storyAuthor = platform.UserService.GetProfile(stories[idx - 1].AuthorId);
          platform.NotificationService.Notify(stories[idx - 1].AuthorId, currentUser.Id,
              NotificationType.StoryReaction,
              $"{currentUser.Username} таны story-д {type} reaction өглөө");
          PrintSuccess("Reaction амжилттай!");
        }
        WaitForKey();
      }
    }

    // ==================== SEARCH ====================

    static void SearchUsers()
    {
      Console.Clear();
      PrintHeader("ХЭРЭГЛЭГЧ ХАЙХ");
      Console.Write("  Хайх нэр: ");
      var query = Console.ReadLine()?.Trim() ?? "";

      if (string.IsNullOrEmpty(query))
        return;

      var results = platform.UserService.GetAllUsers()
          .Where(u => u.Username.Contains(query, StringComparison.OrdinalIgnoreCase) && u.Id != currentUser!.Id)
          .ToList();

      if (results.Count == 0)
      {
        PrintError("Олдсонгүй.");
        WaitForKey();
        return;
      }

      for (int i = 0; i < results.Count; i++)
      {
        bool isFriend = currentUser!.FriendIds.Contains(results[i].Id);
        Console.WriteLine($"  {i + 1}. {results[i].Username} {(isFriend ? "[Найз]" : "")} - {results[i].Bio}");
      }

      PrintLine();
      Console.WriteLine("  Дугаар оруулж профайл харах (0 = буцах):");
      var choice = ReadChoice();

      if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= results.Count)
      {
        ShowProfile(results[idx - 1]);
      }
    }

    // ==================== NOTIFICATIONS ====================

    static void ShowNotifications()
    {
      Console.Clear();
      PrintHeader("МЭДЭГДЭЛ");

      var notifs = platform.NotificationService.GetNotifications(currentUser!.Id);

      if (notifs.Count == 0)
      {
        Console.WriteLine("  Мэдэгдэл алга байна.");
        WaitForKey();
        return;
      }

      foreach (var n in notifs)
      {
        string icon = n.Type switch
        {
          NotificationType.Reaction => "[Reaction]",
          NotificationType.Comment => "[Comment]",
          NotificationType.Share => "[Share]",
          NotificationType.FriendRequest => "[Найз]",
          NotificationType.Message => "[Зурвас]",
          NotificationType.StoryReaction => "[Story]",
          _ => "[?]"
        };
        string readMark = n.IsRead ? " " : "*";
        Console.WriteLine($"  {readMark} {icon} {n.Text}  ({n.CreatedAt:HH:mm})");
      }

      platform.NotificationService.MarkAllRead(currentUser.Id);
      PrintLine();
      Console.WriteLine("  Бүх мэдэгдэл уншигдлаа.");
      WaitForKey();
    }

    // ==================== SETTINGS ====================

    static void ShowSettings()
    {
      Console.Clear();
      PrintHeader("ТОХИРГОО");
      Console.WriteLine($"  Хэрэглэгч: {currentUser!.Username}");
      Console.WriteLine($"  И-мэйл:    {currentUser.Email}");
      Console.WriteLine();
      Console.WriteLine("  1. Нэр солих");
      Console.WriteLine("  2. И-мэйл солих");
      Console.WriteLine("  3. Bio засах");
      Console.WriteLine("  4. Бүртгэл устгах");
      Console.WriteLine("  0. Буцах");
      PrintLine();

      switch (ReadChoice())
      {
        case "1":
          Console.Write("  Шинэ нэр: ");
          var newName = Console.ReadLine()?.Trim() ?? "";
          if (!string.IsNullOrEmpty(newName))
          {
            var existing = platform.GetUserByUsername(newName);
            if (existing != null)
            {
              PrintError("Энэ нэр аль хэдийн бүртгэлтэй!");
            }
            else
            {
              currentUser.Username = newName;
              PrintSuccess("Нэр солигдлоо!");
            }
          }
          WaitForKey();
          break;
        case "2":
          Console.Write("  Шинэ и-мэйл: ");
          var newEmail = Console.ReadLine()?.Trim() ?? "";
          if (!string.IsNullOrEmpty(newEmail))
          {
            currentUser.Email = newEmail;
            PrintSuccess("И-мэйл солигдлоо!");
          }
          WaitForKey();
          break;
        case "3":
          Console.Write("  Шинэ bio: ");
          currentUser.Bio = Console.ReadLine()?.Trim() ?? "";
          PrintSuccess("Bio шинэчлэгдлээ!");
          WaitForKey();
          break;
        case "4":
          Console.Write("  Итгэлтэй юу? (Y/N): ");
          if (Console.ReadLine()?.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
          {
            currentUser.Status = AccountStatus.Deactivated;
            currentUser = null;
            PrintSuccess("Бүртгэл устгагдлаа.");
            WaitForKey();
          }
          break;
      }
    }

    static void Logout()
    {
      PrintSuccess($"Баяртай, {currentUser!.Username}!");
      currentUser = null;
      WaitForKey();
    }

    // ==================== UI HELPERS ====================

    static void PrintLogo()
    {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(@"
   ____  _____   ____             _       _
  | __ )|_   _| / ___|  ___   ___(_) __ _| |
  |  _ \  | |   \___ \ / _ \ / __| |/ _` | |
  | |_) | | |    ___) | (_) | (__| | (_| | |
  |____/  |_|   |____/ \___/ \___|_|\__,_|_|

  Монголын нийгмийн сүлжээ
");
      Console.ResetColor();
      PrintLine();
    }

    static void PrintHeader(string title)
    {
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(new string('=', 55));
      Console.WriteLine($"  {title}");
      Console.WriteLine(new string('=', 55));
      Console.ResetColor();
    }

    static void PrintLine()
    {
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.WriteLine(new string('-', 55));
      Console.ResetColor();
    }

    static void PrintPost(int num, Post post, User? author)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write($"  {num}. ");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write($"{author?.Username ?? "?"} ");
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.WriteLine($"({post.CreatedAt:MM/dd HH:mm})");
      Console.ResetColor();
      Console.WriteLine($"     {post.Text}");
      Console.ForegroundColor = ConsoleColor.DarkCyan;
      Console.WriteLine($"     {post.GetReactionCount()} reactions | {post.GetComments().Count} comments | {post.GetShareCount()} shares");
      Console.ResetColor();
      Console.WriteLine();
    }

    static void PrintSuccess(string msg)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"\n  [OK] {msg}");
      Console.ResetColor();
    }

    static void PrintError(string msg)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"\n  [!] {msg}");
      Console.ResetColor();
    }

    static string ReadChoice()
    {
      Console.Write("  > ");
      return Console.ReadLine()?.Trim() ?? "";
    }

    static void WaitForKey()
    {
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.Write("\n  Үргэлжлүүлэхийн тулд дурын товч дарна уу...");
      Console.ResetColor();
      Console.ReadKey(true);
    }

    static string Truncate(string text, int maxLen)
    {
      return text.Length <= maxLen ? text : text[..maxLen] + "...";
    }
  }
}
