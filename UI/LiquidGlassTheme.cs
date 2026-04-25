using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GymManagementClient.UI
{
    internal static class LiquidGlassTheme
    {
        public static readonly Color BackgroundTop = Color.FromArgb(233, 243, 255);
        public static readonly Color BackgroundBottom = Color.FromArgb(252, 237, 245);
        public static readonly Color TextPrimary = Color.FromArgb(30, 47, 72);
        public static readonly Color TextMuted = Color.FromArgb(95, 111, 135);
        public static readonly Color PrimaryButton = Color.FromArgb(74, 153, 255);
        public static readonly Color AccentButton = Color.FromArgb(99, 208, 197);
        public static readonly Color SecondaryButton = Color.FromArgb(129, 151, 187);
        public static readonly Color DangerButton = Color.FromArgb(231, 119, 143);
        public static readonly Color Success = Color.FromArgb(63, 170, 128);
        public static readonly Color Warning = Color.FromArgb(226, 164, 70);
        public static readonly Color Error = Color.FromArgb(220, 102, 128);
        public static readonly Color InputBackground = Color.FromArgb(246, 250, 255);
        public static readonly Color GridBackground = Color.FromArgb(250, 252, 255);
        public static readonly Color GridAlternate = Color.FromArgb(242, 247, 255);
        public static readonly Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font LabelFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font HeroFont = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font SubtitleFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        public static void Apply(Form form)
        {
            if (form == null)
            {
                return;
            }

            form.BackColor = BackgroundTop;
            form.ForeColor = TextPrimary;
            form.Font = BodyFont;

            EnsureGlassSurface(form);
            StyleControlCollection(form.Controls);
        }

        public static void SetHero(Form form, string title, string subtitle)
        {
            var titleLabel = EnsureHeroLabel(form, "liquidGlassHeroTitle", HeroFont, TextPrimary, new Point(30, 26));
            titleLabel.Text = title;

            var subtitleLabel = EnsureHeroLabel(form, "liquidGlassHeroSubtitle", SubtitleFont, TextMuted, new Point(32, 60));
            subtitleLabel.Text = subtitle;
            ResizeHeroSubtitle(form);

            form.Resize -= HandleHeroResize;
            form.Resize += HandleHeroResize;
        }

        public static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            var diameter = Math.Max(2, radius * 2);
            var arcBounds = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arcBounds, 180, 90);
            arcBounds.X = bounds.Right - diameter;
            path.AddArc(arcBounds, 270, 90);
            arcBounds.Y = bounds.Bottom - diameter;
            path.AddArc(arcBounds, 0, 90);
            arcBounds.X = bounds.Left;
            path.AddArc(arcBounds, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void ApplyButtonStyle(Button button, ButtonVisualStyle visualStyle)
        {
            if (button == null)
            {
                return;
            }

            StyleButton(button, visualStyle);
        }

        public static Color GetFeedbackAccent(FeedbackTone tone)
        {
            if (tone == FeedbackTone.Success)
            {
                return Success;
            }

            if (tone == FeedbackTone.Warning)
            {
                return Warning;
            }

            if (tone == FeedbackTone.Error)
            {
                return Error;
            }

            return PrimaryButton;
        }

        public static Color GetFeedbackBackground(FeedbackTone tone)
        {
            var accent = GetFeedbackAccent(tone);
            return Blend(accent, Color.White, 0.84F);
        }

        public static Color GetFeedbackForeground(FeedbackTone tone)
        {
            if (tone == FeedbackTone.Error)
            {
                return Color.FromArgb(122, 36, 56);
            }

            if (tone == FeedbackTone.Warning)
            {
                return Color.FromArgb(109, 73, 18);
            }

            if (tone == FeedbackTone.Success)
            {
                return Color.FromArgb(32, 92, 62);
            }

            return TextPrimary;
        }

        public static string GetFeedbackPrefix(FeedbackTone tone)
        {
            if (tone == FeedbackTone.Success)
            {
                return "\u0413\u043e\u0442\u043e\u0432\u043e.";
            }

            if (tone == FeedbackTone.Warning)
            {
                return "\u0412\u043d\u0438\u043c\u0430\u043d\u0438\u0435.";
            }

            if (tone == FeedbackTone.Error)
            {
                return "\u041e\u0448\u0438\u0431\u043a\u0430.";
            }

            return "\u0418\u043d\u0444\u043e\u0440\u043c\u0430\u0446\u0438\u044f.";
        }

        private static void EnsureGlassSurface(Form form)
        {
            var surface = form.Controls.OfType<Control>().FirstOrDefault(control => control.Name == "liquidGlassSurface") as LiquidGlassSurface;
            if (surface == null)
            {
                surface = new LiquidGlassSurface();
                surface.Name = "liquidGlassSurface";
                form.Controls.Add(surface);
                surface.SendToBack();
            }
        }

        private static Label EnsureHeroLabel(Form form, string name, Font font, Color color, Point location)
        {
            var label = form.Controls.OfType<Label>().FirstOrDefault(control => control.Name == name);
            if (label == null)
            {
                label = new Label();
                label.Name = name;
                label.AutoSize = true;
                label.BackColor = Color.Transparent;
                form.Controls.Add(label);
            }

            label.Font = font;
            label.ForeColor = color;
            label.Location = location;
            label.BringToFront();
            return label;
        }

        private static void StyleControlCollection(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.Name == "liquidGlassSurface")
                {
                    continue;
                }

                StyleControl(control);

                if (!(control is DataGridView))
                {
                    StyleControlCollection(control.Controls);
                }
            }
        }

        private static void StyleControl(Control control)
        {
            if (control is DateTimePicker)
            {
                StyleDatePicker((DateTimePicker)control);
                return;
            }

            if (control is Label)
            {
                StyleLabel((Label)control);
                return;
            }

            if (control is Button)
            {
                StyleButton((Button)control, ButtonVisualStyle.Primary);
                return;
            }

            if (control is TextBox)
            {
                StyleTextBox((TextBox)control);
                return;
            }

            if (control is ComboBox)
            {
                StyleComboBox((ComboBox)control);
                return;
            }

            if (control is DataGridView)
            {
                StyleDataGrid((DataGridView)control);
            }
        }

        private static void StyleLabel(Label label)
        {
            if (label.Name == "liquidGlassHeroTitle" || label.Name == "liquidGlassHeroSubtitle")
            {
                return;
            }

            label.BackColor = Color.Transparent;
            label.ForeColor = TextPrimary;
            label.Font = LabelFont;
        }

        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = InputBackground;
            textBox.ForeColor = TextPrimary;
            textBox.Font = BodyFont;
        }

        private static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = InputBackground;
            comboBox.ForeColor = TextPrimary;
            comboBox.Font = BodyFont;
            comboBox.IntegralHeight = false;
        }

        private static void StyleDatePicker(DateTimePicker dateTimePicker)
        {
            dateTimePicker.CalendarForeColor = TextPrimary;
            dateTimePicker.CalendarMonthBackground = Color.White;
            dateTimePicker.CalendarTitleBackColor = PrimaryButton;
            dateTimePicker.CalendarTitleForeColor = Color.White;
            dateTimePicker.CalendarTrailingForeColor = TextMuted;
            dateTimePicker.Font = BodyFont;
        }

        private static void StyleDataGrid(DataGridView grid)
        {
            grid.BackgroundColor = GridBackground;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(224, 232, 244);
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.EditMode = DataGridViewEditMode.EditProgrammatically;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(195, 224, 255);
            grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
            grid.DefaultCellStyle.Font = BodyFont;
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAlternate;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(214, 231, 255);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = LabelFont;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.RowTemplate.Height = 34;
        }

        private static void StyleButton(Button button, ButtonVisualStyle visualStyle)
        {
            var baseColor = ResolveButtonColor(visualStyle);

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Blend(baseColor, Color.White, 0.14F);
            button.FlatAppearance.MouseDownBackColor = Blend(baseColor, Color.Black, 0.08F);
            button.UseVisualStyleBackColor = false;
            button.BackColor = baseColor;
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;
            button.Font = LabelFont;
            button.Padding = new Padding(10, 0, 10, 0);
            button.Height = Math.Max(button.Height, 34);

            button.Resize -= HandleButtonResize;
            button.Resize += HandleButtonResize;
            ApplyRoundedRegion(button, 16);
        }

        private static void HandleButtonResize(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            ApplyRoundedRegion(button, 16);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            var previousRegion = control.Region;
            using (var path = CreateRoundedPath(new Rectangle(Point.Empty, control.Size), radius))
            {
                control.Region = new Region(path);
            }

            if (previousRegion != null)
            {
                previousRegion.Dispose();
            }
        }

        private static Color ResolveButtonColor(ButtonVisualStyle visualStyle)
        {
            if (visualStyle == ButtonVisualStyle.Destructive)
            {
                return DangerButton;
            }

            if (visualStyle == ButtonVisualStyle.Secondary)
            {
                return SecondaryButton;
            }

            if (visualStyle == ButtonVisualStyle.Accent)
            {
                return AccentButton;
            }

            return PrimaryButton;
        }

        private static Color Blend(Color source, Color target, float amount)
        {
            var clampedAmount = Math.Max(0F, Math.Min(1F, amount));
            var red = (int)(source.R + ((target.R - source.R) * clampedAmount));
            var green = (int)(source.G + ((target.G - source.G) * clampedAmount));
            var blue = (int)(source.B + ((target.B - source.B) * clampedAmount));
            return Color.FromArgb(source.A, red, green, blue);
        }

        private static void HandleHeroResize(object sender, EventArgs e)
        {
            var form = sender as Form;
            if (form == null)
            {
                return;
            }

            ResizeHeroSubtitle(form);
        }

        private static void ResizeHeroSubtitle(Form form)
        {
            var subtitleLabel = form.Controls.OfType<Label>().FirstOrDefault(control => control.Name == "liquidGlassHeroSubtitle");
            if (subtitleLabel != null)
            {
                subtitleLabel.MaximumSize = new Size(Math.Max(220, form.ClientSize.Width - 64), 0);
            }
        }
    }
}
