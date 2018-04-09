using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace MHPEmailerLib {


    public class EMLSmtpSendClass {

        private string FromEmailName = "";
        private string FromEmailAddress = "";
        private bool UseSSL = false;
        private int port = 25;
        private string mailhost = "";
        private string deliverymail = "";

        private string accountname = "";
        private string accountpassword = "";

        private string MailSubject = "";
        private string MailHtmlBody = "";
        private string MailTextBody = "";

        private string ReplytoEmailName = "";
        private string ReplyToEmailAddress = "";

        public EMLSmtpSendClass(MailerSettings settings) {
            UseSSL = settings.Mailer_smtp_ssl;
            port = settings.Mailer_smtp_port;
            mailhost = settings.Mailer_host;
            deliverymail = settings.Mailer_deliveryemail;
            FromEmailName = settings.Mailer_fromname;
            FromEmailAddress = settings.Mailer_fromemail;
            ReplytoEmailName = settings.Mailer_replytoname;
            ReplyToEmailAddress = settings.Mailer_replytoemail;
            accountname = settings.Mailer_account;
            accountpassword = settings.Mailer_password;

            if(FromEmailName.Length == 0) {
                FromEmailName = FromEmailAddress;
            }

            if(ReplytoEmailName.Length == 0) {
                ReplytoEmailName = ReplyToEmailAddress;
            }

        }


        public EMLSmtpSendClass(bool m_usessl, int m_port, string m_mailhost, string m_accountname, string m_accountpassword, string m_deliverymail, string m_FromEmailName, string m_FromEmailAddress, string m_ReplytoEmailName, string m_ReplyToEmailAddress) {
            UseSSL = m_usessl;
            port = m_port;
            mailhost = m_mailhost;
            deliverymail = m_deliverymail;
            FromEmailName = m_FromEmailName;
            FromEmailAddress = m_FromEmailAddress;
            ReplytoEmailName = m_ReplytoEmailName;
            ReplyToEmailAddress = m_ReplyToEmailAddress;
            accountname = m_accountname;
            accountpassword = m_accountpassword;

            if(FromEmailName.Length == 0) {
                FromEmailName = FromEmailAddress;
            }

            if(ReplytoEmailName.Length == 0) {
                ReplytoEmailName = ReplyToEmailAddress;
            }
        }

        public EMLReturnClass Send(List<EMLMailTarget> targets, string m_MailSubject, string m_MailHtmlBody, string m_MailTextBody, bool substitute) {

            MailSubject = m_MailSubject;
            MailHtmlBody = m_MailHtmlBody;
            MailTextBody = m_MailTextBody;

            EMLReturnClass outcome = new EMLReturnClass();
            AlternateView av1 = null;
            AlternateView av2 = null;
            SmtpClient client = new SmtpClient();

            client.Port = port;
            client.Host = mailhost;
            client.EnableSsl = UseSSL;

            if(accountname.Length > 0) {
                client.Credentials = new NetworkCredential(accountname, accountpassword);
            } else {
                client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            }

            foreach(var x in targets) {
                if(EMLHelpers.IsValidEmail(x.EmailAddress)) {
                    try {
                        if(substitute) {
                            MailHtmlBody = EMLHelpers.SubstituteFields(m_MailHtmlBody, x);
                            MailTextBody = EMLHelpers.SubstituteFields(m_MailTextBody, x);
                            MailSubject = EMLHelpers.SubstituteFields(m_MailSubject, x);
                        }

                        MailMessage mailMessage = new MailMessage(new MailAddress(FromEmailAddress, FromEmailName), new MailAddress(x.EmailAddress, x.Name));

                        mailMessage.Subject = MailSubject;

                        if(EMLHelpers.IsValidEmail(deliverymail)) {
                            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay;
                            mailMessage.Headers.Add("Disposition-Notification-To", deliverymail);
                        }
                        if(MailHtmlBody.Length > 0) {
                            mailMessage.IsBodyHtml = true;
                            if(MailTextBody.Length > 0) {
                                av2 = AlternateView.CreateAlternateViewFromString(MailTextBody, null, MediaTypeNames.Text.Plain);
                                mailMessage.AlternateViews.Add(av2);
                                av1 = AlternateView.CreateAlternateViewFromString(MailHtmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);
                                mailMessage.AlternateViews.Add(av1);
                            } else {
                                av1 = AlternateView.CreateAlternateViewFromString(MailHtmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);
                                mailMessage.AlternateViews.Add(av1);
                            }
                        } else {
                            mailMessage.IsBodyHtml = false;
                            av2 = AlternateView.CreateAlternateViewFromString(MailTextBody, null, MediaTypeNames.Text.Plain);
                            mailMessage.AlternateViews.Add(av2);
                        }

                        if(EMLHelpers.IsValidEmail(ReplyToEmailAddress)) {
                            mailMessage.Headers.Add("Return-Path", ReplyToEmailAddress);
                            mailMessage.ReplyToList.Add(new MailAddress(ReplyToEmailAddress, ReplytoEmailName));
                        }

                        client.Send(mailMessage);

                    } catch(Exception ex) {
                        outcome.Errors.Add("Sending Error with " + x.Name + " " + ex.ToString());
                    }
                } else {
                    outcome.Errors.Add("No Email address for " + x.Name);
                }
            }

            return outcome;
        }
    }
}
