using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;

namespace MHPEmailerLib {

    public class EMLExchangeSendClass {
        private ExchangeVersion exVer = ExchangeVersion.Exchange2007_SP1;
        private string MailserverUrl = "";
        private string MailserverDomain = "";
        private string MailserverUsername = "";
        private string MailserverPassword = "";

        private string ReplytoEmailName = "";
        private string ReplyToEmailAddress = "";

        private string FromEmailName = "";
        private string FromEmailAddress = "";

        private string MailSubject = "";
        private string MailHtmlBody = "";
        private string MailTextBody = "";

        private EmailMessage message = null;
        private ExchangeService service = null;
        private WebCredentials cred = null;


        public EMLExchangeSendClass(MailerSettings settings) {
            exVer = EMLHelpers.GetExchangeVersion(settings.Mailer_exchange_mode);
            MailserverUrl = settings.Mailer_host;
            MailserverDomain = settings.Mailer_exchange_domain;
            MailserverUsername = settings.Mailer_account;
            MailserverPassword = settings.Mailer_password;
            FromEmailName = settings.Mailer_fromname;
            FromEmailAddress = settings.Mailer_fromemail;
            ReplytoEmailName = settings.Mailer_replytoname;
            ReplyToEmailAddress = settings.Mailer_replytoemail;

            if(FromEmailName.Length == 0) {
                FromEmailName = FromEmailAddress;
            }
            if(ReplytoEmailName.Length == 0) {
                ReplytoEmailName = ReplyToEmailAddress;
            }
        }


        public EMLExchangeSendClass(string m_ExchangeVersionEnum, string m_MailserverUrl, string m_MailserverDomain, string m_MailserverUsername, string m_MailserverPassword, string m_FromEmailName, string m_FromEmailAddress, string m_ReplytoEmailName, string m_ReplyToEmailAddress) {
            exVer = EMLHelpers.GetExchangeVersion(m_ExchangeVersionEnum);
            MailserverUrl = m_MailserverUrl;
            MailserverDomain = m_MailserverDomain;
            MailserverUsername = m_MailserverUsername;
            MailserverPassword = m_MailserverPassword;
            FromEmailName = m_FromEmailName;
            FromEmailAddress = m_FromEmailAddress;
            ReplytoEmailName = m_ReplytoEmailName;
            ReplyToEmailAddress = m_ReplyToEmailAddress;

            if(FromEmailName.Length == 0) {
                FromEmailName = m_FromEmailAddress;
            }
            if(ReplytoEmailName.Length == 0) {
                ReplytoEmailName = ReplyToEmailAddress;
            }
        }

        public EMLReturnClass Send(List<EMLMailTarget> targets, string m_MailSubject, string m_MailHtmlBody, string m_MailTextBody, bool substitute) {
            EMLReturnClass outcome = new EMLReturnClass();
            EMLReturnClass outcome2 = new EMLReturnClass();

            MailSubject = m_MailSubject;
            MailHtmlBody = m_MailHtmlBody;
            MailTextBody = m_MailTextBody;

            service = new ExchangeService(exVer, TimeZoneInfo.Local);

            try {
                ServicePointManager.ServerCertificateValidationCallback = delegate(Object oobj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
                service.Url = new Uri(MailserverUrl);
            } catch(Exception ex) {
                outcome.SetFailureMessage("Problem contacting exchange mail host", ex.ToString());
            }
            if(outcome.Success) {
                try {
                    if(MailserverDomain.Length > 0) {
                        cred = new WebCredentials(MailserverUsername, MailserverPassword, MailserverDomain);
                    } else {
                        cred = new WebCredentials(MailserverUsername, MailserverPassword);
                    }
                    service.Credentials = cred;

                } catch(Exception ex) {
                    outcome.SetFailureMessage("Problem contacting exchange mail host", ex.ToString());
                }
            }
            if(outcome.Success) {

                foreach(var x in targets) {
                    if(EMLHelpers.IsValidEmail(x.EmailAddress)) {
                        try {
                            if(substitute) {
                                MailHtmlBody = EMLHelpers.SubstituteFields(m_MailHtmlBody, x);
                                //MailTextBody = EMLHelpers.SubstituteFields(m_MailTextBody, x);
                                MailSubject = EMLHelpers.SubstituteFields(m_MailSubject, x);
                            }
                            message = new EmailMessage(service);
                            message.Body = new MessageBody(BodyType.HTML, MailHtmlBody);
                            message.Subject = MailSubject;
                            message.ToRecipients.Add(new EmailAddress(x.Name, x.EmailAddress));
                            message.From = new EmailAddress(FromEmailName, FromEmailAddress);

                            if(EMLHelpers.IsValidEmail(ReplyToEmailAddress)) {
                                //message.ReplyTo.Add(new MailAddress(ReplyToEmailAddress, ReplytoEmailName));
                                message.ReplyTo.Add(new EmailAddress(ReplytoEmailName, ReplyToEmailAddress));
                            }

                            message.SendAndSaveCopy();

                        } catch(Exception ex) {
                            outcome.Errors.Add("Sending Error with " + x.Name + " " + ex.ToString());
                        }
                    } else {
                        outcome.Errors.Add("No Email address for " + x.Name);
                    }
                }
            }

            if(outcome.Errors.Count > 0 && outcome.Success) {
                outcome.Success = false;
            }
            return outcome;
        }

    }

}
