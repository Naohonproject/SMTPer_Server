using System.Net.Mail;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Net;

namespace MailUtils
{
    public class MailUtils
    {
        public static async Task<MailResponse> SendSmtpMail(
            string _from,
            string _to,
            string _subject,
            string _body,
            string _email,
            string _password,
            string _host,
            int _port,
            bool _isUseSecure
        )
        {
            MailMessage message = new MailMessage(_from, _to, _subject, _body);
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            using var smtpClient = new SmtpClient(_host);
            smtpClient.Port = _port;
            smtpClient.EnableSsl = _isUseSecure;
            smtpClient.Credentials = new NetworkCredential(_email, _password);
            smtpClient.Timeout = 20000;

            try
            {
                await smtpClient.SendMailAsync(message);
                return new MailResponse(
                    "Congratulation! Your Email has been sent successfully",
                    "success"
                );
            }
            catch (System.Exception e)
            {
                return new MailResponse(
                    $"Your email has not been sent.Because {e.Message}",
                    "fail"
                );
            }
        }
    }
}
