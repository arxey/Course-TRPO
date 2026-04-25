using System;
using System.Drawing;
using System.Windows.Forms;
using GymManagementClient.Data;
using GymManagementClient.UI;

namespace GymManagementClient.Forms
{
    public partial class PaymentsForm : LiquidGlassForm
    {
        private readonly DatabaseClient _databaseClient = new DatabaseClient();
        private FeedbackMessageBar _feedbackBar;

        public PaymentsForm()
        {
            InitializeComponent();
            ConfigureLayout();
            ApplyLiquidGlassTheme();
            LiquidGlassTheme.SetHero(this, "Оплаты", "Прозрачная финансовая сводка по истории платежей клиентов.");
        }

        private void PaymentsForm_Load(object sender, EventArgs e)
        {
            TryRunUiAction(() =>
            {
                var paymentHistoryTable = _databaseClient.ExecuteQuery("SELECT * FROM v_payment_history");
                dgvPayments.DataSource = paymentHistoryTable;
                ShowFeedback(_feedbackBar, FeedbackTone.Info, string.Format("История оплат загружена. Строк в таблице: {0}.", paymentHistoryTable.Rows.Count));
            }, "Не удалось загрузить историю оплат.", _feedbackBar);
        }

        private void ConfigureLayout()
        {
            SuspendLayout();

            Text = "История оплат";
            ClientSize = new Size(980, 520);
            MinimumSize = new Size(900, 500);

            var contentRoot = CreatePageLayout(out _feedbackBar);
            contentRoot.AutoSize = false;
            contentRoot.RowStyles.Clear();
            contentRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            dgvPayments.Dock = DockStyle.Fill;
            dgvPayments.Margin = new Padding(0);
            contentRoot.Controls.Add(dgvPayments, 0, 0);

            ResumeLayout(true);
        }
    }
}
