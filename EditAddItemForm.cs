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
    public partial class EditAddItemForm : Form
    {

        private string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public EditAddItemForm()
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

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            idTextBox.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            nameTextBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            priceTExtBox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            discountTextBox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "update items_tbl set item_name=@name,unit_price=@price,item_discount=@discount where item_id=@id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idTextBox.Text);
            cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
            cmd.Parameters.AddWithValue("@price", priceTExtBox.Text);
            cmd.Parameters.AddWithValue("@discount", discountTextBox.Text);


            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Item name is :" + " " + nameTextBox.Text + "\n \n" + "Price is :" + " " + priceTExtBox.Text + "\n \n" + "Discount is :" + " " + discountTextBox.Text);
                BindGridView();
                idTextBox.Clear();
                nameTextBox.Clear();
                priceTExtBox.Clear();
                discountTextBox.Clear();
                nameTextBox.Focus();

            }
            else
            {
                MessageBox.Show("Item updated faild.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void deletetButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "delete from items_tbl where item_id=@id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idTextBox.Text);
            
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // MessageBox.Show("Item name is :" + " " + nameTextBox.Text + "\n \n" + "Price is :" + " " + priceTExtBox.Text + "\n \n" + "Discount is :" + " " + discountTextBox.Text);
                BindGridView();
                idTextBox.Clear();
                nameTextBox.Clear();
                priceTExtBox.Clear();
                discountTextBox.Clear();
                nameTextBox.Focus();

            }
            else
            {
                MessageBox.Show("Item deleted faild.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

       

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) == true)
            {
                errorProvider1.SetError(this.nameTextBox, "Plase give item name");
                nameTextBox.Focus();
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void priceTExtBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(priceTExtBox.Text) == true)
            {
                errorProvider2.SetError(this.priceTExtBox, "Plase give item price");
                priceTExtBox.Focus();
            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void discountTextBox_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(discountTextBox.Text) == true)
            {
                errorProvider3.SetError(this.discountTextBox, "Plase give item discount");
                discountTextBox.Focus();
            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void priceTExtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsDigit(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void discountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsDigit(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }
    }
}
