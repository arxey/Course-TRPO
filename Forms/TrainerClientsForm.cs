using System;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;
using Npgsql;

namespace GymManagementClient.Forms
{
    public partial class TrainerClientsForm : LiquidGlassForm
    {
        private readonly DatabaseClient _databaseClient = new DatabaseClient();
        private readonly int _trainerId;
        private FeedbackMessageBar _feedbackBar;

        public TrainerClientsForm(int trainerId)
        {
            InitializeComponent();
            _trainerId = trainerId;
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Клиенты тренера", "Выбранный тренер будет подставлен автоматически после загрузки списка.");
        }

        private void TrainerClientsForm_Load(object sender, EventArgs e)
        {
            RefreshTrainerClients();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RefreshTrainerClients()
        {
            TryRunUiAction(() =>
            {
                var trainerClientsTable = _databaseClient.ExecuteQuery(
                    @"SELECT DISTINCT c.client_id, c.full_name, c.phone, c.email
                      FROM session_registrations r
                      JOIN sessions s ON r.session_id = s.session_id
                      JOIN clients c ON r.client_id = c.client_id
                      WHERE s.trainer_id = @id
                      ORDER BY full_name",
                    new NpgsqlParameter("@id", _trainerId));

                dgvClients.DataSource = trainerClientsTable;

                var trainerName = LoadTrainerName();
                LiquidGlassTheme.SetHero(this, "Клиенты тренера", trainerName);
                ShowFeedback(_feedbackBar, FeedbackTone.Info, string.Format("Тренер: {0}. Найдено клиентов: {1}.", trainerName, trainerClientsTable.Rows.Count));
            }, "Не удалось загрузить клиентов тренера.", _feedbackBar);
        }

        private string LoadTrainerName()
        {
            var trainerTable = _databaseClient.ExecuteQuery(
                "SELECT full_name FROM trainers WHERE trainer_id = @id",
                new NpgsqlParameter("@id", _trainerId));

            return trainerTable.Rows.Count > 0 ? trainerTable.Rows[0]["full_name"].ToString() : string.Empty;
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Клиенты тренера";
            ClientSize = new Size(980, 560);
            MinimumSize = new Size(900, 500);

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowCount = 2;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            dgvClients.Dock = DockStyle.Fill;
            dgvClients.Margin = new Padding(0, 0, 0, 18);

            btnClose.Margin = new Padding(0);
            btnClose.Width = 160;
            btnClose.Height = 40;
            LiquidGlassTheme.ApplyButtonStyle(btnClose, ButtonVisualStyle.Secondary);

            var actionPanel = CreateActionPanel();
            actionPanel.Controls.Add(btnClose);

            contentRoot.Controls.Add(dgvClients, 0, 0);
            contentRoot.Controls.Add(actionPanel, 0, 1);

            ResumeLayout(true);
        }
    }
}
