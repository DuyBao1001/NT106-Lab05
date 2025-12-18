using System;
using System.Windows.Forms;
using MimeKit;

namespace Bai3
{
    public partial class ReadEmail : Form
    {
        // Hàm khởi tạo nhận vào một bức thư (MimeMessage)
        public ReadEmail(MimeMessage message)
        {
            InitializeComponent();
            LoadEmail(message);
        }

        void LoadEmail(MimeMessage message)
        {
            // Hiển thị thông tin cơ bản
            txtFrom.Text = message.From.ToString();
            txtTo.Text = message.To.ToString();
            txtSubject.Text = message.Subject;

            // Xử lý nội dung thư (Body)
            // Nếu có HTML thì hiện HTML, nếu không thì hiện Text thường
            if (!string.IsNullOrEmpty(message.HtmlBody))
            {
                webBrowser1.DocumentText = message.HtmlBody;
            }
            else
            {
                // Dùng thẻ <pre> để giữ nguyên định dạng xuống dòng của văn bản
                webBrowser1.DocumentText = $"<pre>{message.TextBody}</pre>";
            }
        }
    }
}
