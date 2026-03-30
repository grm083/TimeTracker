using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using SBS.IT.Utilities.Logger.Core;

namespace SBS.IT.Utilities.Shared.Communication
{
    public class EmailCommunication 
    {
        private readonly ILogger _Logger;
        public EmailCommunication(ILogger Logger)
        {
            _Logger = Logger;
        }
        private bool IsEmailAddressWellFormed(string email)
        {
            var reg = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            return reg.IsMatch(email.Trim());
        }
        public void SendEmail(string senderName, string senderEmail, string recipientName, string recipientEmail, string subject, string textBody, string htmlBody, string mailServer)
        {
            string[] sendEmails;
            try
            {
                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    var smtp = new SmtpClient(mailServer);
                    if (recipientEmail.Contains(';'))
                    {
                        sendEmails = recipientEmail.Split(';');
                    }
                    else if (recipientEmail.Contains(','))
                    {
                        sendEmails = recipientEmail.Split(',');
                    }
                    else
                    {
                        sendEmails = new string[] { recipientEmail };
                    }
                    if (sendEmails != null && sendEmails.Length > 0)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(senderEmail, senderName);
                        foreach (string sendMail in sendEmails)
                        {
                            if (!string.IsNullOrEmpty(sendMail))
                            {
                                message.To.Add(new MailAddress(sendMail, recipientName));
                            }
                        }
                        message.Subject = subject;
                        message.IsBodyHtml = !string.IsNullOrEmpty(htmlBody);
                        message.Body = message.IsBodyHtml ? htmlBody : textBody;
                        smtp.Send(message);
                        message.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                _Logger.WriteMessage(this.GetType(), LogLevel.FATAL, string.Empty, e);
            }
        }
        public void SendEmail(string senderName, string senderEmail, string recipientName, string recipientEmail, string subject, string textBody, string htmlBody, string mailServer, string recipientEmailCC)
        {
            string[] sendEmails;
            string[] sendEmailsCC;
            try
            {
                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    var smtp = new SmtpClient(mailServer);
                    if (recipientEmail.Contains(';'))
                    {
                        sendEmails = recipientEmail.Split(';');
                    }
                    else if (recipientEmail.Contains(','))
                    {
                        sendEmails = recipientEmail.Split(',');
                    }
                    else
                    {
                        sendEmails = new string[] { recipientEmail };
                    }
                    if (sendEmails != null && sendEmails.Length > 0)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(senderEmail, senderName);
                        foreach (string sendMail in sendEmails)
                        {
                            if (!string.IsNullOrEmpty(sendMail))
                            {
                                message.To.Add(new MailAddress(sendMail, recipientName));
                            }
                        }
                        if (!string.IsNullOrEmpty(recipientEmailCC))
                        {
                            if (recipientEmailCC.Contains(';'))
                            {
                                sendEmailsCC = recipientEmailCC.Split(';');
                            }
                            else if (recipientEmailCC.Contains(','))
                            {
                                sendEmailsCC = recipientEmailCC.Split(',');
                            }
                            else
                            {
                                sendEmailsCC = new string[] { recipientEmailCC };
                            }
                            if (sendEmailsCC != null && sendEmailsCC.Length > 0)
                            {
                                foreach (string sendMail in sendEmailsCC)
                                {
                                    if (!string.IsNullOrEmpty(sendMail))
                                    {
                                        message.CC.Add(new MailAddress(sendMail));
                                    }
                                }
                            }
                        }
                        message.Subject = subject;
                        message.IsBodyHtml = !string.IsNullOrEmpty(htmlBody);
                        message.Body = message.IsBodyHtml ? htmlBody : textBody;
                        smtp.Send(message);
                        message.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                _Logger.WriteMessage(this.GetType(), LogLevel.FATAL, string.Empty, e);
            }
        }
        public void SendEmail(string senderEmail, string recipientEmail, string subject, string textBody, string htmlBody, string mailServer, int mailServerPort, string mailServerUserName, string mailServerPassword)
        {
            string[] sendEmails;
            try
            {
                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    var smtp = new SmtpClient(mailServer, mailServerPort);
                    smtp.Credentials = new NetworkCredential(mailServerUserName, mailServerPassword);
                    if (recipientEmail.Contains(';'))
                    {
                        sendEmails = recipientEmail.Split(';');
                    }
                    else if (recipientEmail.Contains(','))
                    {
                        sendEmails = recipientEmail.Split(',');
                    }
                    else
                    {
                        sendEmails = new string[] { recipientEmail };
                    }
                    if (sendEmails != null && sendEmails.Length > 0)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(senderEmail);
                        foreach (string sendMail in sendEmails)
                        {
                            if (!string.IsNullOrEmpty(sendMail))
                            {
                                message.To.Add(sendMail);
                            }
                        }
                        message.Subject = subject;
                        message.IsBodyHtml = !string.IsNullOrEmpty(htmlBody);
                        message.Body = message.IsBodyHtml ? htmlBody : textBody;
                        smtp.Send(message);
                        message.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                _Logger.WriteMessage(this.GetType(), LogLevel.FATAL, string.Empty, e);
            }
        }
    }
}