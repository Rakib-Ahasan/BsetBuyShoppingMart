using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace ShoppingMartApplicationInWinForm
{
    public partial class DetailsAndSearchForm : Form
    {
        private string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public DetailsAndSearchForm()
        {
            InitializeComponent();
            BindGridView();
        }

        void BindGridView()
        {
            SqlConnection con=new SqlConnection(cs);
            string query = "sp_getBothTablesData";
            SqlCommand cmd=new SqlCommand(query,con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda=new SqlDataAdapter();
            sda.SelectCommand = cmd;
            DataTable data=new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.Columns[10].Visible = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "sp_getBothTablesDataByInvoice";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@invoiceId", searchByInvoiceTextBox.Text);
            SqlDataAdapter sda = new SqlDataAdapter();
           // sda.SelectCommand.Parameters.AddWithValue("@invoiceId",searchByInvoiceTextBox.Text);
            sda.SelectCommand = cmd;
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;
           
            finalCostTextBox.Text=dataGridView1.Rows[0].Cells[10].Value.ToString();
        }

        private void searchDateTimeButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "sp_getBothTablesDataByDateTime";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@firstDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@secondDate", dateTimePicker2.Value);
            SqlDataAdapter sda = new SqlDataAdapter();
            // sda.SelectCommand.Parameters.AddWithValue("@invoiceId",searchByInvoiceTextBox.Text);
            sda.SelectCommand = cmd;
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;

            //finalCostTextBox.Text = dataGridView1.Rows[0].Cells[10].Value.ToString();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            BindGridView();
        }
    }
}
