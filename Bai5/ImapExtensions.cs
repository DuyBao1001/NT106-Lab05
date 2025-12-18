using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using MailKit.Search;

namespace Bai5
{
    public static class ImapExtensions
    {
        public static Task<IList<UniqueId>> SearchAsync(
            this IMailFolder folder,
            string query,
            CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var searchQuery =
                SearchQuery.SubjectContains(query)
                .Or(SearchQuery.BodyContains(query));

            return folder.SearchAsync(searchQuery, cancellationToken);
        }
    }
}
