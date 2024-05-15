
using MailDataAccessLayer.Enums;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;

namespace MailBusinessLayer.EmailSender;

using MailKit.Net.Smtp;
using MimeKit;

public class EmailSender
{
    private readonly String? _address;
    private readonly String? _name;
    private readonly String? _password;

    public EmailSender(IConfiguration config)
    {
        var configuration = config;
        _address = configuration.GetSection("SenderMail").Value;
        _name = configuration.GetSection("SenderName").Value;
        _password = configuration.GetSection("Password").Value;
    }
    public void Send(String content,String receiverAddress, OutputType outputType){
        var message = new MimeMessage ();
        
        message.From.Add (new MailboxAddress (_name, _address));
        Console.WriteLine(receiverAddress);
        message.To.Add (new MailboxAddress (receiverAddress, receiverAddress ));
        message.Subject = "Report results";

        message.Body = new TextPart (outputType == OutputType.String ? TextFormat.Plain : TextFormat.Html) {
            Text = content
        };

        using (var client = new SmtpClient ()) {
            client.Connect ("smtp.gmail.com", 587, false);
            client.Authenticate(_address, _password);
            client.Send (message);
            client.Disconnect (true);
        }
    }
}