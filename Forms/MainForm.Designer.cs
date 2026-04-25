namespace GymManagementClient.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClients = new System.Windows.Forms.Button();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRegisterSession = new System.Windows.Forms.Button();
            this.btnStats = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClients
            // 
            this.btnClients.Location = new System.Drawing.Point(12, 22);
            this.btnClients.Name = "btnClients";
            this.btnClients.Size = new System.Drawing.Size(75, 23);
            this.btnClients.TabIndex = 0;
            this.btnClients.Text = "Клиенты";
            this.btnClients.UseVisualStyleBackColor = true;
            this.btnClients.Click += new System.EventHandler(this.btnClients_Click);
            // 
            // btnSchedule
            // 
            this.btnSchedule.Location = new System.Drawing.Point(93, 22);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(103, 23);
            this.btnSchedule.TabIndex = 1;
            this.btnSchedule.Text = "Расписание";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // btnPayments
            // 
            this.btnPayments.Location = new System.Drawing.Point(202, 22);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(75, 23);
            this.btnPayments.TabIndex = 2;
            this.btnPayments.Text = "Оплаты";
            this.btnPayments.UseVisualStyleBackColor = true;
            this.btnPayments.Click += new System.EventHandler(this.btnPayments_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(202, 64);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRegisterSession
            // 
            this.btnRegisterSession.Location = new System.Drawing.Point(283, 22);
            this.btnRegisterSession.Name = "btnRegisterSession";
            this.btnRegisterSession.Size = new System.Drawing.Size(138, 23);
            this.btnRegisterSession.TabIndex = 4;
            this.btnRegisterSession.Text = "Записать на занятие";
            this.btnRegisterSession.UseVisualStyleBackColor = true;
            this.btnRegisterSession.Click += new System.EventHandler(this.btnRegisterSession_Click);
            // 
            // btnStats
            // 
            this.btnStats.Location = new System.Drawing.Point(427, 22);
            this.btnStats.Name = "btnStats";
            this.btnStats.Size = new System.Drawing.Size(75, 23);
            this.btnStats.TabIndex = 5;
            this.btnStats.Text = "Статистика";
            this.btnStats.UseVisualStyleBackColor = true;
            this.btnStats.Click += new System.EventHandler(this.btnStats_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 116);
            this.Controls.Add(this.btnStats);
            this.Controls.Add(this.btnRegisterSession);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPayments);
            this.Controls.Add(this.btnSchedule);
            this.Controls.Add(this.btnClients);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Панель управления";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnClients;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegisterSession;
        private System.Windows.Forms.Button btnStats;
    }
}
