using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        string connectionString = @"Data Source=DESKTOP-59DMJ7P\SQLEXPRESS; Initial Catalog= testdb; Integrated Security=True;";

        // Collection that contains id of invisible columns.
        Stack<int> previousNumbers = new Stack<int>();

        public Form1()
        {
            InitializeComponent();
            FillDataGrid();
        }

        

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {   
            dataGridView1.Columns[e.ColumnIndex].Visible = false;
            previousNumbers.Push(e.ColumnIndex);
        }

        void FillDataGrid()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM TestTable", sqlConnection);
                DataTable dataTable = new DataTable();
                sqlData.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

       

        private void ResetButton_Click(object sender, EventArgs e)
        {
            FillDataGrid();

            // Make all columns visible
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Visible = true;
            }

        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (previousNumbers.Count != 0)
            {
                int num = previousNumbers.Pop();
                dataGridView1.Columns[num].Visible = true;
            }
        }
    }
}
