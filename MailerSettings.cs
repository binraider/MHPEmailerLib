using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPEmailerLib {

    public class MailerSettings {

        private bool m_mailer_use_exchange = false;
        private bool m_mailer_smtp_ssl = false;
        private int m_mailer_smtp_port = 0;
        private string m_mailer_host = "";
        private string m_mailer_account = "";
        private string m_mailer_password = "";
        private string m_mailer_fromname = "";
        private string m_mailer_fromemail = "";
        private string m_mailer_replytoname = "";
        private string m_mailer_replytoemail = "";
        private string m_mailer_exchange_mode = "";
        private string m_mailer_exchange_domain = "";
        private string m_mailer_deliveryemail = "";
        private string m_mailer_mailgun_api = "";
        private bool m_mailer_use_mailgun = false;
        private bool m_mailer_use_smtp = false;


        public MailerSettings() {
        }

        public MailerSettings(bool _mailer_use_exchange, bool _mailer_smtp_ssl, int _mailer_smtp_port, string _mailer_host, string _mailer_account, string _mailer_password, string _mailer_fromname, string _mailer_fromemail, string _mailer_replytoname, string _mailer_replytoemail, string _mailer_exchange_mode, string _mailer_exchange_domain, string _mailer_deliveryemail, string _mailer_mailgun_api, bool _mailer_use_mailgun, bool _mailer_use_smtp) {
            m_mailer_use_exchange = _mailer_use_exchange;
            m_mailer_smtp_ssl = _mailer_smtp_ssl;
            m_mailer_smtp_port = _mailer_smtp_port;
            m_mailer_host = _mailer_host;
            m_mailer_account = _mailer_account;
            m_mailer_password = _mailer_password;
            m_mailer_fromname = _mailer_fromname;
            m_mailer_fromemail = _mailer_fromemail;
            m_mailer_replytoname = _mailer_replytoname;
            m_mailer_replytoemail = _mailer_replytoemail;
            m_mailer_exchange_mode = _mailer_exchange_mode;
            m_mailer_exchange_domain = _mailer_exchange_domain;
            m_mailer_deliveryemail = _mailer_deliveryemail;
            m_mailer_mailgun_api = _mailer_mailgun_api;
            m_mailer_use_mailgun = _mailer_use_mailgun;
            m_mailer_use_smtp = _mailer_use_smtp;

        }
        public bool Mailer_use_exchange {
            get { return m_mailer_use_exchange; }
            set { m_mailer_use_exchange = value; }
        }
        public bool Mailer_smtp_ssl {
            get { return m_mailer_smtp_ssl; }
            set { m_mailer_smtp_ssl = value; }
        }
        public int Mailer_smtp_port {
            get { return m_mailer_smtp_port; }
            set { m_mailer_smtp_port = value; }
        }
        public string Mailer_host {
            get { return m_mailer_host; }
            set { m_mailer_host = value; }
        }
        public string Mailer_account {
            get { return m_mailer_account; }
            set { m_mailer_account = value; }
        }
        public string Mailer_password {
            get { return m_mailer_password; }
            set { m_mailer_password = value; }
        }
        public string Mailer_fromname {
            get { return m_mailer_fromname; }
            set { m_mailer_fromname = value; }
        }
        public string Mailer_fromemail {
            get { return m_mailer_fromemail; }
            set { m_mailer_fromemail = value; }
        }
        public string Mailer_replytoname {
            get { return m_mailer_replytoname; }
            set { m_mailer_replytoname = value; }
        }
        public string Mailer_replytoemail {
            get { return m_mailer_replytoemail; }
            set { m_mailer_replytoemail = value; }
        }
        public string Mailer_exchange_mode {
            get { return m_mailer_exchange_mode; }
            set { m_mailer_exchange_mode = value; }
        }
        public string Mailer_exchange_domain {
            get { return m_mailer_exchange_domain; }
            set { m_mailer_exchange_domain = value; }
        }
        public string Mailer_deliveryemail {
            get { return m_mailer_deliveryemail; }
            set { m_mailer_deliveryemail = value; }
        }
        public string Mailer_mailgun_api {
            get { return m_mailer_mailgun_api; }
            set { m_mailer_mailgun_api = value; }
        }
        public bool Mailer_use_mailgun {
            get { return m_mailer_use_mailgun; }
            set { m_mailer_use_mailgun = value; }
        }
        public bool Mailer_use_smtp {
            get { return m_mailer_use_smtp; }
            set { m_mailer_use_smtp = value; }
        }

    }


}
