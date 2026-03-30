using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace BT.Social.WinFormsApp.Controls;

public class ReactionBar : Control
{
    private int _emojiSize = 32;
    private Color _barBackgroundColor = Color.FromArgb(240, 240, 240);
    private int _hoveredIndex = -1;

    private static readonly string[] EmojiLabels = { "❤️", "😂", "😮", "😢", "😡", "👍", "+" };
    private static readonly Color[] EmojiColors =
    {
        Color.Red,                          // heart
        Color.FromArgb(255, 200, 0),        // laugh
        Color.FromArgb(255, 200, 0),        // wow
        Color.FromArgb(255, 200, 0),        // sad
        Color.FromArgb(230, 80, 0),         // angry
        Color.FromArgb(50, 130, 240),       // thumbs up
        Color.FromArgb(100, 100, 100)       // plus
    };

    public class ReactionSelectedEventArgs : EventArgs
    {
        public int ReactionIndex { get; }
        public string ReactionName { get; }
        public ReactionSelectedEventArgs(int index, string name)
        {
            ReactionIndex = index;
            ReactionName = name;
        }
    }

    public delegate void ReactionSelectedEventHandler(object sender, ReactionSelectedEventArgs e);

    [Category("Reaction")]
    [Description("Fires when user selects a reaction emoji.")]
    public event ReactionSelectedEventHandler? ReactionSelected;

