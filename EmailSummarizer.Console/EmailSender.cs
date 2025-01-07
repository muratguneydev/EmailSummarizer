namespace EmailSummarizer.Console;

using System.Net.Mail;
using System.Net;

public class EmailSender
{
	public static void SendSummaryEmail(string summary, string senderEmail, string senderPassword, string receiverEmail)
	{
		try
		{
			using (var message = new MailMessage())
			{
				message.From = new MailAddress(senderEmail);
				message.To.Add(new MailAddress(receiverEmail));
				message.Subject = "School Email Summary";
				message.Body = summary;

				using (var smtp = new SmtpClient("smtp.gmail.com"))
				{
					smtp.Port = 587;
					smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
					smtp.EnableSsl = true;

					smtp.Send(message);
				}
			}
			System.Console.WriteLine("Summary email sent successfully.");
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"Error sending email: {ex.Message}");
		}
	}
}