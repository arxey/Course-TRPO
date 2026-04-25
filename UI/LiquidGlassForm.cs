using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GymManagementClient.UI
{
    public class LiquidGlassForm : Form
    {
        public LiquidGlassForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            BackColor = LiquidGlassTheme.BackgroundTop;
            ForeColor = LiquidGlassTheme.TextPrimary;
            Font = LiquidGlassTheme.BodyFont;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterScreen;
        }

        protected void ApplyLiquidGlassTheme()
        {
            LiquidGlassTheme.Apply(this);
        }

        protected bool TryRunUiAction(Action action, string errorMessage, FeedbackMessageBar feedbackBar)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                ShowFeedback(feedbackBar, FeedbackTone.Error, errorMessage + " " + ex.Message);
                return false;
            }
        }

        protected TableLayoutPanel CreatePageLayout(out FeedbackMessageBar feedbackBar)
        {
            var existingRoot = Controls.OfType<TableLayoutPanel>().FirstOrDefault(control => control.Name == "responsivePageLayout");
            if (existingRoot != null)
            {
                Controls.Remove(existingRoot);
                existingRoot.Dispose();
            }

            var root = new TableLayoutPanel();
            root.Name = "responsivePageLayout";
            root.BackColor = Color.Transparent;
            root.ColumnCount = 1;
            root.Dock = DockStyle.Fill;
            root.Margin = new Padding(0);
            root.Padding = new Padding(30, 108, 30, 28);
            root.RowCount = 2;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            feedbackBar = new FeedbackMessageBar();
            feedbackBar.Name = "feedbackMessageBar";

            var content = new TableLayoutPanel();
            content.Name = "responsiveContentRoot";
            content.BackColor = Color.Transparent;
            content.ColumnCount = 1;
            content.Dock = DockStyle.Fill;
            content.Margin = new Padding(0);
            content.RowCount = 1;
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(feedbackBar, 0, 0);
            root.Controls.Add(content, 0, 1);

            Controls.Add(root);
            root.BringToFront();
            return content;
        }

        protected void ShowFeedback(FeedbackMessageBar feedbackBar, FeedbackTone tone, string message)
        {
            if (feedbackBar == null)
            {
                return;
            }

            feedbackBar.ShowMessage(tone, message);
        }

        protected static TableLayoutPanel CreateTransparentTableLayout(int columns, int rows)
        {
            var layout = new TableLayoutPanel();
            layout.AutoSize = true;
            layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = columns;
            layout.Dock = DockStyle.Fill;
            layout.Margin = new Padding(0);
            layout.Padding = new Padding(0);
            layout.RowCount = rows;
            return layout;
        }

        protected static FlowLayoutPanel CreateActionPanel()
        {
            var actionPanel = new FlowLayoutPanel();
            actionPanel.AutoSize = true;
            actionPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            actionPanel.BackColor = Color.Transparent;
            actionPanel.Dock = DockStyle.Top;
            actionPanel.FlowDirection = FlowDirection.LeftToRight;
            actionPanel.Margin = new Padding(0);
            actionPanel.Padding = new Padding(0);
            actionPanel.WrapContents = true;
            return actionPanel;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // The form background stays lightweight: a soft gradient plus three glows
            // that keep the glass surface from looking flat on large screens.
            using (var backgroundBrush = new LinearGradientBrush(ClientRectangle, LiquidGlassTheme.BackgroundTop, LiquidGlassTheme.BackgroundBottom, 35f))
            {
                graphics.FillRectangle(backgroundBrush, ClientRectangle);
            }

            DrawGlow(graphics, new Rectangle(-80, -20, 280, 280), Color.FromArgb(96, 124, 196, 255));
            DrawGlow(graphics, new Rectangle(ClientSize.Width - 240, -40, 260, 260), Color.FromArgb(78, 255, 173, 212));
            DrawGlow(graphics, new Rectangle(ClientSize.Width / 2 - 160, ClientSize.Height - 180, 320, 260), Color.FromArgb(62, 255, 222, 169));
        }

        private static void DrawGlow(Graphics graphics, Rectangle bounds, Color color)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(bounds);
                using (var brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = color;
                    brush.SurroundColors = new[] { Color.FromArgb(0, color) };
                    graphics.FillPath(brush, path);
                }
            }
        }
    }
}
