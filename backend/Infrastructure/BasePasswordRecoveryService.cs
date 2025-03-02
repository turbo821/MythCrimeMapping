using Application.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace Infrastructure
{
    public class BasePasswordRecoveryService : IPasswordRecoveryService
    {
        private NetworkCredential credential;
        private static Random random = new Random();
        public BasePasswordRecoveryService(NetworkCredential cred)
        {
            credential = cred;
        }

        public async Task<string> SendCodeAsync(string email)
        {
            string to = email;
            string from = "admin@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Код подтверждения для смены пароля MythCrimeMapping";
            string code = RandomString(10);
            message.Body = $"Код: {code}";
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = credential;
            try
            {
                await client.SendMailAsync(message);
                return code;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return code;
            }
        }
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
