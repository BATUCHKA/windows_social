using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace BT.Social.WinFormsApp.Controls;

/// <summary>
/// Instagram-style circular avatar with gradient border ring.
/// Custom Properties: BorderGradientStart, BorderGradientEnd
/// Custom Event: AvatarClicked
/// </summary>
public class AvatarControl : Control
{
    private Color _borderGradientStart = Color.FromArgb(255, 214, 0);
    private Color _borderGradientEnd = Color.FromArgb(200, 55, 171);
    private Image? _avatarImage;
    private bool _isHovered;

    public class AvatarClickedEventArgs : EventArgs
    {
        public string Username { get; }
        public DateTime ClickedAt { get; }
        public AvatarClickedEventArgs(string username)
        {
            Username = username;
            ClickedAt = DateTime.Now;
        }
    }

    public delegate void AvatarClickedEventHandler(object sender, AvatarClickedEventArgs e);

    [Category("Avatar")]
    [Description("Fires when the avatar is clicked, providing username and timestamp.")]
    public event AvatarClickedEventHandler? AvatarClicked;

    [Category("Avatar")]
    [Description("Start color of the gradient border ring.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BorderGradientStart
    {
        get => _borderGradientStart;
        set { _borderGradientStart = value; Invalidate(); }
    }

    [Category("Avatar")]
    [Description("End color of the gradient border ring.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BorderGradientEnd
    {
        get => _borderGradientEnd;
        set { _borderGradientEnd = value; Invalidate(); }
    }

    [Category("Avatar")]
    [Description("The avatar image to display.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Image? AvatarImage
    {
        get => _avatarImage;
        set { _avatarImage = value; Invalidate(); }
    }

    [Category("Avatar")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Username { get; set; } = "";

    public AvatarControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        Size = new Size(80, 80);
        Cursor = Cursors.Hand;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int borderWidth = Math.Max(3, Width / 16);
        var outerRect = new Rectangle(1, 1, Width - 3, Height - 3);
        var innerRect = new Rectangle(
            borderWidth + 2, borderWidth + 2,
            Width - 2 * borderWidth - 5, Height - 2 * borderWidth - 5);

        // Draw gradient border ring
        using (var path = new GraphicsPath())
        {
            path.AddEllipse(outerRect);
            using var brush = new LinearGradientBrush(outerRect, _borderGradientStart, _borderGradientEnd, 135f);
            using var pen = new Pen(brush, borderWidth);
            g.DrawEllipse(pen, outerRect);
        }

        // Clip to circle and draw avatar image
        using (var clipPath = new GraphicsPath())
        {
            clipPath.AddEllipse(innerRect);
            g.SetClip(clipPath);

            if (_avatarImage != null)
            {
                g.DrawImage(_avatarImage, innerRect);
            }
            else
            {
                // Draw placeholder with first letter of username
                using var bgBrush = new SolidBrush(Color.FromArgb(200, 200, 220));
                g.FillEllipse(bgBrush, innerRect);

                if (!string.IsNullOrEmpty(Username))
                {
                    string letter = Username[..1].ToUpper();
                    using var font = new Font("Segoe UI", innerRect.Height * 0.4f, FontStyle.Bold);
                    using var textBrush = new SolidBrush(Color.White);
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(letter, font, textBrush, innerRect, sf);
                }
            }

            g.ResetClip();
        }

        // Hover overlay
        if (_isHovered)
        {
            using var clipPath = new GraphicsPath();
            clipPath.AddEllipse(innerRect);
            g.SetClip(clipPath);
            using var hoverBrush = new SolidBrush(Color.FromArgb(40, 255, 255, 255));
            g.FillEllipse(hoverBrush, innerRect);
            g.ResetClip();
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _isHovered = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _isHovered = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        AvatarClicked?.Invoke(this, new AvatarClickedEventArgs(Username));
    }
}
