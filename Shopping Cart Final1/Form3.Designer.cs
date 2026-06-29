namespace Shopping_Cart_Final1
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lnkbackToLogin = new Label();
            btnCreateAccount = new Button();
            txtContact = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            lblContact = new Label();
            lblEmail = new Label();
            lblPassword = new Label();
            lblUsername = new Label();
            label1 = new Label();
            txtUsername = new TextBox();
            pictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // lnkbackToLogin
            // 
            lnkbackToLogin.AutoSize = true;
            lnkbackToLogin.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lnkbackToLogin.Location = new Point(734, 576);
            lnkbackToLogin.Name = "lnkbackToLogin";
            lnkbackToLogin.Size = new Size(115, 23);
            lnkbackToLogin.TabIndex = 35;
            lnkbackToLogin.Text = "Back To Login";
            lnkbackToLogin.Click += lnkbackToLogin_Click;
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.BackColor = Color.FromArgb(192, 0, 192);
            btnCreateAccount.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnCreateAccount.ForeColor = SystemColors.ButtonHighlight;
            btnCreateAccount.Location = new Point(693, 519);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(194, 41);
            btnCreateAccount.TabIndex = 34;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.UseVisualStyleBackColor = false;
            btnCreateAccount.Click += btnCreateAccount_Click;
            // 
            // txtContact
            // 
            txtContact.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            txtContact.Location = new Point(523, 467);
            txtContact.Multiline = true;
            txtContact.Name = "txtContact";
            txtContact.Size = new Size(517, 34);
            txtContact.TabIndex = 33;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            txtEmail.Location = new Point(523, 384);
            txtEmail.Multiline = true;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(517, 34);
            txtEmail.TabIndex = 32;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            txtPassword.Location = new Point(523, 306);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(517, 34);
            txtPassword.TabIndex = 31;
            // 
            // lblContact
            // 
            lblContact.AutoSize = true;
            lblContact.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lblContact.Location = new Point(523, 432);
            lblContact.Name = "lblContact";
            lblContact.Size = new Size(70, 23);
            lblContact.TabIndex = 30;
            lblContact.Text = "Contact";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lblEmail.Location = new Point(523, 352);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(51, 23);
            lblEmail.TabIndex = 29;
            lblEmail.Text = "Email";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lblPassword.Location = new Point(523, 271);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(82, 23);
            lblPassword.TabIndex = 28;
            lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lblUsername.Location = new Point(523, 181);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(87, 23);
            lblUsername.TabIndex = 27;
            lblUsername.Text = "Username";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(712, 106);
            label1.Name = "label1";
            label1.Size = new Size(159, 31);
            label1.TabIndex = 26;
            label1.Text = "Register Form";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            txtUsername.Location = new Point(523, 215);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(517, 34);
            txtUsername.TabIndex = 25;
            // 
            // pictureBox
            // 
            pictureBox.Image = Properties.Resources.download;
            pictureBox.Location = new Point(734, 28);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(125, 75);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.TabIndex = 24;
            pictureBox.TabStop = false;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1812, 763);
            Controls.Add(lnkbackToLogin);
            Controls.Add(btnCreateAccount);
            Controls.Add(txtContact);
            Controls.Add(txtEmail);
            Controls.Add(txtPassword);
            Controls.Add(lblContact);
            Controls.Add(lblEmail);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(label1);
            Controls.Add(txtUsername);
            Controls.Add(pictureBox);
            Name = "Form3";
            Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lnkbackToLogin;
        private Button btnCreateAccount;
        private TextBox txtContact;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Label lblContact;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblUsername;
        private Label label1;
        private TextBox txtUsername;
        private PictureBox pictureBox;
    }
}