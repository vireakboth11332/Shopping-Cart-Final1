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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            // ១. Validate — ត្រួតពិនិត្យ input ទទេ
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("សូមបំពេញព័ត៌មានឲ្យគ្រប់គ្រាន់!",
                    "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ២. Validate — Password យ៉ាងតិច 6 តួ
            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password ត្រូវមានយ៉ាងតិច 6 តួ!",
                    "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = HashPassword(txtPassword.Text.Trim()); // Hash password
            string email = txtEmail.Text.Trim();
            string contact = txtContact.Text.Trim();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // ៣. ពិនិត្យថា Username មានក្នុង DB រួចហើយទេ
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE Username = @username", conn);
                checkCmd.Parameters.AddWithValue("@username", username);
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    MessageBox.Show("Username នេះត្រូវបានប្រើប្រាស់រួចហើយ! សូមជ្រើសរើស Username ផ្សេង។",
                        "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ៤. INSERT User ថ្មី
                SqlCommand insertCmd = new SqlCommand(@"
                    INSERT INTO Users (Username, Password, Email, Contact, Role, CreatedDate)
                    VALUES (@username, @password, @email, @contact, 'Staff', @date)", conn);
                insertCmd.Parameters.AddWithValue("@username", username);
                insertCmd.Parameters.AddWithValue("@password", password);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@contact", contact);
                insertCmd.Parameters.AddWithValue("@date", DateTime.Now);
                insertCmd.ExecuteNonQuery();
            }

            MessageBox.Show("បង្កើតគណនីជោគជ័យ! សូមចូលប្រព័ន្ធ។",
                "ជោគជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // ៥. បិទ Form3 ហើយត្រឡប់ទៅ Form2 (Login)
            this.Close();
        }

        private void lnkbackToLogin_Click(object sender, EventArgs e)
        {
            this.Close(); // បិទ Form3 ត្រឡប់ Form2
        }

        // -----------------------------------------------
        // Click លើ PictureBox ដើម្បីជ្រើសរូបភាព Profile
        // -----------------------------------------------
        private void pictureBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "ជ្រើសរើសរូបភាព Profile";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(ofd.FileName);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
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
    }
    
}
