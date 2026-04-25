using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;
using Npgsql;

namespace GymManagementClient.Forms
{
    public partial class RegisterSessionForm : LiquidGlassForm
    {
        private readonly DatabaseClient _databaseClient = new DatabaseClient();
        private FeedbackMessageBar _feedbackBar;

        public RegisterSessionForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Запись на занятие", "Свяжите клиента с нужной тренировкой в несколько кликов.");
        }

        private void RegisterSessionForm_Load(object sender, EventArgs e)
        {
            TryRunUiAction(() =>
            {
                LoadClientOptions();
                LoadSessionOptions();
                ShowFeedback(_feedbackBar, FeedbackTone.Info, "Данные загружены. Выберите клиента и занятие.");
            }, "Не удалось загрузить данные формы.", _feedbackBar);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (cbClients.SelectedValue == null || cbSessions.SelectedValue == null)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Выберите клиента и занятие.");
                return;
            }

            int selectedClientId;
            int selectedSessionId;

            try
            {
                selectedClientId = Convert.ToInt32(cbClients.SelectedValue);
                selectedSessionId = Convert.ToInt32(cbSessions.SelectedValue);
            }
            catch
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Не удалось определить выбранного клиента или занятие.");
                return;
            }

            try
            {
                // Проверяем, не записан ли клиент на это занятие уже
                const string checkDuplicateQuery = "SELECT COUNT(*) FROM session_registrations WHERE client_id = @client AND session_id = @session";
                int existingCount;
                using (var checkTable = _databaseClient.ExecuteQuery(
                    checkDuplicateQuery,
                    new NpgsqlParameter("@client", selectedClientId),
                    new NpgsqlParameter("@session", selectedSessionId)))
                {
                    existingCount = Convert.ToInt32(checkTable.Rows[0][0]);
                }

                if (existingCount > 0)
                {
                    ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Клиент уже записан на это занятие.");
                    return;
                }

                const string registerSessionCommand = @"INSERT INTO session_registrations (client_id, session_id, registration_date)
                                                        VALUES (@client, @session, CURRENT_DATE)";

                _databaseClient.ExecuteNonQuery(
                    registerSessionCommand,
                    new NpgsqlParameter("@client", selectedClientId),
                    new NpgsqlParameter("@session", selectedSessionId));

                ShowFeedback(_feedbackBar, FeedbackTone.Success, "Клиент успешно записан на занятие.");
            }
            catch (PostgresException ex)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Error, ex.MessageText);
            }
            catch (Exception ex)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Error, "Не удалось записать клиента: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadClientOptions()
        {
            var clientsTable = _databaseClient.ExecuteQuery("SELECT client_id, full_name FROM clients ORDER BY full_name");
            cbClients.DataSource = clientsTable;
            cbClients.DisplayMember = "full_name";
            cbClients.ValueMember = "client_id";
        }

        private void LoadSessionOptions()
        {
            var sessionsTable = _databaseClient.ExecuteQuery(
                "SELECT session_id, training_type || ' - ' || day_of_week || ' ' || start_time AS session_summary FROM v_schedule");
            cbSessions.DataSource = sessionsTable;
            cbSessions.DisplayMember = "session_summary";
            cbSessions.ValueMember = "session_id";
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Запись клиента на занятие";
            ClientSize = new Size(620, 360);
            MinimumSize = new Size(560, 320);
            AcceptButton = btnRegister;
            CancelButton = btnCancel;

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.Controls.Add(CreateCenteredFormLayout(), 0, 0);

            ResumeLayout(true);
        }

        private TableLayoutPanel CreateCenteredFormLayout()
        {
            var outerLayout = new TableLayoutPanel();
            outerLayout.BackColor = Color.Transparent;
            outerLayout.ColumnCount = 3;
            outerLayout.Dock = DockStyle.Fill;
            outerLayout.Margin = new Padding(0);
            outerLayout.RowCount = 1;
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76F));
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            outerLayout.Controls.Add(CreateSessionRegistrationLayout(), 1, 0);
            return outerLayout;
        }

        private TableLayoutPanel CreateSessionRegistrationLayout()
        {
            var registrationLayout = CreateTransparentTableLayout(1, 5);
            registrationLayout.Dock = DockStyle.Top;
            registrationLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            registrationLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            registrationLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            registrationLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            registrationLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblClient.Margin = new Padding(0, 0, 0, 6);
            lblSession.Margin = new Padding(0, 0, 0, 6);

            cbClients.Dock = DockStyle.Fill;
            cbClients.Margin = new Padding(0, 0, 0, 14);
            cbClients.MinimumSize = new Size(0, 30);

            cbSessions.Dock = DockStyle.Fill;
            cbSessions.Margin = new Padding(0, 0, 0, 18);
            cbSessions.MinimumSize = new Size(0, 30);

            registrationLayout.Controls.Add(lblClient, 0, 0);
            registrationLayout.Controls.Add(cbClients, 0, 1);
            registrationLayout.Controls.Add(lblSession, 0, 2);
            registrationLayout.Controls.Add(cbSessions, 0, 3);
            registrationLayout.Controls.Add(CreateActionButtonsLayout(), 0, 4);

            return registrationLayout;
        }

        private TableLayoutPanel CreateActionButtonsLayout()
        {
            var actionsLayout = CreateTransparentTableLayout(2, 1);
            actionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            actionsLayout.Margin = new Padding(0);

            btnRegister.Dock = DockStyle.Fill;
            btnCancel.Dock = DockStyle.Fill;
            btnRegister.Margin = new Padding(0, 0, 10, 0);
            btnCancel.Margin = new Padding(10, 0, 0, 0);
            btnRegister.Height = 40;
            btnCancel.Height = 40;

            LiquidGlassTheme.ApplyButtonStyle(btnRegister, ButtonVisualStyle.Primary);
            LiquidGlassTheme.ApplyButtonStyle(btnCancel, ButtonVisualStyle.Secondary);

            actionsLayout.Controls.Add(btnRegister, 0, 0);
            actionsLayout.Controls.Add(btnCancel, 1, 0);
            return actionsLayout;
        }
    }
}
