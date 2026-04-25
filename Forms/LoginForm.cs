using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;
using Npgsql;

namespace GymManagementClient.Forms
{
    public partial class LoginForm : LiquidGlassForm
    {
        private const int MaxLoginAttempts = 5;
        private readonly Random _random = new Random();
        private int _captchaAnswer;
        private int _loginAttempts;
        private FeedbackMessageBar _feedbackBar;

        public LoginForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Gym Management", "Добро пожаловать обратно. Введите учётные данные, чтобы продолжить работу.");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_loginAttempts >= MaxLoginAttempts)
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Error, string.Format("Превышено число попыток ({0}). Перезапустите приложение.", MaxLoginAttempts));
                btnLogin.Enabled = false;
                return;
            }

            var userName = txtLogin.Text.Trim();
            var password = txtPassword.Text;
            var captchaText = txtCaptcha.Text.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(captchaText))
            {
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, "Введите логин, пароль и ответ на капчу.");
                return;
            }

            int enteredCaptchaAnswer;
            if (!int.TryParse(captchaText, out enteredCaptchaAnswer) || enteredCaptchaAnswer != _captchaAnswer)
            {
                _loginAttempts++;
                ShowFeedback(_feedbackBar, FeedbackTone.Warning, string.Format("Капча введена неверно. Осталось попыток: {0}.", MaxLoginAttempts - _loginAttempts));
                txtCaptcha.Clear();
                GenerateCaptcha();
                txtCaptcha.Focus();
                return;
            }

            var connectionString = CreateConnectionString(userName, password);

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    SessionContext.ResetSession();
                    SessionContext.CurrentRole = ResolveCurrentRole(connection, userName);
                }

                SessionContext.ConnectionString = connectionString;
                _loginAttempts = 0;
                OpenMainForm();
            }
            catch (Exception ex)
            {
                _loginAttempts++;
                var remaining = MaxLoginAttempts - _loginAttempts;
                var hint = remaining > 0
                    ? string.Format(" Осталось попыток: {0}.", remaining)
                    : " Дальнейшие попытки заблокированы.";
                ShowFeedback(_feedbackBar, FeedbackTone.Error, "Не удалось выполнить вход. " + ex.Message + hint);
                txtCaptcha.Clear();
                GenerateCaptcha();
                txtPassword.Focus();
            }
        }

        private void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            txtCaptcha.Clear();
            GenerateCaptcha();
            ShowFeedback(_feedbackBar, FeedbackTone.Info, "Капча обновлена.");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SessionContext.ResetSession();
            Close();
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "Вход в систему";
            ClientSize = new Size(540, 430);
            MinimumSize = new Size(470, 380);
            AcceptButton = btnLogin;

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentRoot.Controls.Add(CreateCenteredLoginLayout(), 0, 0);

            ResumeLayout(true);
        }

        private TableLayoutPanel CreateCenteredLoginLayout()
        {
            var outerLayout = new TableLayoutPanel();
            outerLayout.BackColor = Color.Transparent;
            outerLayout.ColumnCount = 3;
            outerLayout.Dock = DockStyle.Fill;
            outerLayout.Margin = new Padding(0);
            outerLayout.RowCount = 1;
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            outerLayout.Controls.Add(CreateLoginFormLayout(), 1, 0);
            return outerLayout;
        }

        private TableLayoutPanel CreateLoginFormLayout()
        {
            var loginLayout = CreateTransparentTableLayout(1, 7);
            loginLayout.Dock = DockStyle.Top;
            loginLayout.MaximumSize = new Size(420, 0);
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            loginLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            labelLogin.Margin = new Padding(0, 0, 0, 6);
            txtLogin.Dock = DockStyle.Top;
            txtLogin.Margin = new Padding(0, 0, 0, 14);
            txtLogin.MinimumSize = new Size(0, 28);

            labelPassword.Margin = new Padding(0, 0, 0, 6);
            txtPassword.Dock = DockStyle.Top;
            txtPassword.Margin = new Padding(0, 0, 0, 14);
            txtPassword.MinimumSize = new Size(0, 28);
            txtPassword.UseSystemPasswordChar = true;

            lblCaptchaTask.Margin = new Padding(0, 0, 0, 6);

            loginLayout.Controls.Add(labelLogin, 0, 0);
            loginLayout.Controls.Add(txtLogin, 0, 1);
            loginLayout.Controls.Add(labelPassword, 0, 2);
            loginLayout.Controls.Add(txtPassword, 0, 3);
            loginLayout.Controls.Add(lblCaptchaTask, 0, 4);
            loginLayout.Controls.Add(CreateCaptchaRow(), 0, 5);

            btnLogin.Dock = DockStyle.Top;
            btnLogin.Height = 40;
            btnLogin.Margin = new Padding(0);
            btnLogin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            LiquidGlassTheme.ApplyButtonStyle(btnLogin, ButtonVisualStyle.Primary);
            loginLayout.Controls.Add(btnLogin, 0, 6);

            return loginLayout;
        }

        private TableLayoutPanel CreateCaptchaRow()
        {
            var captchaLayout = CreateTransparentTableLayout(2, 1);
            captchaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            captchaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            captchaLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            captchaLayout.Margin = new Padding(0, 0, 0, 16);

            txtCaptcha.Dock = DockStyle.Fill;
            txtCaptcha.Margin = new Padding(0, 0, 12, 0);
            txtCaptcha.MinimumSize = new Size(0, 28);

            btnRefreshCaptcha.Dock = DockStyle.Fill;
            btnRefreshCaptcha.Height = 36;
            btnRefreshCaptcha.Margin = new Padding(0);
            btnRefreshCaptcha.Text = "Обновить капчу";
            LiquidGlassTheme.ApplyButtonStyle(btnRefreshCaptcha, ButtonVisualStyle.Accent);

            captchaLayout.Controls.Add(txtCaptcha, 0, 0);
            captchaLayout.Controls.Add(btnRefreshCaptcha, 1, 0);
            return captchaLayout;
        }

        private void GenerateCaptcha()
        {
            var firstOperand = _random.Next(1, 10);
            var secondOperand = _random.Next(1, 10);
            var useAddition = _random.Next(0, 2) == 0;

            if (useAddition)
            {
                _captchaAnswer = firstOperand + secondOperand;
                lblCaptchaTask.Text = string.Format("Сколько будет {0} + {1}?", firstOperand, secondOperand);
                return;
            }

            if (firstOperand < secondOperand)
            {
                var largerOperand = secondOperand;
                secondOperand = firstOperand;
                firstOperand = largerOperand;
            }

            _captchaAnswer = firstOperand - secondOperand;
            lblCaptchaTask.Text = string.Format("Сколько будет {0} - {1}?", firstOperand, secondOperand);
        }

        private static string CreateConnectionString(string userName, string password)
        {
            var host = ConfigurationManager.AppSettings["DbHost"] ?? "localhost";
            var portText = ConfigurationManager.AppSettings["DbPort"] ?? "5432";
            var database = ConfigurationManager.AppSettings["DbName"] ?? "gym_management";

            int port;
            if (!int.TryParse(portText, out port) || port <= 0 || port > 65535)
            {
                port = 5432;
            }

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
            connectionStringBuilder.Host = host;
            connectionStringBuilder.Port = port;
            connectionStringBuilder.Database = database;
            connectionStringBuilder.Username = userName;
            connectionStringBuilder.Password = password;
            return connectionStringBuilder.ConnectionString;
        }

        private void OpenMainForm()
        {
            var mainForm = new MainForm();
            mainForm.FormClosed += MainForm_FormClosed;
            mainForm.Show();
            Hide();
        }

        private static string ResolveCurrentRole(NpgsqlConnection connection, string fallbackRole)
        {
            const string resolveRoleQuery = @"
                SELECT rolname
                FROM pg_roles
                WHERE rolname IN ('admin', 'trainer', 'client')
                  AND pg_has_role(current_user, oid, 'member')
                ORDER BY CASE rolname
                    WHEN 'admin' THEN 1
                    WHEN 'trainer' THEN 2
                    WHEN 'client' THEN 3
                    ELSE 4
                END
                LIMIT 1";

            using (var command = new NpgsqlCommand(resolveRoleQuery, connection))
            {
                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    return fallbackRole;
                }

                return result.ToString();
            }
        }
    }
}
