using System;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;

namespace GymManagementClient.Forms
{
    public partial class ScheduleForm : LiquidGlassForm
    {
        private readonly DatabaseClient _databaseClient = new DatabaseClient();
        private FeedbackMessageBar _feedbackBar;

        public ScheduleForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Расписание", "Актуальная сетка тренировок с быстрым обновлением по запросу.");
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            RefreshSchedule();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshSchedule();
        }

        private void RefreshSchedule()
        {
            TryRunUiAction(() =>
            {
                var scheduleTable = _databaseClient.ExecuteQuery("SELECT * FROM v_schedule");
                dgvSchedule.DataSource = scheduleTable;
                ShowFeedback(_feedbackBar, FeedbackTone.Info, string.Format("Расписание обновлено. Строк в таблице: {0}.", scheduleTable.Rows.Count));
            }, "Не удалось загрузить расписание.", _feedbackBar);
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Расписание тренировок";
            ClientSize = new Size(1040, 560);
            MinimumSize = new Size(900, 500);

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowCount = 2;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            dgvSchedule.Dock = DockStyle.Fill;
            dgvSchedule.Margin = new Padding(0, 0, 0, 18);

            btnLoad.Margin = new Padding(0);
            btnLoad.Width = 180;
            btnLoad.Height = 40;
            LiquidGlassTheme.ApplyButtonStyle(btnLoad, ButtonVisualStyle.Accent);

            var actionPanel = CreateActionPanel();
            actionPanel.Controls.Add(btnLoad);

            contentRoot.Controls.Add(dgvSchedule, 0, 0);
            contentRoot.Controls.Add(actionPanel, 0, 1);

            ResumeLayout(true);
        }
    }
}
