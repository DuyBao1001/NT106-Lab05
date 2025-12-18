using System;
using System.Windows.Forms;
using MimeKit;
using System.Linq;

namespace Bai6
{
    public partial class ReadEmail : Form
    {
        public ReadEmail()
        {
            InitializeComponent();
        }

        public ReadEmail(MimeMessage message)
        {
            InitializeComponent();
            HienThiNoiDung(message);
        }

        private void HienThiNoiDung(MimeMessage message)
        {
            // 1. Hiển thị Header
            textBox1.Text = message.From.ToString(); // From
            textBox2.Text = message.To.ToString();   // To
            textBox3.Text = message.Subject;         // Subject

            // 2. Xử lý Body (Ưu tiên HTML)
            string bodyContent = "";
            var bodyHtml = message.BodyParts.OfType<TextPart>().FirstOrDefault(x => x.IsHtml);

            if (bodyHtml != null)
            {
                bodyContent = bodyHtml.Text;
            }
            else
            {
                var bodyText = message.BodyParts.OfType<TextPart>().FirstOrDefault(x => !x.IsHtml);
                if (bodyText != null)
                {
                    bodyContent = $"<html><body><pre>{System.Security.SecurityElement.Escape(bodyText.Text)}</pre></body></html>";
                }
                else
                {
                    bodyContent = message.TextBody ?? "<i>Không có nội dung.</i>";
                }
            }

            // Hiển thị lên WebBrowser
            webBrowser1.DocumentText = bodyContent;
        }
    }
}