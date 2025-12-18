using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;
using System;
using System.Linq;
using System.Windows.Forms;
using MailKit;

namespace Bai2
{
    public partial class Bai2 : Form
    {
        public Bai2()
        {
            InitializeComponent();
            lvEmails.View = View.Details;
            lvEmails.FullRowSelect = true; 
            lvEmails.GridLines = true;
            lvEmails.Columns.Add("Tiêu đề", 400);
            lvEmails.Columns.Add("Người gửi", 250);
            lvEmails.Columns.Add("Thời gian", 150);
        }
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string username = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email và Mật khẩu App Password.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            lvEmails.Items.Clear();
            lblTotal.Text = "Tổng: Đang kết nối...";
            lblRecent.Text = "Mới: Đang tải...";

            try
            {
                using (var client = new ImapClient())
                {
                    await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(username, password);
                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadOnly);
                    lblTotal.Text = $"Tổng: {inbox.Count}";
                    lblRecent.Text = $"Mới: {inbox.Recent}";

                    int emailCount = inbox.Count;
                    int maxEmailsToLoad = 20; 
                    for (int i = emailCount - 1; i >= Math.Max(0, emailCount - maxEmailsToLoad); i--)
                    {
                        var message = await inbox.GetMessageAsync(i);

                        var item = new ListViewItem(message.Subject);

                        string fromAddress = message.From.Mailboxes.FirstOrDefault()?.Address ?? "Không xác định";

                        item.SubItems.Add(fromAddress);
                        item.SubItems.Add(message.Date.LocalDateTime.ToString("dd/MM/yyyy HH:mm"));

                        lvEmails.Items.Add(item);
                    }

                    await client.DisconnectAsync(true);
                }

            }
            catch (AuthenticationException)
            {
                MessageBox.Show("Lỗi xác thực: Sai Email hoặc App Password! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTotal.Text = "Tổng: Lỗi";
                lblRecent.Text = "Mới: Lỗi";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTotal.Text = "Tổng: Lỗi";
                lblRecent.Text = "Mới: Lỗi";
            }
        }
    }
}