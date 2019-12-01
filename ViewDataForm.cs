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
    public partial class ViewDataForm : Form
    {
        private string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public ViewDataForm()
        {
            InitializeComponent();
            BindGridView();
        }
        void BindGridView()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select * from items_tbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            AddItemForm adf=new AddItemForm();
            adf.ShowDialog();
        }

        private void updateItemButton_Click(object sender, EventArgs e)
        {
            EditAddItemForm edf=new EditAddItemForm();
            edf.ShowDialog();
        }

        private void deleteItemButton_Click(object sender, EventArgs e)
        {
            EditAddItemForm edf = new EditAddItemForm();
            edf.ShowDialog();
        }

        private void ViewDataForm_Activated(object sender, EventArgs e)
        {
            BindGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
