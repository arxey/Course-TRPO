using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymManagementClient.UI
{
    internal sealed class LiquidGlassSurface : Panel
    {
        public LiquidGlassSurface()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            Enabled = false;
            TabStop = false;
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var surfaceBounds = Rectangle.Inflate(ClientRectangle, -10, -10);
            if (surfaceBounds.Width <= 0 || surfaceBounds.Height <= 0)
            {
                return;
            }

            // Two borders and a translucent fill create the main "glass card" that
            // all forms sit on top of, while keeping the background glow visible.
            using (var cardPath = LiquidGlassTheme.CreateRoundedPath(surfaceBounds, 28))
            using (var fillBrush = new LinearGradientBrush(surfaceBounds, Color.FromArgb(212, 255, 255, 255), Color.FromArgb(172, 229, 240, 255), 90f))
            using (var outerBorder = new Pen(Color.FromArgb(150, 255, 255, 255), 1.2f))
            using (var innerBorder = new Pen(Color.FromArgb(60, 82, 124, 175), 1f))
            {
                graphics.FillPath(fillBrush, cardPath);
                graphics.DrawPath(outerBorder, cardPath);

                var innerBounds = Rectangle.Inflate(surfaceBounds, -3, -3);
                using (var innerPath = LiquidGlassTheme.CreateRoundedPath(innerBounds, 24))
                {
                    graphics.DrawPath(innerBorder, innerPath);
                }
            }

            // A shallow top highlight helps the panel read as glass instead of a matte rectangle.
            var highlightBounds = new Rectangle(surfaceBounds.X + 14, surfaceBounds.Y + 12, surfaceBounds.Width - 28, System.Math.Max(52, surfaceBounds.Height / 3));
            using (var highlightPath = LiquidGlassTheme.CreateRoundedPath(highlightBounds, 24))
            using (var highlightBrush = new LinearGradientBrush(highlightBounds, Color.FromArgb(116, 255, 255, 255), Color.FromArgb(12, 255, 255, 255), 90f))
            {
                graphics.FillPath(highlightBrush, highlightPath);
            }
        }
    }
}
