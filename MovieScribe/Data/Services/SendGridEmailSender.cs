using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;

public class SendGridEmailSender : IEmailSender
{
    // _apiKey holds the API key needed to authenticate with the SendGrid API.
    private readonly string _apiKey;

    // _logger is a logging instance for logging events that occur during the execution of the code.
    private readonly ILogger<SendGridEmailSender> _logger;

    // Constructor for the SendGridEmailSender class, which is passed the apiKey and logger when an instance is created.
    public SendGridEmailSender(string apiKey, ILogger<SendGridEmailSender> logger)
    {
        // Initialize the _apiKey and _logger fields.
        _apiKey = apiKey;
        _logger = logger;
    }

    // This method is tasked with sending an email message.
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        // Create a new SendGrid client using the API key.
        var client = new SendGridClient(_apiKey);

        // Create a new SendGrid message. You can set the sender (From), subject, and content of the email.
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("trahornpg@gmail.com", "MedijasSkapis"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };

        // Set the recipient of the email using the 'email' parameter passed into the method.
        msg.AddTo(new EmailAddress(email));

        // Asynchronously send the email. 
        var response = await client.SendEmailAsync(msg);

        // Check if the email sending was successful. If not, log the error message.
        if (response.StatusCode != HttpStatusCode.Accepted)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            _logger.LogError($"Failed to send email. Response body: {responseBody}");
        }
    }
}