    [Category("Reaction")]
    [Description("Size of each emoji circle in pixels.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int EmojiSize
    {
        get => _emojiSize;
        set { _emojiSize = Math.Max(16, value); Invalidate(); }
    }

    [Category("Reaction")]
    [Description("Background color of the reaction bar.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BarBackgroundColor
    {
        get => _barBackgroundColor;
        set { _barBackgroundColor = value; Invalidate(); }
    }

    public ReactionBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        Size = new Size(320, 50);
        Cursor = Cursors.Hand;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        // Draw rounded background pill
        int pillHeight = _emojiSize + 12;
        int pillWidth = EmojiLabels.Length * (_emojiSize + 8) + 8;
        int pillX = (Width - pillWidth) / 2;
        int pillY = (Height - pillHeight) / 2;
        var pillRect = new Rectangle(pillX, pillY, pillWidth, pillHeight);

        using (var path = CreateRoundedRect(pillRect, pillHeight / 2))
        {
            using var bgBrush = new SolidBrush(_barBackgroundColor);
            g.FillPath(bgBrush, path);
            using var borderPen = new Pen(Color.FromArgb(210, 210, 210), 1);
            g.DrawPath(borderPen, path);
        }

        // Draw each emoji circle
        for (int i = 0; i < EmojiLabels.Length; i++)
        {
            int x = pillX + 8 + i * (_emojiSize + 8);
            int y = pillY + 6;
            bool hovered = i == _hoveredIndex;
            int size = hovered ? _emojiSize + 6 : _emojiSize;
            int offset = hovered ? -3 : 0;

            var emojiRect = new Rectangle(x + offset, y + offset, size, size);

            // Draw emoji circle background
            using (var brush = new SolidBrush(hovered ? Color.FromArgb(40, EmojiColors[i]) : Color.Transparent))
            {
                g.FillEllipse(brush, emojiRect);
            }

            // Draw the emoji symbol using Graphics.DrawString
            using var font = new Font("Segoe UI Emoji", size * 0.5f, FontStyle.Regular);
            using var textBrush = new SolidBrush(EmojiColors[i]);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            if (i < EmojiLabels.Length - 1)
            {
                // Draw colored circle with symbol
                g.FillEllipse(new SolidBrush(Color.FromArgb(hovered ? 60 : 30, EmojiColors[i])), emojiRect);
                DrawEmojiSymbol(g, i, emojiRect, hovered);
            }
            else
            {
                // Plus button
                using var plusPen = new Pen(Color.Gray, 2);
                g.DrawEllipse(plusPen, emojiRect);
                using var plusFont = new Font("Segoe UI", size * 0.4f, FontStyle.Bold);
                using var plusBrush = new SolidBrush(Color.Gray);
                g.DrawString("+", plusFont, plusBrush, emojiRect, sf);
            }
        }
    }

    private void DrawEmojiSymbol(Graphics g, int index, Rectangle rect, bool hovered)
    {
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        float fontSize = rect.Height * 0.35f;

        switch (index)
        {
            case 0: // Heart
                DrawHeart(g, rect, hovered);
                break;
            case 1: // Laugh - circle face
                DrawFace(g, rect, EmojiColors[index], "XD", hovered);
                break;
            case 2: // Wow
                DrawFace(g, rect, EmojiColors[index], "O", hovered);
                break;
            case 3: // Sad
                DrawFace(g, rect, EmojiColors[index], ":(", hovered);
                break;
            case 4: // Angry
                DrawFace(g, rect, EmojiColors[index], ">:(", hovered);
                break;
            case 5: // Thumbs up
                DrawThumbsUp(g, rect, hovered);
                break;
        }
    }

    private void DrawHeart(Graphics g, Rectangle rect, bool hovered)
    {
        float scale = hovered ? 1.15f : 1f;
        int cx = rect.X + rect.Width / 2;
        int cy = rect.Y + rect.Height / 2;
        float s = rect.Width * 0.35f * scale;

        using var path = new GraphicsPath();
        // Heart shape using bezier curves
        path.AddBezier(cx, cy - s * 0.2f, cx - s, cy - s * 1.2f, cx - s * 1.3f, cy + s * 0.2f, cx, cy + s);
        path.AddBezier(cx, cy + s, cx + s * 1.3f, cy + s * 0.2f, cx + s, cy - s * 1.2f, cx, cy - s * 0.2f);
        using var brush = new SolidBrush(Color.Red);
        g.FillPath(brush, path);
    }

    private void DrawFace(Graphics g, Rectangle rect, Color color, string expression, bool hovered)
    {
        float scale = hovered ? 1.1f : 1f;
        int cx = rect.X + rect.Width / 2;
        int cy = rect.Y + rect.Height / 2;
        int r = (int)(rect.Width * 0.35f * scale);

        // Face circle
        using var faceBrush = new SolidBrush(color);
        g.FillEllipse(faceBrush, cx - r, cy - r, r * 2, r * 2);

        // Expression text
        using var font = new Font("Segoe UI", r * 0.7f, FontStyle.Bold);
        using var brush = new SolidBrush(Color.FromArgb(80, 50, 0));
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        var faceRect = new Rectangle(cx - r, cy - r, r * 2, r * 2);
        g.DrawString(expression, font, brush, faceRect, sf);
    }

    private void DrawThumbsUp(Graphics g, Rectangle rect, bool hovered)
    {
        float scale = hovered ? 1.15f : 1f;
        int cx = rect.X + rect.Width / 2;
        int cy = rect.Y + rect.Height / 2;
        int s = (int)(rect.Width * 0.3f * scale);

        // Simple thumb shape using filled rectangles and ellipse
        using var brush = new SolidBrush(EmojiColors[5]);
        g.FillEllipse(brush, cx - s, cy - s, s * 2, (int)(s * 1.5f));
        g.FillRectangle(brush, cx - s / 2, cy, s, (int)(s * 0.8f));

        // Draw 👍 text as fallback
        using var font = new Font("Segoe UI", s * 1.2f, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.White);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("👍", font, textBrush, rect, sf);
    }

    private int GetEmojiIndexAt(Point pt)
    {
        int pillWidth = EmojiLabels.Length * (_emojiSize + 8) + 8;
        int pillX = (Width - pillWidth) / 2;
        int pillY = (Height - _emojiSize - 12) / 2;

        for (int i = 0; i < EmojiLabels.Length; i++)
        {
            int x = pillX + 8 + i * (_emojiSize + 8);
            var r = new Rectangle(x, pillY, _emojiSize, _emojiSize + 12);
            if (r.Contains(pt)) return i;
        }
        return -1;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int newIndex = GetEmojiIndexAt(e.Location);
        if (newIndex != _hoveredIndex)
        {
            _hoveredIndex = newIndex;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hoveredIndex = -1;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (_hoveredIndex >= 0 && _hoveredIndex < EmojiLabels.Length)
        {
            string[] names = { "Love", "Haha", "Wow", "Sad", "Angry", "Like", "More" };
            ReactionSelected?.Invoke(this, new ReactionSelectedEventArgs(_hoveredIndex, names[_hoveredIndex]));
        }
    }

    private static GraphicsPath CreateRoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
