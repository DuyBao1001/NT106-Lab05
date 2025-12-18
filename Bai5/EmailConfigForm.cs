using System;
using System.Windows.Forms;

namespace Bai5
{
    public partial class EmailConfigForm : Form
    {
        public EmailConfig Config { get; private set; }

        public EmailConfigForm(EmailConfig currentConfig)
        {
            InitializeComponent();
            this.Config = currentConfig;

            // Khởi tạo các trường với cấu hình hiện tại (nếu có)
            txtImapHost.Text = currentConfig.ImapHost;
            txtImapPort.Text = currentConfig.ImapPort.ToString();
            txtEmail.Text = currentConfig.Email;
            txtAppPassword.Text = currentConfig.AppPassword;

            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAppPassword.Text) ||
                !int.TryParse(txtImapPort.Text, out int port))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email và App Password (sử dụng App Password 16 ký tự).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật cấu hình
            Config.ImapHost = txtImapHost.Text;
            Config.ImapPort = port;
            Config.Email = txtEmail.Text.Trim();
            Config.AppPassword = txtAppPassword.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}