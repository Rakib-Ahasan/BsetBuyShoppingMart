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
    public partial class Login : Form
    {
        public static string username = "";
        private string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Login()
        {
            InitializeComponent();
            LoadCredential();
        }
        void SaveCredential()
        {
            if (checkBox1.Checked == true)
            {
                Properties.Settings.Default.Username = userNameTextBox.Text;
                Properties.Settings.Default.Password = passwordTextBox.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Save();
            }
        }

        void LoadCredential()
        {
            if (Properties.Settings.Default.Username != string.Empty)
            {
                userNameTextBox.Text = Properties.Settings.Default.Username;
                passwordTextBox.Text = Properties.Settings.Default.Password;
                checkBox1.Checked = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con= new SqlConnection(cs);
            string query = "select * from signUp where name=@user and password=@pass";
            SqlCommand cmd= new SqlCommand(query,con);
            
            cmd.Parameters.AddWithValue("@user", userNameTextBox.Text);
            cmd.Parameters.AddWithValue("@pass", passwordTextBox.Text);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows==true)
            {
                MessageBox.Show("Login successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SaveCredential();
                username = userNameTextBox.Text;
                this.Hide();
                Form1 mainForm=new Form1();
                mainForm.ShowDialog();

            }
            else
            {
                MessageBox.Show("Login faild", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool check = checkBox1.Checked;
            switch (check)
            {
                case true:
                    passwordTextBox.UseSystemPasswordChar = false;
                    break;
                default:
                    passwordTextBox.UseSystemPasswordChar = true;
                    break;

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignUp su=new SignUp();
            this.Hide();
            su.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
