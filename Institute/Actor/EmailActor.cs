using Akka.Actor;
using Azure.Core;
using Institute.DAOs.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using MimeKit;
using System;
using ContentType = MimeKit.ContentType;

namespace Institute.Actor
{
    public class EmailActor : ReceiveActor
    {
        public EmailActor()
        {
            ReceiveAsync<ReportResponse>(async message =>
            {


                try
                {
                    if (string.IsNullOrWhiteSpace(message.To))
                    {
                        Sender.Tell("Recipient email address is null or empty.");
                        return;
                    }

                    await SendReportAsync(message.Attachment, message.To);
                    Console.WriteLine("Email sent successfully.");

                    // Send a response back to the sender (controller) with a success message.


                    Sender.Tell("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Sender.Tell("Error sending email: " + ex.Message);
                }
            });
        }

        private async Task SendReportAsync(byte[] pdfBytes, string recipientEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ben Bartoletti", "ben.bartoletti@ethereal.email"));
            message.To.Add(new MailboxAddress(string.Empty, recipientEmail));

            message.Subject = "PDF Document";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Kindly, find the attached PDF document.";

            if (pdfBytes != null && pdfBytes.Length > 0)
            {
                var attachment = bodyBuilder.Attachments.Add("students report.pdf", pdfBytes, ContentType.Parse("application/pdf"));
                message.Body = bodyBuilder.ToMessageBody();
            }
            else
            {
                throw new ArgumentException("PDF attachment data is null or empty.");
            }

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("ben.bartoletti@ethereal.email", "ryK43QqwQekdEFpEP8");


                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (AuthenticationException authEx)
                {
                   
                    throw  new AbandonedMutexException(authEx.Message);
                }



                message.Dispose();

            }


        }
    }
}
