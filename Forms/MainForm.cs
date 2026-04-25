using System;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;

namespace GymManagementClient.Forms
{
    public partial class MainForm : LiquidGlassForm
    {
        private FeedbackMessageBar _feedbackBar;

        public MainForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Gym Management", string.Format("Панель управления. Текущая роль: {0}.", GetRoleDisplayName(SessionContext.CurrentRole)));
            ApplyRolePermissions();
        }

        private void ApplyRolePermissions()
        {
            var normalizedRole = SessionContext.CurrentRole == null
                ? string.Empty
                : SessionContext.CurrentRole.Trim().ToLowerInvariant();
            var isAdministrator = normalizedRole == "admin";
            var isTrainer = normalizedRole == "trainer";
            var isClient = normalizedRole == "client";

            btnClients.Enabled = isAdministrator;
            btnSchedule.Enabled = isAdministrator || isTrainer;
            btnPayments.Enabled = isAdministrator;
            btnRegisterSession.Enabled = isAdministrator || isTrainer;
            btnStats.Enabled = isAdministrator || isTrainer;

            if (isClient)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Info, "Вы вошли как клиент. Для записи на занятие обратитесь к администратору или тренеру.");
            }
            else
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Info, string.Format("Вы вошли как {0}. Выберите нужный раздел.", GetRoleDisplayName(SessionContext.CurrentRole)));
            }
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Панель управления";
            ClientSize = new Size(980, 470);
            MinimumSize = new Size(900, 420);

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.Controls.Add(CreateDashboardLayout(), 0, 0);

            ResumeLayout(true);
        }

        private TableLayoutPanel CreateDashboardLayout()
        {
            var dashboardLayout = new TableLayoutPanel();
            dashboardLayout.BackColor = Color.Transparent;
            dashboardLayout.ColumnCount = 3;
            dashboardLayout.Dock = DockStyle.Fill;
            dashboardLayout.Margin = new Padding(0);
            dashboardLayout.RowCount = 2;
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            AddDashboardButton(dashboardLayout, btnClients, ButtonVisualStyle.Primary, 0, 0, 0);
            AddDashboardButton(dashboardLayout, btnSchedule, ButtonVisualStyle.Accent, 1, 0, 1);
            AddDashboardButton(dashboardLayout, btnPayments, ButtonVisualStyle.Accent, 2, 0, 2);
            AddDashboardButton(dashboardLayout, btnRegisterSession, ButtonVisualStyle.Primary, 0, 1, 3);
            AddDashboardButton(dashboardLayout, btnStats, ButtonVisualStyle.Accent, 1, 1, 4);
            AddDashboardButton(dashboardLayout, btnExit, ButtonVisualStyle.Secondary, 2, 1, 5);

            return dashboardLayout;
        }

        private static void AddDashboardButton(TableLayoutPanel dashboardLayout, Button button, ButtonVisualStyle visualStyle, int column, int row, int tabIndex)
        {
            var hasRightGap = column < dashboardLayout.ColumnCount - 1;
            var hasBottomGap = row < dashboardLayout.RowCount - 1;

            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(0, 0, hasRightGap ? 16 : 0, hasBottomGap ? 16 : 0);
            button.MinimumSize = new Size(220, 78);
            button.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            button.TabIndex = tabIndex;
            button.TextAlign = ContentAlignment.MiddleCenter;
            LiquidGlassTheme.ApplyButtonStyle(button, visualStyle);
            dashboardLayout.Controls.Add(button, column, row);
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            OpenDialog(new ClientsForm());
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            OpenDialog(new ScheduleForm());
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            OpenDialog(new PaymentsForm());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegisterSession_Click(object sender, EventArgs e)
        {
            OpenDialog(new RegisterSessionForm());
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            OpenDialog(new TrainerStatsForm());
        }

        private void OpenDialog(Form dialog)
        {
            using (dialog)
            {
                dialog.ShowDialog(this);
            }
        }

        private static string GetRoleDisplayName(string role)
        {
            var normalizedRole = role == null ? string.Empty : role.Trim().ToLowerInvariant();
            if (normalizedRole == "admin")
            {
                return "администратор";
            }

            if (normalizedRole == "trainer")
            {
                return "тренер";
            }

            if (normalizedRole == "client")
            {
                return "клиент";
            }

            return string.IsNullOrWhiteSpace(role) ? "пользователь" : role;
        }
    }
}
