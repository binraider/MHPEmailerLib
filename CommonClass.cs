using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;


namespace MHPEmailerLib{


    internal static class EMLHelpers {

        internal static bool IsValidEmail(string s) {
            if(s.Length > 0) {
                return s.Contains('@');
            } else {
                return false;
            }
        }

        internal static ExchangeVersion GetExchangeVersion(string ver) {
            ExchangeVersion outy = ExchangeVersion.Exchange2007_SP1;
            switch(ver.Trim().ToLower()) {
                case "Exchange2007_SP1":
                    outy = ExchangeVersion.Exchange2007_SP1;
                    break;
                case "Exchange2010":
                    outy = ExchangeVersion.Exchange2010;
                    break;
                case "Exchange2010_SP1":
                    outy = ExchangeVersion.Exchange2010_SP1;
                    break;
                case "Exchange2010_SP2":
                    outy = ExchangeVersion.Exchange2010_SP2;
                    break;
                case "Exchange2013":
                    outy = ExchangeVersion.Exchange2013;
                    break;
            
            }
            return outy;
        }

        internal static string SubstituteFields(string text, EMLMailTarget target) {
            string outy = text;
            if(target.Field1.Length > 0) {
                outy = outy.Replace("[FIELD1]", target.Field1);
            }
            if(target.Field2.Length > 0) {
                outy = outy.Replace("[FIELD2]", target.Field2);
            }
            if(target.Field3.Length > 0) {
                outy = outy.Replace("[FIELD3]", target.Field3);
            }
            if(target.Field4.Length > 0) {
                outy = outy.Replace("[FIELD4]", target.Field4);
            }
            return outy;
        }

    }


    public class EMLReturnClass {

        private bool m_success = true;
        private string m_message = "";
        private string m_techmessage = "";
        private List<string> m_errors = null;


        public EMLReturnClass() {
            m_errors = new List<string>();
        }

        public EMLReturnClass(bool _success, string _message, string _techmessage) {
            m_success = _success;
            m_message = _message;
            m_techmessage = _techmessage;
            m_errors = new List<string>();
        }
        public bool Success {
            get { return m_success; }
            set { m_success = value; }
        }
        public string Message {
            get { return m_message; }
            set { m_message = value; }
        }
        public string Techmessage {
            get { return m_techmessage; }
            set { m_techmessage = value; }
        }
        public void SetFailureMessage(string m) {
            m_success = false;
            m_message = m;
        }
        public void SetFailureMessage(string m, string m2) {
            m_success = false;
            m_message = m;
            m_techmessage = m2;
        }
        public string Messages {
            get {
                return m_message + " " + m_techmessage;
            }
        }

        public List<string> Errors {
            get { return m_errors; }
            set { m_errors = value; }
        }
    }

    public class EMLMailTarget {

        private string m_name = "";
        private string m_emailaddress = "";
        private string m_field1 = "";
        private string m_field2 = "";
        private string m_field3 = "";
        private string m_field4 = "";

        public EMLMailTarget() {
        }
        public EMLMailTarget(string _name, string _emailaddress) {
            m_name = _name;
            m_emailaddress = _emailaddress;
        }
        public EMLMailTarget(string _name, string _emailaddress, string _field1, string _field2, string _field3, string _field4) {
            m_name = _name;
            m_emailaddress = _emailaddress;
            m_field1 = _field1;
            m_field2 = _field2;
            m_field3 = _field3;
            m_field4 = _field4;

        }
        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }
        public string EmailAddress {
            get { return m_emailaddress; }
            set { m_emailaddress = value; }
        }
        public string Field1 {
            get { return m_field1; }
            set { m_field1 = value; }
        }
        public string Field2 {
            get { return m_field2; }
            set { m_field2 = value; }
        }
        public string Field3 {
            get { return m_field3; }
            set { m_field3 = value; }
        }
        public string Field4 {
            get { return m_field4; }
            set { m_field4 = value; }
        }

    }

}
