namespace Shopping_Cart_Final1
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            panel1 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            lnkCreateAccount = new Label();
            lblPassword = new Label();
            lblUsername = new Label();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            picAnimation = new PictureBox();
            picLogo = new PictureBox();
            lnkForgetPassword = new Label();
            btnLogin = new Sunny.UI.UIButton();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAnimation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(255, 255, 128);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(lnkCreateAccount);
            panel1.Controls.Add(lblPassword);
            panel1.Controls.Add(lblUsername);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(picAnimation);
            panel1.Controls.Add(picLogo);
            panel1.Controls.Add(lnkForgetPassword);
            panel1.Controls.Add(btnLogin);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1784, 726);
            panel1.TabIndex = 18;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.images__3_;
            pictureBox2.Location = new Point(975, 367);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(39, 29);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 26;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.user_login_icon_14;
            pictureBox1.Location = new Point(975, 263);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(39, 29);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 25;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // lnkCreateAccount
            // 
            lnkCreateAccount.AutoSize = true;
            lnkCreateAccount.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lnkCreateAccount.Location = new Point(1410, 493);
            lnkCreateAccount.Name = "lnkCreateAccount";
            lnkCreateAccount.Size = new Size(150, 28);
            lnkCreateAccount.TabIndex = 24;
            lnkCreateAccount.Text = "Create Account";
            lnkCreateAccount.Click += lnkCreateAccount_Click;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblPassword.Location = new Point(1025, 328);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(97, 28);
            lblPassword.TabIndex = 23;
            lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblUsername.Location = new Point(1020, 224);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(104, 28);
            lblUsername.TabIndex = 22;
            lblUsername.Text = "Username";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(1020, 362);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(540, 34);
            txtPassword.TabIndex = 21;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            txtUsername.Location = new Point(1020, 263);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(540, 34);
            txtUsername.TabIndex = 18;
            // 
            // picAnimation
            // 
            picAnimation.Image = (Image)resources.GetObject("picAnimation.Image");
            picAnimation.Location = new Point(0, 3);
            picAnimation.Name = "picAnimation";
            picAnimation.Size = new Size(859, 720);
            picAnimation.SizeMode = PictureBoxSizeMode.StretchImage;
            picAnimation.TabIndex = 17;
            picAnimation.TabStop = false;
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.download__37_;
            picLogo.Location = new Point(1185, 34);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(168, 176);
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.TabIndex = 10;
            picLogo.TabStop = false;
            // 
            // lnkForgetPassword
            // 
            lnkForgetPassword.AutoSize = true;
            lnkForgetPassword.Font = new Font("Courier New", 8F);
            lnkForgetPassword.ForeColor = SystemColors.ActiveCaptionText;
            lnkForgetPassword.Location = new Point(1410, 399);
            lnkForgetPassword.Name = "lnkForgetPassword";
            lnkForgetPassword.Size = new Size(144, 17);
            lnkForgetPassword.TabIndex = 15;
            lnkForgetPassword.Text = "Forgot Password ?";
            // 
            // btnLogin
            // 
            btnLogin.FillColor = Color.Blue;
            btnLogin.Font = new Font("Microsoft Uighur", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.Location = new Point(1175, 427);
            btnLogin.Margin = new Padding(3, 4, 3, 4);
            btnLogin.MinimumSize = new Size(1, 1);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(200, 47);
            btnLogin.TabIndex = 16;
            btnLogin.Text = "&Login";
            btnLogin.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnLogin.Click += btnLogin_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1792, 750);
            Controls.Add(panel1);
            Name = "Form2";
            Text = "USER LOG IN";
            Load += Form2_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAnimation).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lnkCreateAccount;
        private Label lblPassword;
        private Label lblUsername;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private PictureBox picAnimation;
        private PictureBox picLogo;
        private Label lnkForgetPassword;
        private Sunny.UI.UIButton btnLogin;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}