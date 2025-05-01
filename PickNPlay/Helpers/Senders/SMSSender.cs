using Vonage;
using Vonage.Request;

namespace PickNPlay.Helpers.Senders
{
    public class SMSSender
    {
        private const string From = "+380505212564";

        public async Task SendVerificationToken(string phoneNumber, string verificationToken)
        {
            try
            {
                var credentials = Credentials.FromApiKeyAndSecret("b1b313f4", "5oJIpWmwtEd7OKMY");

                var VonageClient = new VonageClient(credentials);

                var response = await VonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
                {
                    To = phoneNumber.Replace('+', ' ').Trim(),
                    From = "Pick&Play",
                    Text = $"Your verification code is {verificationToken}. Keep it secret."
                });

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
