namespace EmailSummarizer.Console;

using MimeKit;
using System.Text;

public class EmailSummarizer
{
    public string SummarizeEmails(List<MimeMessage> messages)
    {
        StringBuilder mergedText = new StringBuilder();

        foreach (var message in messages)
        {
            string body = "";

            if (message.Body is TextPart textPart)
            {
                body = textPart.Text;
            }
            else if (message.Body is MultipartAlternative multipart)
            {
                // Prioritize plain text
                foreach (var part in multipart)
                {
                    if (part is TextPart tp && tp.ContentType.MediaType == "text/plain")
                    {
                        body = tp.Text;
                        break; // Important: Exit loop once plain text is found
                    }
                }
                if (string.IsNullOrEmpty(body)) // if no plain text, take the first part
                {
                    body = multipart.First().ToString();
                }
            }
            else if (message.Body is MultipartRelated related)
            {
                 foreach (var part in related)
                {
                    if (part is TextPart tp && tp.ContentType.MediaType == "text/plain")
                    {
                        body = tp.Text;
                        break; // Important: Exit loop once plain text is found
                    }
                }
                 if (string.IsNullOrEmpty(body)) // if no plain text, take the first part
                {
                    body = related.First().ToString();
                }
            }
            mergedText.AppendLine($"Subject: {message.Subject}");
            mergedText.AppendLine(body);
            mergedText.AppendLine("--- End of Email ---");
            mergedText.AppendLine();
        }
        return mergedText.ToString();
    }
}

