using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Search; 

namespace Bai5
{
  
    public class EmailHandler
    {
        private readonly ApiHandler _apiHandler;
        private const string CONTRIBUTION_SUBJECT = "Đóng góp món ăn";
        private const string DEFAULT_CONTRIBUTOR = "Người ẩn danh";

        public EmailHandler(ApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
        }

        public async Task<int> SyncContributionsAsync(EmailConfig config)
        {
            if (!config.IsConfigured)
            {
                throw new InvalidOperationException("Cấu hình Email chưa được thiết lập.");
            }

            int newDishesCount = 0;

            using (var client = new ImapClient())
            {
                try
                {
   
                    await client.ConnectAsync(config.ImapHost, config.ImapPort, SecureSocketOptions.SslOnConnect);

                    await client.AuthenticateAsync(config.Email, config.AppPassword);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadWrite);

                    var query = SearchQuery.And(
                        SearchQuery.NotSeen,
                        SearchQuery.SubjectContains(CONTRIBUTION_SUBJECT)
                    );

                    var uids = await inbox.SearchAsync(query);

                    if (!uids.Any())
                    {
                        await client.DisconnectAsync(true);
                        return 0;
                    }

                    foreach (var uid in uids)
                    {
                        var message = await inbox.GetMessageAsync(uid);

                        string contributorName = message.From.Mailboxes.FirstOrDefault()?.Name;
                        if (string.IsNullOrEmpty(contributorName))
                        {
                            contributorName = message.From.Mailboxes.FirstOrDefault()?.Address ?? DEFAULT_CONTRIBUTOR;
                        }

                        string body = message.TextBody?.Trim();

                        if (string.IsNullOrEmpty(body))
                        {
                            await inbox.SetFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        var lines = body.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        string contributionLine = lines.FirstOrDefault()?.Trim();

                        if (contributionLine == null)
                        {
                            await inbox.SetFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        var parts = contributionLine.Split(new[] { ';' }, 2, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length == 2)
                        {
                            var newDish = new Dish
                            {
                                Name = parts[0].Trim(),
                                ImageUrl = parts[1].Trim(),
                                ContributorName = contributorName,
                                ContributorId = 0
                            };

                            bool success = await _apiHandler.AddDishFromEmailAsync(newDish);

                            if (success)
                            {
                                newDishesCount++;
                            }
                        }

                        await inbox.SetFlagsAsync(uid, MessageFlags.Seen, true);
                    }
                }
                catch (AuthenticationException ex)
                {
                    throw new Exception("Lỗi Xác thực Email. Vui lòng kiểm tra lại Email và App Password.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi kết nối IMAP: {ex.Message}", ex);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync(true);
                    }
                }
            }

            return newDishesCount;
        }
    }
}