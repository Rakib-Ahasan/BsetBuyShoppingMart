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
using System.Text.RegularExpressions;

namespace ShoppingMartApplicationInWinForm
{
    public partial class SignUp : Form
    {
        string passPattern = @"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";
        private string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        private string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public SignUp()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void signUpButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "insert into SignUp values(@name,@surname,@gender,@age,@address,@email,@pass)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
            cmd.Parameters.AddWithValue("@surname", surnameTextBox.Text);
            cmd.Parameters.AddWithValue("@gender", genderComboBox.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@age", ageNumericUpDown.Value);
            cmd.Parameters.AddWithValue("@address", addressTextBox.Text);
            cmd.Parameters.AddWithValue("@email", emailTextBox.Text);
            cmd.Parameters.AddWithValue("@pass", passwordTextBox.Text);
           
            con.Open();
            int a=cmd.ExecuteNonQuery();
            if (a>0)
            {
                MessageBox.Show("SignUp successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Username is :"+nameTextBox.Text + "\n \n"+ "Password is :" + passwordTextBox.Text);
                this.Hide();
                Login login=new Login();
                login.ShowDialog();
            }
            else
            {
                MessageBox.Show("SignUp faild.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text)==true)
            {
                errorProvider1.SetError(this.nameTextBox,"Please enter your name.");
                nameTextBox.Focus();
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(ch)==true)
            {
                e.Handled = false;
            }
            else if(ch==8||ch==32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(surnameTextBox.Text) == true)
            {
                errorProvider2.SetError(this.surnameTextBox, "Please enter your surname.");
                surnameTextBox.Focus();
            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void surnameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsLetter(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8 || ch == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void genderComboBox_Leave(object sender, EventArgs e)
        {
            if (genderComboBox.SelectedItem==null)
            {errorProvider3.SetError(this.genderComboBox,"Please select your gender.");
                genderComboBox.Focus();

            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void ageNumericUpDown_Leave(object sender, EventArgs e)
        {
            if (ageNumericUpDown.Value==0)
            {
                errorProvider4.SetError(this.ageNumericUpDown,"Please set your age.");
                ageNumericUpDown.Focus();
            }
            else
            {
                errorProvider4.Clear();
            }
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(addressTextBox.Text) == true)
            {
                errorProvider5.SetError(this.addressTextBox, "Please enter your address.");
                addressTextBox.Focus();
            }
            else
            {
                errorProvider5.Clear();
            }
        }

        private void addressTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            
            char ch = e.KeyChar;
            if (char.IsLetter(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8 || ch == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(emailTextBox.Text,pattern)==false)
            {
                errorProvider6.SetError(this.emailTextBox,"Please enter email correct format.");
                emailTextBox.Focus();
            }
            else
            {
                errorProvider6.Clear();
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(passwordTextBox.Text, passPattern) == false)
            {
                errorProvider7.SetError(this.emailTextBox, "Please enter UpperCase,LowerCase,Numbers,Symbols.");
                emailTextBox.Focus();
            }
            else
            {
                errorProvider7.Clear();
            }
        }

        private void confirmPasswordTextBox_Leave(object sender, EventArgs e)
        {
            if (confirmPasswordTextBox.Text!=passwordTextBox.Text)
            {
                errorProvider8.SetError(this.confirmPasswordTextBox,"Your password don't match.");
                confirmPasswordTextBox.Focus();

            }
            else
            {
                errorProvider8.Clear();
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            nameTextBox.Clear();
            surnameTextBox.Clear();
            genderComboBox.SelectedItem = null;
            ageNumericUpDown.Value = 0;
            addressTextBox.Clear();
            emailTextBox.Clear();
            passwordTextBox.Clear();
            confirmPasswordTextBox.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login lg=new Login();
            lg.ShowDialog();
        }
    }
}
