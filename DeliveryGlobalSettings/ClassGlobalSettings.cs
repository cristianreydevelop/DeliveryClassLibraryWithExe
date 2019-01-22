using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DeliveryGlobalSettings
{
    public static class ClassGlobalSettings
    {
        private static string CLASS_NAME = System.Reflection.Assembly.GetExecutingAssembly().ToString() + "/DeliveryData.ClassData";

        public static string GetConnString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DeliverySystem"].ToString();
            }
        }

        public struct DELIVERY_TYPES
        {
            public static string EMAIL
            {
                get
                {
                    return "EMAIL";
                }
            }

            public static string FTP
            {
                get
                {
                    return "FTP";
                }
            }

            public static string SFTP
            {
                get
                {
                    return "SFTP";
                }
            }

            public static string HTTP
            {
                get
                {
                    return "HTTP";
                }
            }

            public static string HTTPS
            {
                get
                {
                    return "HTTPS";
                }
            }
        }

        public static string TestEmailUname
        { 
            get
            { 
                return ConfigurationManager.AppSettings["TEST_EMAIL_UNAME"];
            }
        }

        public static string TestEmailPwd
        { 
            get
            { 
                return ConfigurationManager.AppSettings["TEST_EMAIL_PWD"];
            }
        }    
    }
}
