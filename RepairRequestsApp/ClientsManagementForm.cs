using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class ClientsManagementForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int userID;
        private int roleID;

        public ClientsManagementForm(int userID, int roleID)
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;
            LoadClients();
        }

        public ClientsManagementForm() : this(0, 0) { }

        private void LoadClients()
        {
            string query = "SELECT ClientID AS [Номер_клиента], Name AS [Название], ContactInfo AS [Контактная_информация] FROM Clients";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvClients.AutoGenerateColumns = true;
                        dgvClients.DataSource = table;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
                }
            }
        }

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            AddClientForm form = new AddClientForm(userID, roleID);
            form.ShowDialog();
            LoadClients();
        }

        private void btnEditClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для редактирования.");
                return;
            }

            int clientID = Convert.ToInt32(dgvClients.SelectedRows[0].Cells["Номер_клиента"].Value);
            AddClientForm form = new AddClientForm(userID, roleID, clientID);
            form.ShowDialog();
            LoadClients();
        }

        private void btnDeleteClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления.");
                return;
            }

            int clientID = Convert.ToInt32(dgvClients.SelectedRows[0].Cells["Номер_клиента"].Value);
            if (MessageBox.Show("Удалить клиента?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Clients WHERE ClientID = @clientID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@clientID", clientID);
                            cmd.ExecuteNonQuery();
                        }
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления клиента: " + ex.Message);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainAppForm mainForm = new MainAppForm(userID, roleID);
            mainForm.Show();
            this.Hide();
        }
    }
}