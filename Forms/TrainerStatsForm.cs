using System;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;

namespace GymManagementClient.Forms
{
    public partial class TrainerStatsForm : LiquidGlassForm
    {
        private readonly DatabaseClient _databaseClient = new DatabaseClient();
        private FeedbackMessageBar _feedbackBar;

        public TrainerStatsForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Статистика тренеров", "Двойной клик по строке откроет список клиентов выбранного тренера.");
        }

        private void TrainerStatsForm_Load(object sender, EventArgs e)
        {
            RefreshTrainerStatistics();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshTrainerStatistics();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvStats_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var row = dgvStats.Rows[e.RowIndex];
            if (!dgvStats.Columns.Contains("trainer_id"))
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Столбец trainer_id не найден в таблице.");
                return;
            }

            var cellValue = row.Cells["trainer_id"].Value;
            if (cellValue == null || cellValue == DBNull.Value)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Идентификатор тренера не определён.");
                return;
            }

            var selectedTrainerId = Convert.ToInt32(cellValue);

            using (var trainerClientsForm = new TrainerClientsForm(selectedTrainerId))
            {
                trainerClientsForm.ShowDialog(this);
            }
        }

        private void RefreshTrainerStatistics()
        {
            TryRunUiAction(() =>
            {
                var trainerStatsTable = _databaseClient.ExecuteQuery("SELECT * FROM v_trainer_stats ORDER BY total_clients DESC");
                dgvStats.DataSource = trainerStatsTable;
                ShowFeedback(_feedbackBar, FeedbackTone.Info, string.Format("Статистика обновлена. В таблице: {0} тренеров.", trainerStatsTable.Rows.Count));
            }, "Не удалось загрузить статистику тренеров.", _feedbackBar);
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Статистика тренеров";
            ClientSize = new Size(1040, 620);
            MinimumSize = new Size(920, 560);

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowCount = 2;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            dgvStats.Dock = DockStyle.Fill;
            dgvStats.Margin = new Padding(0, 0, 0, 18);

            btnRefresh.Margin = new Padding(0, 0, 12, 0);
            btnClose.Margin = new Padding(0);
            btnRefresh.Width = 180;
            btnClose.Width = 160;
            btnRefresh.Height = 40;
            btnClose.Height = 40;
            LiquidGlassTheme.ApplyButtonStyle(btnRefresh, ButtonVisualStyle.Accent);
            LiquidGlassTheme.ApplyButtonStyle(btnClose, ButtonVisualStyle.Secondary);

            var actionPanel = CreateActionPanel();
            actionPanel.Controls.Add(btnRefresh);
            actionPanel.Controls.Add(btnClose);

            contentRoot.Controls.Add(dgvStats, 0, 0);
            contentRoot.Controls.Add(actionPanel, 0, 1);

            ResumeLayout(true);
        }
    }
}
