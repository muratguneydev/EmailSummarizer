namespace EmailSummarizer.Console;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

public class ImapEmailReader
{
	private readonly string _emailAccount;
	private readonly string _emailPassword;

	public ImapEmailReader(string emailAccount, string emailPassword)
	{
		_emailAccount = emailAccount;
		_emailPassword = emailPassword;
	}

	public List<MimeMessage> GetEmails()
	{
		var messages = new List<MimeMessage>();
		try
		{
			using (var client = new ImapClient())
			{
				client.Connect("imap.gmail.com", 993, true);
				client.Authenticate(_emailAccount, _emailPassword);
				client.Inbox.Open(FolderAccess.ReadOnly);

				var query = SearchQuery.All;
				var uids = client.Inbox.Search(query);

				foreach (var uid in uids)
				{
					var message = client.Inbox.GetMessage(uid);
					messages.Add(message);
				}

				client.Disconnect(true);
			}
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"Error reading emails: {ex.Message}");
			throw;
		}
		return messages;
	}
}