using System;
using System.Windows.Forms;
using MimeKit;
using MailKit.Net.Smtp;

namespace Bai1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string from = txtBoxFrom.Text.Trim();
                string to = txtBoxTo.Text.Trim();
                string subject = txtBoxSubject.Text;
                string body = rtBoxBody.Text;

                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                    return;
                }

                string password = "risa qxfc ngun rvsd";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from, from));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                MessageBox.Show("Email đã được gửi thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi gửi email: {ex.Message}", "Lỗi");
            }
        }
    }
}