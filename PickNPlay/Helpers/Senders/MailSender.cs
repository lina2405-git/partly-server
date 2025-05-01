using MailKit.Net.Smtp;
using MimeKit;

namespace PickNPlay.Helpers.Senders
{
    public class MailSender
    {
        private readonly IConfiguration configuration;
        public MailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendVerificationEmailAsync(string emailAddress, string verificationToken, string userName)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("picknplay.news@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailAddress));
            email.Subject = "Verify your email";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = @"
<style>
    body {
        font-family: Arial, sans-serif;
        background-color: #f4f4f4;
        margin: 0;
        padding: 0;
    }
    .container {
        width: 100%;
        max-width: 600px;
        margin: 0 auto;
        background-color: #ffffff;
        padding: 20px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }
    .header {
        text-align: center;
        padding: 10px 0;
    }
    .content {
        padding: 20px 0;
    }
    .button {
        display: inline-block;
        background-color: #007bff;
        color: #ffffff;
        padding: 10px 20px;
        text-decoration: none;
        border-radius: 5px;
    }
    .footer {
        text-align: center;
        font-size: 12px;
        color: #888888;
        padding: 10px 0;
    }
</style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Email verification</h1>
        </div>
        <div class='content'>
            <p>Greetings, " + userName + @"</p>
            <p>Thanks for registration! Please verify your email address by clicking the link below.</p>
            <p><a href='http://localhost:4200/email/verify?token=" + verificationToken + @"' class='button'>Verify</a></p>
            <p>If you do not know what's going on, just skip this message</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Pick&Play. All rights reserved.</p>
        </div>
    </div>
</body>" };

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("picknplay.news@gmail.com", "uliu ipnq warx selv");
                emailClient.Send(email);
                emailClient.Disconnect(true);
            }
        }

        public void SendResetPasswordEmail(string reciepient, string resetToken, string userName)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("picknplay.news@gmail.com"));
            email.To.Add(MailboxAddress.Parse(reciepient));
            email.Subject = "Reset your password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = @" <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
            padding: 20px;
        }
        .container {
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            max-width: 600px;
            margin: 0 auto;
        }
        .token {
            display: block;
            font-size: 20px;
            color: #007bff;
            background-color: #e9ecef;
            padding: 10px;
            border-radius: 5px;
            text-align: center;
            margin: 20px 0;
            word-break: break-all;
        }
        .footer {
            margin-top: 20px;
            font-size: 12px;
            color: #777;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h2>Password Reset Request</h2>
        <p>Hello, <b>" + userName + @"</b></p>
        <p>We received a request to reset your password. Use the following code to reset your password:</p>
        <h2 class=""token"">" + resetToken + @"</h2>
        <p>If you did not request a password reset, please ignore this email.</p>
        <div class=""footer"">
            <p>If you're having trouble using the code, copy and paste it into the password reset form on our website.</p>
            <p>And for future: don't forget your passwords, please (^-^)</p>
        <p>&copy; 2024 Pick&Play. All rights reserved.</p>
        </div>
    </div>
</body>"
            };

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("picknplay.news@gmail.com", "uliu ipnq warx selv");
                emailClient.Send(email);
                emailClient.Disconnect(true);
            }

        }
    }
}
