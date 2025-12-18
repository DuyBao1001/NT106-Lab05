using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit.Search;
using MimeKit;

namespace Bai6
{
    public partial class Dashboard : Form
    {
        // Khai báo biến toàn cục
        private ImapClient _imapClient = null;
        private string _userEmail = "";
        private string _appPassword = "";

        public Dashboard()
        {
            InitializeComponent();

            this.button1.Click += new System.EventHandler(this.btnDangNhap_Click);
            this.button4.Click += new System.EventHandler(this.btnDangXuat_Click);
            this.button3.Click += new System.EventHandler(this.btnRefresh_Click);
            this.button2.Click += new System.EventHandler(this.btnGuiMail_Click);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
        }

        // --- 1. ĐĂNG NHẬP ---
        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            _userEmail = textBox1.Text.Trim();
            _appPassword = textBox2.Text.Trim();
            string imapHost = textBox3.Text.Trim();
            int imapPort = Convert.ToInt32(numericUpDown1.Value);

            if (string.IsNullOrEmpty(_userEmail) || string.IsNullOrEmpty(_appPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi");
                return;
            }

            try
            {
                _imapClient = new ImapClient();
                _imapClient.CheckCertificateRevocation = false; 

                await _imapClient.ConnectAsync(imapHost, imapPort, SecureSocketOptions.SslOnConnect);
                await _imapClient.AuthenticateAsync(_userEmail, _appPassword);

                MessageBox.Show("Đăng nhập thành công!", "Thông báo");
                CapNhatGiaoDien(true);
                await TaiMailAsync(); // Tự động tải mail ngay
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi");
            }
        }

        // --- 2. ĐĂNG XUẤT ---
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            try { _imapClient?.Disconnect(true); } catch { }
            _imapClient = null;
            listView1.Items.Clear();
            CapNhatGiaoDien(false);
            textBox2.Text = "";
        }

        private void CapNhatGiaoDien(bool loggedIn)
        {
            button1.Visible = !loggedIn;
            button2.Visible = loggedIn;
            button3.Visible = loggedIn;
            button4.Visible = loggedIn;
            textBox1.ReadOnly = textBox2.ReadOnly = textBox3.ReadOnly = textBox4.ReadOnly = loggedIn;
            numericUpDown1.Enabled = numericUpDown2.Enabled = !loggedIn;
        }

        // --- 3. TẢI MAIL ---
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await TaiMailAsync();
        }

        private async Task TaiMailAsync()
        {
            if (_imapClient == null || !_imapClient.IsConnected) return;

            listView1.Items.Clear();

            try
            {
                var inbox = _imapClient.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                // Lấy danh sách 20 mail mới nhất
                var uids = await inbox.SearchAsync(SearchQuery.All);
                var latestUids = uids.Skip(Math.Max(0, uids.Count - 20)).ToList();
                latestUids.Reverse(); // Đảo ngược để mail mới nhất lên đầu

                if (latestUids.Count > 0)
                {
                    // Truyền nguyên danh sách (latestUids) vào FetchAsync
                    var summaries = await inbox.FetchAsync(latestUids, MessageSummaryItems.Envelope | MessageSummaryItems.InternalDate);

                    foreach (var summary in summaries)
                    {
                        ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString());
                        item.SubItems.Add(summary.Envelope.From.ToString());
                        item.SubItems.Add(summary.Envelope.Subject);
                        item.SubItems.Add(summary.InternalDate?.ToString("dd/MM/yyyy HH:mm") ?? "");

                        // Lưu UID để dùng khi đọc mail
                        item.Tag = summary.UniqueId;

                        listView1.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải mail: {ex.Message}", "Lỗi");
            }
        }

        // --- 4. ĐỌC MAIL ---
        private async void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0 || _imapClient == null) return;

            if (listView1.SelectedItems[0].Tag is UniqueId uid)
            {
                try
                {
                    var message = await _imapClient.Inbox.GetMessageAsync(uid);
                    ReadEmail formRead = new ReadEmail(message);
                    formRead.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi đọc mail: {ex.Message}", "Lỗi");
                }
            }
        }

        // --- 5. GỬI MAIL ---
        private void btnGuiMail_Click(object sender, EventArgs e)
        {
            if (_imapClient == null || !_imapClient.IsConnected) return;

            string smtpHost = textBox4.Text.Trim();
            int smtpPort = Convert.ToInt32(numericUpDown2.Value);

            Form2 formSend = new Form2(_userEmail, _appPassword, smtpHost, smtpPort);
            formSend.ShowDialog();
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }
    }
}