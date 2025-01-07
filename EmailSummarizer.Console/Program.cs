namespace EmailSummarizer.Console;

public class Program
{
	public static void Main(string[] args)
	{
		var emailAccount = Environment.GetEnvironmentVariable("SOURCE_EMAIL_USER");
		var emailPassword = Environment.GetEnvironmentVariable("SOURCE_EMAIL_PASSWORD");
		var senderEmail = Environment.GetEnvironmentVariable("FEEDBACK_SENDER_EMAIL");
		var senderPassword = Environment.GetEnvironmentVariable("FEEDBACK_SENDER_PASSWORD");
		var receiverEmail = Environment.GetEnvironmentVariable("FEEDBACK_RECEIVER_EMAIL");

		var emailReader = new ImapEmailReader(emailAccount, emailPassword);
		var summarizer = new EmailSummarizer();
		var fileUploader = new GoogleDriveUploader();

		try
		{
			var messages = emailReader.GetEmails();
			var mergedText = summarizer.SummarizeEmails(messages);
			var filePath = "merged_emails.txt";
			File.WriteAllText(filePath, mergedText);
			var fileId = fileUploader.UploadFile(filePath);

			System.Console.WriteLine($"File uploaded to Google Drive with ID: {fileId}");
			EmailSender.SendSummaryEmail(mergedText, senderEmail, senderPassword, receiverEmail);
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}
}