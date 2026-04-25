using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManagementClient.UI
{
    public sealed class FeedbackMessageBar : UserControl
    {
        private readonly Panel _accentPanel;
        private readonly Panel _contentPanel;
        private readonly TableLayoutPanel _layout;
        private readonly Label _messageLabel;

        public FeedbackMessageBar()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Dock = DockStyle.Top;
            Margin = new Padding(0, 0, 0, 16);
            Padding = new Padding(0);
            BackColor = Color.Transparent;
            Visible = false;
            TabStop = false;

            _layout = new TableLayoutPanel();
            _layout.AutoSize = true;
            _layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _layout.BackColor = Color.Transparent;
            _layout.ColumnCount = 2;
            _layout.Dock = DockStyle.Fill;
            _layout.Margin = new Padding(0);
            _layout.Padding = new Padding(0);
            _layout.RowCount = 1;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _accentPanel = new Panel();
            _accentPanel.Dock = DockStyle.Fill;
            _accentPanel.Margin = new Padding(0);
            _accentPanel.MinimumSize = new Size(8, 0);

            _contentPanel = new Panel();
            _contentPanel.AutoSize = true;
            _contentPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.Margin = new Padding(0);
            _contentPanel.Padding = new Padding(14, 12, 14, 12);

            _messageLabel = new Label();
            _messageLabel.AutoSize = true;
            _messageLabel.BackColor = Color.Transparent;
            _messageLabel.Dock = DockStyle.Top;
            _messageLabel.Margin = new Padding(0);
            _messageLabel.MaximumSize = new Size(1200, 0);

            _contentPanel.Controls.Add(_messageLabel);
            _layout.Controls.Add(_accentPanel, 0, 0);
            _layout.Controls.Add(_contentPanel, 1, 0);
            Controls.Add(_layout);

            SizeChanged += FeedbackMessageBar_SizeChanged;
        }

        public void ShowMessage(FeedbackTone tone, string message)
        {
            var text = string.IsNullOrWhiteSpace(message) ? string.Empty : message.Trim();
            if (string.IsNullOrEmpty(text))
            {
                HideMessage();
                return;
            }

            var backgroundColor = LiquidGlassTheme.GetFeedbackBackground(tone);
            var accentColor = LiquidGlassTheme.GetFeedbackAccent(tone);
            var foregroundColor = LiquidGlassTheme.GetFeedbackForeground(tone);

            _accentPanel.BackColor = accentColor;
            _messageLabel.ForeColor = foregroundColor;
            _messageLabel.Font = LiquidGlassTheme.LabelFont;
            _messageLabel.Text = string.Format("{0} {1}", LiquidGlassTheme.GetFeedbackPrefix(tone), text);

            _layout.BackColor = backgroundColor;
            _contentPanel.BackColor = backgroundColor;
            BackColor = backgroundColor;
            Visible = true;
            BringToFront();
            UpdateMessageWidth();
        }

        public void HideMessage()
        {
            _messageLabel.Text = string.Empty;
            Visible = false;
        }

        private void FeedbackMessageBar_SizeChanged(object sender, EventArgs e)
        {
            UpdateMessageWidth();
        }

        private void UpdateMessageWidth()
        {
            _messageLabel.MaximumSize = new Size(Math.Max(180, Width - 52), 0);
        }
    }
}
