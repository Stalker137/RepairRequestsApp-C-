using System;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class MainAppForm : Form
    {
        private int userID;
        private int roleID;

        public MainAppForm(int userID, int roleID)
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;

            if (roleID != 1)
            {
                btnClients.Visible = false;
                btnStatistics.Visible = false;
            }
        }

        public MainAppForm() : this(0, 0) { }

        private void BtnRequests_Click(object sender, EventArgs e)
        {
            RequestManagementForm form = new RequestManagementForm(userID, roleID);
            form.Show();
            this.Hide();
        }

        private void BtnClients_Click(object sender, EventArgs e)
        {
            ClientsManagementForm form = new ClientsManagementForm(userID, roleID);
            form.Show();
            this.Hide();
        }

        private void BtnStatistics_Click(object sender, EventArgs e)
        {
            StatisticsForm form = new StatisticsForm(userID, roleID);
            form.Show();
            this.Hide();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Restart();
        }
    }
}