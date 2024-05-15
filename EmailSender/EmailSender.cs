using MailKit.Net.Smtp;
using MimeKit;

namespace EmailSender;

public class EmailSender
{
    public void SendEmail(){
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("Report sender", "sreportsender@gmail.com"));
        message.To.Add (new MailboxAddress ("David Rusnak", "davidkorusnak@gmail.com"));
        message.Subject = "How you doin'?";

        message.Body = new TextPart ("plain") {
            Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
        };

        using (var client = new SmtpClient ()) {
            client.Connect ("smtp.gmail.com", 587, false);
            client.Authenticate("sreportsender@gmail.com", "cfmd mfht fxwn xubn");
            client.Send (message);
            client.Disconnect (true);
        }
    }
}