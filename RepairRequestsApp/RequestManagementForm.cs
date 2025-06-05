using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class RequestManagementForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int userID;
        private int roleID;

        public RequestManagementForm(int userID, int roleID)
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;
            LoadStatuses();
            LoadRequests();
        }

        public RequestManagementForm() : this(0, 0) { }

        private void LoadStatuses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT StatusID, StatusName FROM Statuses";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        DataRow allRow = table.NewRow();
                        allRow["StatusID"] = -1;
                        allRow["StatusName"] = "Все статусы";
                        table.Rows.InsertAt(allRow, 0);

                        cmbStatusFilter.DataSource = table;
                        cmbStatusFilter.DisplayMember = "StatusName";
                        cmbStatusFilter.ValueMember = "StatusID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки статусов: " + ex.Message);
                }
            }
        }

        private void LoadRequests()
        {
            int statusID = Convert.ToInt32(cmbStatusFilter.SelectedValue);
            string query = statusID == -1 ?
                "SELECT r.RequestID AS [Номер_заявки], r.Equipment AS [Оборудование], c.Name AS [Клиент], ft.TypeName AS [Тип_неисправности], s.StatusName AS [Статус], r.DateAdded AS [Дата_добавления] FROM Requests r JOIN Clients c ON r.ClientID = c.ClientID JOIN FaultTypes ft ON r.FaultTypeID = ft.FaultTypeID JOIN Statuses s ON r.StatusID = s.StatusID" :
                "SELECT r.RequestID AS [Номер_заявки], r.Equipment AS [Оборудование], c.Name AS [Клиент], ft.TypeName AS [Тип_неисправности], s.StatusName AS [Статус], r.DateAdded AS [Дата_добавления] FROM Requests r JOIN Clients c ON r.ClientID = c.ClientID JOIN FaultTypes ft ON r.FaultTypeID = ft.FaultTypeID JOIN Statuses s ON r.StatusID = s.StatusID WHERE r.StatusID = @statusID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (statusID != -1)
                        {
                            cmd.Parameters.AddWithValue("@statusID", statusID);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvRequests.AutoGenerateColumns = true;
                        dgvRequests.DataSource = table;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки заявок: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RequestDetailsForm form = new RequestDetailsForm(0, userID, roleID);
            form.ShowDialog();
            LoadRequests();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявку для редактирования.");
                return;
            }

            int requestID = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["Номер_заявки"].Value);
            RequestDetailsForm form = new RequestDetailsForm(userID, roleID, requestID);
            form.ShowDialog();
            LoadRequests();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявку для удаления.");
                return;
            }

            int requestID = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["Номер_заявки"].Value);
            if (MessageBox.Show("Удалить заявку?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Requests WHERE RequestID = @requestID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@requestID", requestID);
                            cmd.ExecuteNonQuery();
                        }
                        LoadRequests();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления заявки: " + ex.Message);
                    }
                }
            }
        }
        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRequests();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainAppForm mainForm = new MainAppForm(userID, roleID);
            mainForm.Show();
            this.Hide();
        }
    }
}