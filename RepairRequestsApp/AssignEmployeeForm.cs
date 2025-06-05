using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class AssignEmployeeForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int requestID;

        public AssignEmployeeForm(int requestID)
        {
            InitializeComponent();
            this.requestID = requestID;
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT e.EmployeeID, e.Name FROM Employees e JOIN Users u ON e.UserID = u.UserID WHERE u.RoleID = 3";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbEmployee.Items.Add(new EmployeeItem
                            {
                                EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки исполнителей: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbEmployee.SelectedItem == null)
            {
                MessageBox.Show("Выберите исполнителя!");
                return;
            }

            int employeeID = ((EmployeeItem)cmbEmployee.SelectedItem).EmployeeID;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Requests SET EmployeeID = @employeeID WHERE RequestID = @requestID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@employeeID", employeeID);
                        cmd.Parameters.AddWithValue("@requestID", requestID);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Исполнитель назначен!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка назначения исполнителя: " + ex.Message);
                }
            }
        }

        private class EmployeeItem
        {
            public int EmployeeID { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
    }
}