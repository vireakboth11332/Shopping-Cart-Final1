using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Shopping_Cart_Final1
{
    public partial class Form2 : Form
    {
        private int _loginAttempts = 0; // រាប់ចំនួនដងចូលខុស
        private const int MaxAttempts = 5;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // ដំណើរការ Animation GIF ដោយស្វ័យប្រវត្តិ
            // picAnimation.Image = Image.FromFile("path/to/animation.gif");

            // ដាក់ Enter key ភ្ជាប់ btnLogin
            this.AcceptButton = btnLogin;

            // ដាក់ Password character mask
            txtPassword.PasswordChar = '●';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
               string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("សូមបញ្ចូល Username និង Password!",
                    "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Block បន្ទាប់ពីចូលខុសច្រើនដង
            if (_loginAttempts >= MaxAttempts)
            {
                MessageBox.Show("គណនីត្រូវបានផ្អាក! សូមទំនាក់ទំនង Admin។",
                    "ហាមឃាត់", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = txtUsername.Text.Trim();
            string passwordHash = HashPassword(txtPassword.Text.Trim());

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT UserID, Username, Role 
                    FROM Users 
                    WHERE Username = @username AND Password = @password AND IsActive = 1", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", passwordHash);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Login ជោគជ័យ
                        _loginAttempts = 0;

                        // រក្សាទុកព័ត៌មាន User ក្នុង Session (Static class)
                        SessionHelper.CurrentUser = reader["Username"].ToString();
                        SessionHelper.CurrentRole = reader["Role"].ToString();
                        SessionHelper.UserID = Convert.ToInt32(reader["UserID"]);

                        // Update Last Login Date
                        reader.Close();
                        new SqlCommand($"UPDATE Users SET LastLogin=@date WHERE Username=@u", conn)
                        {
                            Parameters =
                            {
                                new SqlParameter("@date", DateTime.Now),
                                new SqlParameter("@u",    username)
                            }
                        }.ExecuteNonQuery();

                        // បើក Form1 (Main)
                        Form1 mainForm = new Form1();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        _loginAttempts++;
                        int remaining = MaxAttempts - _loginAttempts;
                        MessageBox.Show($"Username ឬ Password មិនត្រឹមត្រូវ!\nនៅសល់ {remaining} ដងទៀត។",
                            "Login បរាជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            }
        }

        private void lnkCreateAccount_Click(object sender, EventArgs e)
        {
            Form3 registerForm = new Form3();
            registerForm.ShowDialog(); // ShowDialog ដើម្បីឲ្យ Form3 ហ្វ្រោសសលើ Form2
        }

        // -----------------------------------------------
        // Link Forgot Password
        // -----------------------------------------------
        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string email = Microsoft.VisualBasic.Interaction.InputBox(
                "សូមបញ្ចូល Email ដែលបានចុះឈ្មោះ:", "Forgot Password");

            if (string.IsNullOrWhiteSpace(email)) return;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Username FROM Users WHERE Email = @email", conn);
                cmd.Parameters.AddWithValue("@email", email.Trim());

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show($"Username របស់អ្នកគឺ: [{result}]\nសូមទំនាក់ទំនង Admin ដើម្បីកំណត់ Password ថ្មី។",
                        "Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("រកមិនឃើញ Email នេះក្នុងប្រព័ន្ធ!",
                        "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
