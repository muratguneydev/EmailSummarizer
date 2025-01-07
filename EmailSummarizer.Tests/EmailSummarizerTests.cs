namespace EmailSummarizer.Tests;

using EmailSummarizer.Console;
using MimeKit;

[TestFixture]
public class EmailSummarizerTests
{
        [Test]
    public void SummarizeEmails_ShouldHandleMultipartAlternativeEmails_PlainTextPreferred()
    {
        // Arrange
        var multipart = new MultipartAlternative();
        multipart.Add(new TextPart("plain") { Text = "Plain Text Body" });
        multipart.Add(new TextPart("html") { Text = "<html><body>HTML Body</body></html>" });

        var message = new MimeMessage { Subject = "Multipart Test", Body = multipart };
        var summarizer = new EmailSummarizer();

        // Act
        string summary = summarizer.SummarizeEmails(new List<MimeMessage> { message });

        // Assert
        Assert.That(summary, Contains.Substring("Plain Text Body"));
        Assert.That(summary, !Contains.Substring("HTML Body"));
    }

        [Test]
    public void SummarizeEmails_ShouldHandleMultipartRelatedEmails_PlainTextPreferred()
    {
        // Arrange
        var multipart = new MultipartRelated
		{
			new TextPart("plain") { Text = "Plain Text Body" },
			new MimePart() // some other part
		};

        var message = new MimeMessage { Subject = "Multipart Test", Body = multipart };
        var summarizer = new EmailSummarizer();

        // Act
        string summary = summarizer.SummarizeEmails(new List<MimeMessage> { message });

        // Assert
        Assert.That(summary, Contains.Substring("Plain Text Body"));
    }
}