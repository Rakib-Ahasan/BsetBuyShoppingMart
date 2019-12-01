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
using System.Diagnostics.Eventing.Reader;

namespace ShoppingMartApplicationInWinForm
{
    public partial class Form1 : Form
    {

        private int finalCoast = 0;
        private int slNo = 0;
        private int tax = 0;
        private int discount = 0;
        private int price = 0;
        private string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            GetItems();
            BindGridView();
            GetInvoiceId();
            userTextBox.Text = Login.username;

        }

        void BindGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].Name = "SL NO";
            dataGridView1.Columns[1].Name = "ITEM NAME";
            dataGridView1.Columns[2].Name = "UNIT PRICE";
            dataGridView1.Columns[3].Name = "DISCOUNT PER ITEM";
            dataGridView1.Columns[4].Name = "QUANTITY";
            dataGridView1.Columns[5].Name = "SUB TOTAL";
            dataGridView1.Columns[6].Name = "TAX";
            dataGridView1.Columns[7].Name = "TOTAL COAST";
        }

        void GetItems()
        {
            selectItemComboBox.Items.Clear();
            SqlConnection con = new SqlConnection(cs);
            string query = "select * from items_tbl ";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item_name = dr.GetString(1);
                selectItemComboBox.Items.Add(item_name);
            }

            selectItemComboBox.Sorted = true;
            con.Close();
        }

        void GetPrice()
        {
            if (selectItemComboBox.SelectedItem == null)
            {

            }
            else
            {
                SqlConnection con = new SqlConnection(cs);
                string query = "select unit_price from items_tbl where item_name=@name";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@name", selectItemComboBox.SelectedItem.ToString());
                DataTable data = new DataTable();
                sda.Fill(data);
                if (data.Rows.Count > 0)
                {
                    price = Convert.ToInt32(data.Rows[0]["unit_price"]);
                }

                unitPriceTextBox.Text = price.ToString();
            }
        }
        void GetDiscount()
        {

            if (selectItemComboBox.SelectedItem == null)
            {

            }
            else
            {
                SqlConnection con = new SqlConnection(cs);
                string query = "select item_discount from items_tbl where item_name=@name";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@name", selectItemComboBox.SelectedItem.ToString());
                DataTable data = new DataTable();
                sda.Fill(data);
                if (data.Rows.Count > 0)
                {
                    discount = Convert.ToInt32(data.Rows[0]["item_discount"]);
                }

                discountPerItemTextBox.Text = discount.ToString();
            }

        }
        private void selectItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPrice();
            GetDiscount();
            quantityTextBox.Enabled = true;
        }

        private void quantityTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(quantityTextBox.Text) == true)
            {

            }
            else
            {
                int price = Convert.ToInt32(unitPriceTextBox.Text);
                int discount = Convert.ToInt32(discountPerItemTextBox.Text);
                int quantity = Convert.ToInt32(quantityTextBox.Text);

                int subTotal = price * quantity;
                subTotal = subTotal - discount * quantity;
                subTotalTextBox.Text = subTotal.ToString();
            }
        }

        private void subTotalTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(subTotalTextBox.Text) == true)
            {

            }
            else
            {
                int subTotal = Convert.ToInt32(subTotalTextBox.Text);
                if (subTotal >= 10000)
                {
                    tax = (int)(subTotal * 0.15);
                    taxTextBox.Text = tax.ToString();
                }
                else if (subTotal >= 6000)
                {
                    tax = (int)(subTotal * 0.10);
                    taxTextBox.Text = tax.ToString();
                }
                else if (subTotal >= 3000)
                {
                    tax = (int)(subTotal * 0.07);
                    taxTextBox.Text = tax.ToString();
                }
                else if (subTotal >= 1000)
                {
                    tax = (int)(subTotal * 0.05);
                    taxTextBox.Text = tax.ToString();
                }
                else
                {
                    tax = (int)(subTotal * 0.03);
                    taxTextBox.Text = tax.ToString();
                }
            }
        }

        private void taxTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(taxTextBox.Text) == true)
            {

            }
            else
            {
                int subTotal = Convert.ToInt32(subTotalTextBox.Text);
                int tax = Convert.ToInt32(taxTextBox.Text);
                int totalCoast = subTotal + tax;
                totalCoastTextBox.Text = totalCoast.ToString();
            }

        }

        void AddDataToGridView(string sl_No, string item_name, string unit_price, string discount, string quantity, string sub_total, string tax, string tota_coast)
        {
            string[] row = { sl_No, item_name, unit_price, discount, quantity, sub_total, tax, tota_coast };
            dataGridView1.Rows.Add(row);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (selectItemComboBox.SelectedItem != null && string.IsNullOrEmpty(quantityTextBox.Text)==false)
            {
                AddDataToGridView((++slNo).ToString(), selectItemComboBox.SelectedItem.ToString(), unitPriceTextBox.Text, discountPerItemTextBox.Text, quantityTextBox.Text, subTotalTextBox.Text, taxTextBox.Text, totalCoastTextBox.Text);
                ResetControls();
                CalculateFinalCoast();

            }
            else
            {
                MessageBox.Show("Please select an item and give quantity.","Faild",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetControls();

        }

        void ResetControls()
        {
            selectItemComboBox.SelectedItem = null;
            unitPriceTextBox.Clear();
            discountPerItemTextBox.Clear();
            quantityTextBox.Clear();
            subTotalTextBox.Clear();
            taxTextBox.Clear();
            totalCoastTextBox.Clear();
            finalCoastTextBox.Clear();
            amountPaidTextBox.Clear();
            ChangeTextBox.Clear();
            quantityTextBox.Enabled = false;
        }

        void CalculateFinalCoast()
        {
            finalCoast = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                finalCoast = finalCoast + Convert.ToInt32(dataGridView1.Rows[i].Cells[7].Value);

            }

            finalCoastTextBox.Text = finalCoast.ToString();
        }

        private void amountPaidTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(amountPaidTextBox.Text) == true)
            {

            }
            else
            {
                int amountPaid = Convert.ToInt32(amountPaidTextBox.Text);
                int fcoast = Convert.ToInt32(finalCoastTextBox.Text);
                int change = amountPaid - fcoast;
                ChangeTextBox.Text = change.ToString();

            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            slNo = 0;
        }

        void GetInvoiceId()
        {
            SqlConnection con = new SqlConnection(cs);
            string query1 = "select invoice_id from OrderMaster ";
            SqlDataAdapter sda = new SqlDataAdapter(query1, con);
            DataTable data = new DataTable();
            sda.Fill(data);
            if (data.Rows.Count < 1)
            {
                invoiceNoTextBox.Text = "1";
            }
            else
            {
                string query2 = "select max(invoice_id) from OrderMaster ";
                SqlCommand cmd = new SqlCommand(query2, con);
                con.Open();
                int a = Convert.ToInt32(cmd.ExecuteScalar());
                a = a + 1;
                invoiceNoTextBox.Text = a.ToString();
                con.Close();
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (selectItemComboBox.SelectedItem != null && string.IsNullOrEmpty(quantityTextBox.Text)==false)
            {
                SqlConnection con = new SqlConnection(cs);
                string query1 = "insert into OrderMaster values(@id,@name,@datetime,@finalcost) ";
                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.Parameters.AddWithValue("@id", invoiceNoTextBox.Text);
                cmd.Parameters.AddWithValue("@name", userTextBox.Text);
                cmd.Parameters.AddWithValue("@datetime", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@finalcost", finalCoastTextBox.Text);
                con.Open();
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Inserted successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetInvoiceId();
                    ResetControls();
                    InsertIntoOrderDetails();
                }
                else
                {
                    MessageBox.Show("Insertion faild ", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            else
            {
                MessageBox.Show("Please select an item and give quantity.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        int GetLastInsertedInvoiceId()
        {
            SqlConnection con =new SqlConnection(cs);
            string query = "select max(invoice_id) from ordermaster";
            SqlCommand cmd=new SqlCommand(query,con);
            con.Open();
            int maxInvoiceId=Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            return maxInvoiceId;

        }

        void InsertIntoOrderDetails()
        {
            int a = 0;
            SqlConnection con = new SqlConnection(cs);
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    string query = "insert into orderDetails values(@invoice_id,@name,@price,@discount,@quantity,@subtotal,@tax,@finalcost)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@invoice_id", GetLastInsertedInvoiceId());
                    cmd.Parameters.AddWithValue("@name", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@price", dataGridView1.Rows[i].Cells[2].Value);
                    cmd.Parameters.AddWithValue("@discount", dataGridView1.Rows[i].Cells[3].Value);
                    cmd.Parameters.AddWithValue("@quantity", dataGridView1.Rows[i].Cells[4].Value);
                    cmd.Parameters.AddWithValue("@subtotal", dataGridView1.Rows[i].Cells[5].Value);
                    cmd.Parameters.AddWithValue("@tax", dataGridView1.Rows[i].Cells[6].Value);
                    cmd.Parameters.AddWithValue("@finalcost", dataGridView1.Rows[i].Cells[7].Value);
                    con.Open();
                    a = a + cmd.ExecuteNonQuery();
                    con.Close();
                }

                if (a > 0)
                {
                    MessageBox.Show("Data added in OrderDetails table");
                }
                else
                {
                    MessageBox.Show("Data not  added in OrderDetails table");
                }
            }
            catch 
            {
                

            }
         
          
        }

        private void quantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
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

        private void amountPaidTextBox_KeyPress(object sender, KeyPressEventArgs e)
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

        private void printPreviewButton_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = Properties.Resources._68272839_490970644815698_5833767087889186816_n;
            Image img = bmp;
            e.Graphics.DrawImage(img, 30, 5, 800, 250);
            e.Graphics.DrawString("Invoice Id :" + " " + invoiceNoTextBox.Text, new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 350));
            e.Graphics.DrawString("User Name :" + " " + userTextBox.Text, new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 380));
            e.Graphics.DrawString("Date :" + " " + DateTime.Now.ToShortDateString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 420));
            e.Graphics.DrawString("Time :" + " " + DateTime.Now.ToLongTimeString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 450));
            e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------------------------", new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 480));
            e.Graphics.DrawString("Item", new Font("Kalpurush", 16, FontStyle.Bold), Brushes.Black, new Point(30, 520));
            e.Graphics.DrawString("Price", new Font("Kalpurush", 16, FontStyle.Bold), Brushes.Black, new Point(200, 520));
            e.Graphics.DrawString("Quantity", new Font("Kalpurush", 16, FontStyle.Bold), Brushes.Black, new Point(400, 520));
            e.Graphics.DrawString("Discount", new Font("Kalpurush", 16, FontStyle.Bold), Brushes.Black, new Point(600, 520));
            e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------------------------", new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 550));

            //Its for Item
            int gap = 580;
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, gap));
                        gap = gap + 30;
                    }
                    catch
                    {

                    }


                }
            }

            //Its for Price
            int gap1 = 580;
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(200, gap1));
                        gap1 = gap1 + 30;
                    }
                    catch
                    {

                    }


                }
            }

            //Its for Quantity
            int gap2 = 580;
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(400, gap2));
                        gap2 = gap2 + 30;
                    }
                    catch
                    {

                    }


                }
            }
            //Its for Discount
            int gap3 = 580;
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(600, gap3));
                        gap3 = gap3 + 30;
                    }
                    catch
                    {

                    }


                }
            }
            //Its for SubTotal.
            int subTotalPrint = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                subTotalPrint = subTotalPrint + Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);

            }

            e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------------------------", new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 900));

            e.Graphics.DrawString("Sub Total :" + " " + subTotalPrint.ToString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 930));

            //Its for TaxPrint.
            int taxPrint = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                taxPrint = taxPrint + Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);

            }

            //e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------------------------", new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 900));

            e.Graphics.DrawString("Tax :" + " " + taxPrint.ToString(), new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 960));
            e.Graphics.DrawString("Final Amount :" + " " + finalCoastTextBox.Text, new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 990));
            e.Graphics.DrawString("---------------------------------------------------------------------------------------------------------------------------", new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 1020));
            e.Graphics.DrawString("Amount Paid :" + " " + amountPaidTextBox.Text, new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 1050));
            e.Graphics.DrawString("Change :" + " " + ChangeTextBox.Text, new Font("Kalpurush", 12, FontStyle.Bold), Brushes.Black, new Point(30, 1080));

        }

        private void printButton_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemForm adf=new AddItemForm();
            adf.ShowDialog();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            GetItems();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAddItemForm edf=new EditAddItemForm();
            edf.ShowDialog();
        }

        private void vewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewDataForm vdf=new ViewDataForm();
            vdf.ShowDialog();
        }

        private void detailsAndSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetailsAndSearchForm das=new DetailsAndSearchForm();
            das.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

