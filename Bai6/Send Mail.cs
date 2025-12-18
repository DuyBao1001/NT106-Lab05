using System;
using System.Windows.Forms;
using MimeKit;
using MailKit.Security;
using System.IO;

namespace Bai6
{
    public partial class Form2 : Form
    {
        private string _smtpHost;
        private int _smtpPort;
        private string _userEmail;
        private string _appPassword;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string email, string password, string host, int port)
        {
            InitializeComponent();
            _userEmail = email;
            _appPassword = password;
            _smtpHost = host;
            _smtpPort = port;

            textBox1.Text = email;
            textBox1.ReadOnly = true;

            this.button1.Click += new System.EventHandler(this.btnBrowse_Click);
            this.button2.Click += new System.EventHandler(this.btnSend_Click);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = dialog.FileName;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Vui lòng nhập Người nhận và Tiêu đề.", "Thiếu thông tin");
                return;
            }

            try
            {
                var message = new MimeMessage();
                string name = string.IsNullOrEmpty(textBox2.Text) ? _userEmail : textBox2.Text;
                message.From.Add(new MailboxAddress(name, _userEmail));

                var recipients = textBox3.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var recipient in recipients)
                {
                    message.To.Add(MailboxAddress.Parse(recipient.Trim()));
                }

                message.Subject = textBox4.Text;

                var builder = new BodyBuilder();
                if (checkBox1.Checked) builder.HtmlBody = richTextBox1.Text;
                else builder.TextBody = richTextBox1.Text;

                if (!string.IsNullOrEmpty(textBox5.Text) && File.Exists(textBox5.Text))
                {
                    builder.Attachments.Add(textBox5.Text);
                }

                message.Body = builder.ToMessageBody();

                // Gọi đích danh SmtpClient của MailKit
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_smtpHost, _smtpPort, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(_userEmail, _appPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

                MessageBox.Show("Gửi mail thành công!", "Thông báo");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private void Form2_Load(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
    }
}