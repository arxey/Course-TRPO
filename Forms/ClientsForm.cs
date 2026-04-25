using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;
using Npgsql;

namespace GymManagementClient.Forms
{
    public partial class ClientsForm : LiquidGlassForm
    {
        private readonly DatabaseClient _databaseClient = new DatabaseClient();
        private FeedbackMessageBar _feedbackBar;

        public ClientsForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Клиенты", "Управляйте карточками клиентов, обновляйте контакты и отслеживайте регистрацию.");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshClientList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var fullName = txtName.Text.Trim();
            var phoneNumber = txtPhone.Text.Trim();
            var emailAddress = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Укажите имя клиента перед сохранением.");
                txtName.Focus();
                return;
            }

            const string insertClientCommand = @"INSERT INTO clients(full_name, phone, email, registration_date)
                                                 VALUES (@name, @phone, @mail, CURRENT_DATE)";

            TryRunUiAction(() =>
            {
                _databaseClient.ExecuteNonQuery(
                    insertClientCommand,
                    new NpgsqlParameter("@name", fullName),
                    new NpgsqlParameter("@phone", string.IsNullOrWhiteSpace(phoneNumber) ? (object)DBNull.Value : phoneNumber),
                    new NpgsqlParameter("@mail", string.IsNullOrWhiteSpace(emailAddress) ? (object)DBNull.Value : emailAddress));

                RefreshClientList();
                ResetClientEditor();
                ShowFeedback(_feedbackBar, FeedbackTone.Success, "Карточка клиента добавлена.");
            }, "Не удалось добавить клиента.", _feedbackBar);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvClients.CurrentRow == null)
            {
                return;
            }

            var idCell = dgvClients.CurrentRow.Cells["client_id"];
            if (idCell == null || idCell.Value == null || idCell.Value == DBNull.Value)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Не удалось определить идентификатор клиента.");
                return;
            }

            var selectedClientId = Convert.ToInt32(idCell.Value);

            // Проверяем наличие связанных записей на занятия
            const string checkRegistrationsQuery = "SELECT COUNT(*) FROM session_registrations WHERE client_id = @id";
            int registrationCount;
            try
            {
                using (var countTable = _databaseClient.ExecuteQuery(checkRegistrationsQuery, new NpgsqlParameter("@id", selectedClientId)))
                {
                    registrationCount = Convert.ToInt32(countTable.Rows[0][0]);
                }
            }
            catch
            {
                registrationCount = 0;
            }

            if (registrationCount > 0)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Error,
                    string.Format("Невозможно удалить: у клиента есть {0} запис(ей) на занятия.", registrationCount));
                return;
            }

            if (MessageBox.Show("Удалить клиента? Это действие необратимо.", "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            const string deleteClientCommand = "DELETE FROM clients WHERE client_id = @id";
            TryRunUiAction(() =>
            {
                _databaseClient.ExecuteNonQuery(deleteClientCommand, new NpgsqlParameter("@id", selectedClientId));
                RefreshClientList();
                ShowFeedback(_feedbackBar, FeedbackTone.Success, "Клиент удалён.");
            }, "Не удалось удалить клиента.", _feedbackBar);
        }

        private void RefreshClientList()
        {
            TryRunUiAction(() =>
            {
                var clientsTable = _databaseClient.ExecuteQuery(
                    "SELECT client_id, full_name, phone, email, registration_date FROM clients ORDER BY client_id");
                dgvClients.DataSource = clientsTable;
                ShowFeedback(_feedbackBar, FeedbackTone.Info, string.Format("Список клиентов обновлён. Найдено записей: {0}.", clientsTable.Rows.Count));
            }, "Не удалось загрузить список клиентов.", _feedbackBar);
        }

        private void ResetClientEditor()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtName.Focus();
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Клиенты";
            ClientSize = new Size(1040, 680);
            MinimumSize = new Size(900, 600);

            labelName.Text = "ФИО клиента";
            labelPhone.Text = "Телефон";
            labelEmail.Text = "Электронная почта";

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowCount = 3;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            contentRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            ConfigureClientsGrid();
            contentRoot.Controls.Add(dgvClients, 0, 0);
            contentRoot.Controls.Add(CreateClientEditorLayout(), 0, 1);
            contentRoot.Controls.Add(CreateActionButtonsPanel(), 0, 2);

            ResumeLayout(true);
        }

        private void ConfigureClientsGrid()
        {
            dgvClients.Dock = DockStyle.Fill;
            dgvClients.Margin = new Padding(0, 0, 0, 18);
        }

        private TableLayoutPanel CreateClientEditorLayout()
        {
            var editorLayout = CreateTransparentTableLayout(3, 2);
            editorLayout.Dock = DockStyle.Top;
            editorLayout.Margin = new Padding(0, 0, 0, 18);
            editorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            editorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            editorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            labelName.Margin = new Padding(0, 0, 16, 6);
            labelPhone.Margin = new Padding(0, 0, 16, 6);
            labelEmail.Margin = new Padding(0, 0, 0, 6);

            txtName.Dock = DockStyle.Fill;
            txtPhone.Dock = DockStyle.Fill;
            txtEmail.Dock = DockStyle.Fill;
            txtName.Margin = new Padding(0, 0, 16, 0);
            txtPhone.Margin = new Padding(0, 0, 16, 0);
            txtEmail.Margin = new Padding(0);
            txtName.MinimumSize = new Size(0, 28);
            txtPhone.MinimumSize = new Size(0, 28);
            txtEmail.MinimumSize = new Size(0, 28);

            editorLayout.Controls.Add(labelName, 0, 0);
            editorLayout.Controls.Add(labelPhone, 1, 0);
            editorLayout.Controls.Add(labelEmail, 2, 0);
            editorLayout.Controls.Add(txtName, 0, 1);
            editorLayout.Controls.Add(txtPhone, 1, 1);
            editorLayout.Controls.Add(txtEmail, 2, 1);

            return editorLayout;
        }

        private FlowLayoutPanel CreateActionButtonsPanel()
        {
            var actionPanel = CreateActionPanel();

            btnLoad.Margin = new Padding(0, 0, 12, 0);
            btnAdd.Margin = new Padding(0, 0, 12, 0);
            btnDelete.Margin = new Padding(0);
            btnLoad.Width = 150;
            btnAdd.Width = 150;
            btnDelete.Width = 150;
            btnLoad.Height = 40;
            btnAdd.Height = 40;
            btnDelete.Height = 40;

            LiquidGlassTheme.ApplyButtonStyle(btnLoad, ButtonVisualStyle.Accent);
            LiquidGlassTheme.ApplyButtonStyle(btnAdd, ButtonVisualStyle.Primary);
            LiquidGlassTheme.ApplyButtonStyle(btnDelete, ButtonVisualStyle.Destructive);

            actionPanel.Controls.Add(btnLoad);
            actionPanel.Controls.Add(btnAdd);
            actionPanel.Controls.Add(btnDelete);
            return actionPanel;
        }
    }
}
