using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTask
{
    public partial class Form1 : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

        List<string> columnNames = new List<string>();

        bool changeColor = true;

        public Form1()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (changeColor)
            {
                if (dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor == Color.Green)
                    dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = Color.White;
                else
                    dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = Color.Green;
            }
        }

        void FillDataGrid()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM TestTable", sqlConnection);

                DataSet dataSet = new DataSet();

                DataTable dataTable = new DataTable();
                sqlData.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        void ResetColumnsColor()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.BackColor = Color.White;
            }
        }
               
        private void ResetButton_Click(object sender, EventArgs e)
        {
            changeColor = true;
            
            ResetColumnsColor();

            dataGridView1.DataSource = null;

            FillDataGrid();
        }

        private void GroupButton_Click(object sender, EventArgs e)
        {
            changeColor = false;

            columnNames.Clear();

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.DefaultCellStyle.BackColor == Color.Green)
                {
                    columnNames.Add(column.Name);
                }
            }            

            ResetColumnsColor();

            string words = string.Join(" ,", columnNames);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter($"SELECT {words} , Sum([Количество]) as Количество,  Sum([Сумма]) as Сумма FROM TestTable GROUP BY {words}", sqlConnection);

                DataSet dataSet = new DataSet();

                DataTable dataTable = new DataTable();
                sqlData.Fill(dataTable);
                
                dataGridView1.DataSource = dataTable;
            }
        }               
    }
}
