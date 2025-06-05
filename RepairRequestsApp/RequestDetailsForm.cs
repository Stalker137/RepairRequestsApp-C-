using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class RequestDetailsForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int? requestID = null;
        private int userID;
        private int roleID;

        // Конструктор по умолчанию
        public RequestDetailsForm() : this(0, 0, 0) { }

        // Конструктор с requestID
        public RequestDetailsForm(int requestID) : this(requestID, 0, 0) { }

        // Конструктор с requestID, userID, roleID
        public RequestDetailsForm(int requestID, int userID, int roleID)
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;
            LoadClients();
            LoadFaultTypes();
            LoadStatuses();
            this.requestID = requestID;
            if (requestID > 0)
            {
                LoadRequestData();
            }
        }

        private void LoadClients()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ClientID, Name FROM Clients";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbClient.Items.Add(new ClientItem
                            {
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
                }
            }
        }

        private void LoadFaultTypes()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT FaultTypeID, TypeName FROM FaultTypes";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbFaultType.Items.Add(new FaultTypeItem
                            {
                                FaultTypeID = Convert.ToInt32(reader["FaultTypeID"]),
                                TypeName = reader["TypeName"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки типов неисправностей: " + ex.Message);
                }
            }
        }

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
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbStatus.Items.Add(new StatusItem
                            {
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                StatusName = reader["StatusName"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки статусов: " + ex.Message);
                }
            }
        }

        private void LoadRequestData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Requests WHERE RequestID = @requestID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@requestID", requestID);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtEquipment.Text = reader["Equipment"].ToString();
                            txtDescription.Text = reader["Description"].ToString();
                            cmbClient.SelectedItem = GetClientItem(Convert.ToInt32(reader["ClientID"]));
                            cmbFaultType.SelectedItem = GetFaultTypeItem(Convert.ToInt32(reader["FaultTypeID"]));
                            cmbStatus.SelectedItem = GetStatusItem(Convert.ToInt32(reader["StatusID"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных заявки: " + ex.Message);
                }
            }
        }

        private object GetClientItem(int clientID)
        {
            foreach (var item in cmbClient.Items)
            {
                if (((ClientItem)item).ClientID == clientID)
                    return item;
            }
            return null;
        }

        private object GetFaultTypeItem(int faultTypeID)
        {
            foreach (var item in cmbFaultType.Items)
            {
                if (((FaultTypeItem)item).FaultTypeID == faultTypeID)
                    return item;
            }
            return null;
        }

        private object GetStatusItem(int statusID)
        {
            foreach (var item in cmbStatus.Items)
            {
                if (((StatusItem)item).StatusID == statusID)
                    return item;
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbClient.SelectedItem == null || cmbFaultType.SelectedItem == null || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            int clientID = ((ClientItem)cmbClient.SelectedItem).ClientID;
            int faultTypeID = ((FaultTypeItem)cmbFaultType.SelectedItem).FaultTypeID;
            int statusID = ((StatusItem)cmbStatus.SelectedItem).StatusID;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (requestID.HasValue && requestID > 0)
                    {
                        string query = "UPDATE Requests SET Equipment = @equipment, Description = @description, " +
                                       "ClientID = @clientID, FaultTypeID = @faultTypeID, StatusID = @statusID " +
                                       "WHERE RequestID = @requestID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@equipment", txtEquipment.Text);
                            cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                            cmd.Parameters.AddWithValue("@clientID", clientID);
                            cmd.Parameters.AddWithValue("@faultTypeID", faultTypeID);
                            cmd.Parameters.AddWithValue("@statusID", statusID);
                            cmd.Parameters.AddWithValue("@requestID", requestID.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string query = "INSERT INTO Requests (Equipment, Description, ClientID, FaultTypeID, StatusID) " +
                                       "VALUES (@equipment, @description, @clientID, @faultTypeID, @statusID)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@equipment", txtEquipment.Text);
                            cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                            cmd.Parameters.AddWithValue("@clientID", clientID);
                            cmd.Parameters.AddWithValue("@faultTypeID", faultTypeID);
                            cmd.Parameters.AddWithValue("@statusID", statusID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Заявка сохранена!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения заявки: " + ex.Message);
                }
            }
        }

        private class ClientItem
        {
            public int ClientID { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }

        private class FaultTypeItem
        {
            public int FaultTypeID { get; set; }
            public string TypeName { get; set; }
            public override string ToString() => TypeName;
        }

        private class StatusItem
        {
            public int StatusID { get; set; }
            public string StatusName { get; set; }
            public override string ToString() => StatusName;
        }
    }
}