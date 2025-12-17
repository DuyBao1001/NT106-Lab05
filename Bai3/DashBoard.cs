using MailKit;
using MailKit.Net.Pop3;
using MimeKit;
using System;
using System.Linq;
using System.Windows.Forms;


namespace Bai3
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();

            lvEmails.View = View.Details;
            lvEmails.FullRowSelect = true;
            lvEmails.GridLines = true;

            lvEmails.Columns.Clear();
            lvEmails.Columns.Add("Tiêu đề", 400);
            lvEmails.Columns.Add("Người gửi", 250);
            lvEmails.Columns.Add("Thời gian", 150);

            lvEmails.MouseDoubleClick += lvEmails_MouseDoubleClick;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string username = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim(); // App Password

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Vui lòng nhập Email và App Password.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            lvEmails.Items.Clear();
            lblTotal.Text = "Đang kết nối...";
            lblRecent.Text = "N/A (POP)";

            try
            {
                using (var client = new Pop3Client())
                {
                    // Kết nối POP3 Gmail
                    await client.ConnectAsync("pop.gmail.com", 995, true);
                    await client.AuthenticateAsync(username, password);

                    int emailCount = client.GetMessageCount();
                    lblTotal.Text = $"Tổng: {emailCount}";
                    lblRecent.Text = "N/A (POP)";

                    int maxEmailsToLoad = 20;

                    for (int i = emailCount - 1; i >= Math.Max(0, emailCount - maxEmailsToLoad); i--)
                    {
                        MimeMessage message = client.GetMessage(i);

                        var item = new ListViewItem(
                            message.Subject ?? "(Không có tiêu đề)"
                        );

                        string from = message.From.Mailboxes.FirstOrDefault()?.Address
                                      ?? "Không xác định";

                        item.SubItems.Add(from);
                        item.SubItems.Add(
                            message.Date.LocalDateTime.ToString("dd/MM/yyyy HH:mm")
                        );

                        // Lưu toàn bộ email để đọc khi double click
                        item.Tag = message;

                        lvEmails.Items.Add(item);
                    }

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi đọc email:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                lblTotal.Text = "Lỗi";
                lblRecent.Text = "Lỗi";
            }
        }

        // Double click để mở email
        private void lvEmails_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvEmails.SelectedItems.Count == 0) return;

            var item = lvEmails.SelectedItems[0];

            if (item.Tag is MimeMessage message)
            {
                ReadEmail readForm = new ReadEmail(message);
                readForm.Show();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
