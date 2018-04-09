using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace MHPEmailerLib {

    public class EMLMailgunSendClass {

        private string APIKEY = "";
        private string MAILDOMAIN = "";

        private string FromEmailName = "";
        private string FromEmailUsername = "";

        private string ReplytoEmailName = "";
        private string ReplyToEmailAddress = "";

        private string MailSubject = "";
        private string MailHtmlBody = "";
        private string MailTextBody = "";

        public EMLMailgunSendClass(MailerSettings settings) {
            APIKEY = settings.Mailer_mailgun_api;
            MAILDOMAIN = settings.Mailer_host;
            FromEmailName = settings.Mailer_fromname;
            FromEmailUsername = settings.Mailer_fromemail;
            ReplytoEmailName = settings.Mailer_replytoname;
            ReplyToEmailAddress = settings.Mailer_replytoemail;

            if(ReplytoEmailName.Length == 0) {
                ReplytoEmailName = ReplyToEmailAddress;
            }

        }

        public EMLMailgunSendClass(string m_apikey, string m_maildomain, string m_FromEmailName, string m_FromEmailUsername, string m_ReplytoEmailName, string m_ReplyToEmailAddress) {
            APIKEY = m_apikey;
            MAILDOMAIN = m_maildomain;
            FromEmailName = m_FromEmailName;
            FromEmailUsername = m_FromEmailUsername;
            ReplytoEmailName = m_ReplytoEmailName;
            ReplyToEmailAddress = m_ReplyToEmailAddress;

            if(ReplytoEmailName.Length == 0) {
                ReplytoEmailName = ReplyToEmailAddress;
            }
        }

        public EMLReturnClass Send(List<EMLMailTarget> targets, string m_MailSubject, string m_MailHtmlBody, string m_MailTextBody, bool substitute) {
            EMLReturnClass outcome = new EMLReturnClass();
            MailSubject = m_MailSubject;
            MailHtmlBody = m_MailHtmlBody;
            MailTextBody = m_MailTextBody;

            IRestResponse response = null;
            RestClient client = new RestClient();

            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", APIKEY);
            foreach(var x in targets) {
                if(EMLHelpers.IsValidEmail(x.EmailAddress)) {
                    try {
                        if(substitute) {
                            MailHtmlBody = EMLHelpers.SubstituteFields(m_MailHtmlBody, x);
                            MailTextBody = EMLHelpers.SubstituteFields(m_MailTextBody, x);
                            MailSubject = EMLHelpers.SubstituteFields(m_MailSubject, x);
                        }

                        RestRequest request = new RestRequest();
                        request.AddParameter("domain", MAILDOMAIN, ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "" + FromEmailName + " <" + FromEmailUsername + "@" + MAILDOMAIN + ">");
                        if(EMLHelpers.IsValidEmail(ReplyToEmailAddress)) {
                            request.AddParameter("h:Reply-To", "" + ReplytoEmailName + " <" + ReplyToEmailAddress + ">");
                        }
                        request.AddParameter("to", "" + x.Name + " <" + x.EmailAddress + ">");
                        request.AddParameter("subject", MailSubject);
                        request.AddParameter("html", MailHtmlBody);
                        request.AddParameter("text", MailTextBody);
                        request.Method = Method.POST;
                        response = client.Execute(request);

                    } catch(Exception ex) {
                        outcome.Errors.Add("Error with " + x.Name + " " + ex.ToString());
                    }
                } else {
                    outcome.Errors.Add("No Email address for " + x.Name);
                }
            }

            if(outcome.Errors.Count > 0 && outcome.Success) {
                outcome.Success = false;
            }
            return outcome;
        }
    }
}
