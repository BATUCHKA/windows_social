using BT.Social.Core.Models;
using BT.Social.WinFormsApp.Controls;

namespace BT.Social.WinFormsApp;

public partial class Form1 : Form
{
    private BtSocialPlatform _platform = null!;
    private Panel _feedPanel = null!;

    public Form1()
    {
        InitializeComponent();
        Text = "BT Social";
        Size = new Size(500, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;

        InitPlatform();
        BuildUI();
    }

    private void InitPlatform()
    {
        _platform = new BtSocialPlatform();
        _platform.CreateUser("Bat", "bat@mail.com", 22);
        _platform.CreateUser("Sarnai", "sarnai@mail.com", 20);
        _platform.CreateUser("Bold", "bold@mail.com", 25);

        var bat = _platform.GetUserByUsername("Bat");
        var sarnai = _platform.GetUserByUsername("Sarnai");
        var bold = _platform.GetUserByUsername("Bold");

        if (bat != null) _platform.PostService.CreatePost(bat.Id, "Өнөөдөр маш гоё өдөр байна! ☀️");
        if (sarnai != null) _platform.PostService.CreatePost(sarnai.Id, "Шинэ номоо уншиж дуусгалаа 📚");
        if (bold != null) _platform.PostService.CreatePost(bold.Id, "Баярын мэнд хүргэе! 🎉");
    }

    private void BuildUI()
    {
        // Header
        var header = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(59, 89, 152) };
        var title = new Label
        {
            Text = "BT Social",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 10)
        };
        header.Controls.Add(title);
        Controls.Add(header);

        // Feed panel
        _feedPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };
        Controls.Add(_feedPanel);

        header.BringToFront();
        LoadFeed();
    }

    private void LoadFeed()
    {
        _feedPanel.Controls.Clear();
        var posts = _platform.PostService.GetFeed();
        int y = 10;

        foreach (var post in posts)
        {
            var card = CreatePostCard(post, ref y);
            _feedPanel.Controls.Add(card);
        }
    }

    private Panel CreatePostCard(Post post, ref int y)
    {
        var author = _platform.UserService.GetProfile(post.AuthorId);
        string username = author?.Username ?? "Unknown";

        var card = new Panel
        {
            Location = new Point(10, y),
            Size = new Size(440, 150),
            BackColor = Color.FromArgb(250, 250, 252),
            BorderStyle = BorderStyle.FixedSingle
        };

        // Avatar control (Custom Control #1)
        var avatar = new AvatarControl
        {
            Location = new Point(8, 8),
            Size = new Size(48, 48),
            Username = username,
            BorderGradientStart = Color.FromArgb(255, 214, 0),
            BorderGradientEnd = Color.FromArgb(200, 55, 171)
        };
        avatar.AvatarClicked += (s, e) =>
        {
            MessageBox.Show($"Профайл: {e.Username}\nЦаг: {e.ClickedAt:HH:mm:ss}",
                "Avatar Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        card.Controls.Add(avatar);

        // Username
        card.Controls.Add(new Label
        {
            Text = username,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Location = new Point(64, 12),
            AutoSize = true
        });

        // Time
        card.Controls.Add(new Label
        {
            Text = post.CreatedAt.ToString("HH:mm"),
            Font = new Font("Segoe UI", 8),
            ForeColor = Color.Gray,
            Location = new Point(64, 32),
            AutoSize = true
        });

        // Post text
        card.Controls.Add(new Label
        {
            Text = post.Text,
            Font = new Font("Segoe UI", 10),
            Location = new Point(10, 65),
            Size = new Size(420, 35)
        });

        // Reaction bar (Custom Control #2)
        var reactionBar = new ReactionBar
        {
            Location = new Point(5, 100),
            Size = new Size(430, 44),
            EmojiSize = 28,
            BarBackgroundColor = Color.FromArgb(245, 245, 245)
        };
        reactionBar.ReactionSelected += (s, e) =>
        {
            MessageBox.Show($"'{e.ReactionName}' reaction - {username}",
                "Reaction", MessageBoxButtons.OK);
        };
        card.Controls.Add(reactionBar);

        y += 160;
        return card;
    }
}
