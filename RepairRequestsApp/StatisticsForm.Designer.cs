using System.Windows.Forms;

namespace RepairRequestsApp
{
    partial class StatisticsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Label lblAvgTime;
        private System.Windows.Forms.Label lblFaultStats;
        private System.Windows.Forms.Button btnClose;
        private DataGridView dgvFaultStats; 

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvFaultStats = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaultStats)).BeginInit();
            this.dgvFaultStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFaultStats.Location = new System.Drawing.Point(12, 100);
            this.dgvFaultStats.Name = "dgvFaultStats";
            this.dgvFaultStats.Size = new System.Drawing.Size(776, 300);
            this.dgvFaultStats.TabIndex = 0;
            this.dgvFaultStats.AutoGenerateColumns = true;

            this.lblCompleted = new System.Windows.Forms.Label();
            this.lblAvgTime = new System.Windows.Forms.Label();
            this.lblFaultStats = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCompleted
            // 
            this.lblCompleted.AutoSize = true;
            this.lblCompleted.Location = new System.Drawing.Point(20, 20);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(115, 13);
            this.lblCompleted.TabIndex = 0;
            this.lblCompleted.Text = "Выполнено заявок: 0";
            // 
            // lblAvgTime
            // 
            this.lblAvgTime.AutoSize = true;
            this.lblAvgTime.Location = new System.Drawing.Point(20, 40);
            this.lblAvgTime.Name = "lblAvgTime";
            this.lblAvgTime.Size = new System.Drawing.Size(189, 13);
            this.lblAvgTime.TabIndex = 1;
            this.lblAvgTime.Text = "Среднее время выполнения: 0 дней";
            // 
            // lblFaultStats
            // 
            this.lblFaultStats.AutoSize = true;
            this.lblFaultStats.Location = new System.Drawing.Point(20, 60);
            this.lblFaultStats.Name = "lblFaultStats";
            this.lblFaultStats.Size = new System.Drawing.Size(203, 13);
            this.lblFaultStats.TabIndex = 2;
            this.lblFaultStats.Text = "Статистика по типам неисправностей:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(23, 242);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(200, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // StatisticsForm
            // 
            this.ClientSize = new System.Drawing.Size(250, 284);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblFaultStats);
            this.Controls.Add(this.lblAvgTime);
            this.Controls.Add(this.lblCompleted);
            this.Name = "StatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Статистика";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}