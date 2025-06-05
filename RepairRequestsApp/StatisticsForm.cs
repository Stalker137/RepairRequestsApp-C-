using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepairRequestsApp
{
    public partial class StatisticsForm : Form
    {
        private string connectionString = @"Data Source=ARTEM\SQLEXPRESS;Initial Catalog=Repair;Integrated Security=True;Encrypt=False";
        private int userID;
        private int roleID;

        public StatisticsForm(int userID, int roleID)
        {
            InitializeComponent();
            this.userID = userID;
            this.roleID = roleID;
            LoadStatistics();
        }

        public StatisticsForm() : this(0, 0) { }

        private void LoadStatistics()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Выполните статистику здесь
                    // Пример: количество выполненных заявок
                    string query1 = "SELECT COUNT(*) FROM Requests WHERE StatusID = 3";
                    using (SqlCommand cmd = new SqlCommand(query1, conn))
                    {
                        int completed = Convert.ToInt32(cmd.ExecuteScalar());
                        lblCompleted.Text = $"Выполнено заявок: {completed}";
                    }

                    // Пример: среднее время выполнения
                    string query2 = "SELECT AVG(DATEDIFF(DAY, DateAdded, GETDATE())) FROM Requests WHERE StatusID = 3";
                    using (SqlCommand cmd = new SqlCommand(query2, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            double avgDays = Convert.ToDouble(result);
                            lblAvgTime.Text = $"Среднее время выполнения: {avgDays:F1} дней";
                        }
                    }

                    // Пример: статистика по типам неисправностей
                    string query3 = "SELECT ft.TypeName AS [Тип_неисправности], COUNT(r.RequestID) AS [Количество_заявок] FROM Requests r JOIN FaultTypes ft ON r.FaultTypeID = ft.FaultTypeID GROUP BY ft.TypeName";
                    using (SqlCommand cmd = new SqlCommand(query3, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgvFaultStats.DataSource = table;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки статистики: " + ex.Message);
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