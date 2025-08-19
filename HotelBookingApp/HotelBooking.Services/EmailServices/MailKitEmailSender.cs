using HotelBooking.Services.Contracts.EmailServicesContracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Options;
namespace HotelBooking.Services.EmailServices;

public sealed class MailKitEmailSender(IOptionsSnapshot<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _smtpOptions = options.Value;

    public async Task SendAsync(
        string to,
        string subject,
        string htmlBody,
        string? textBody = null,
        IEnumerable<(string fileName, byte[] bytes)>? attachments = null,
        CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpOptions.FromName, _smtpOptions.FromAddress));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = textBody ?? TextFormatConverter(htmlBody)
        };

        if (attachments != null)
            foreach (var (fileName, bytes) in attachments)
                builder.Attachments.Add(fileName, bytes);

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        // подаване само на валиднивалиден сертификат:
        // smtp.ServerCertificateValidationCallback = (s, c, h, e) => e == SslPolicyErrors.None;

        var secure = _smtpOptions.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect;

        await smtp.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, secure, ct);

        if (!string.IsNullOrWhiteSpace(_smtpOptions.User))
            await smtp.AuthenticateAsync(_smtpOptions.User, _smtpOptions.Password, ct);

        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }

    // Проста конверсия ако няма текстово тяло (за по-добра доставяемост)
    private static string TextFormatConverter(string html)
        => HtmlToText.Simple(html);
}

