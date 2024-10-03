using System;
using System.Configuration;
using System.Web;

namespace Unity.Common.Configuration
{
    public class AppSettings
    {
        public static bool _isDebug = bool.Parse(ConfigurationManager.AppSettings["IsDebug"]);

        public static char Delimiter
        {
            get { return '|'; }
        }

        public static string DomainUrl
        {
            get { return ConfigurationManager.AppSettings["DomainUrl"]; }
        }

        public static string Resources
        {
            get { return "Web.Resources"; }
        }

        public static string ImageHosting
        {
            get { return ConfigurationManager.AppSettings["ImageHosting"]; }
        }

        public static string DateFormat
        {
            get { return "dd/MM/yyyy"; }

        }

        public static string DateFormatLongTime
        {
            get { return "dd/MM/yyyy - HH:mm"; }

        }

        public static string CurrencyFormat
        {
            get { return "{0:n0}"; }
        }

        public static int ItemPerPage
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ItemPerPage"]); }
        }



        #region =====-- Resource Path

        public static string Common
        {
            get { return "Common/"; }
        }


        public static string TempPath
        {
            get { return "Resources/Temp/"; }
        }


        public static string ContentPath
        {
            get { return "content/"; }

        }

        public static string ThumbnailPath
        {
            get { return "thumbnail/"; }

        }
        public static string BannerPath
        {
            get { return "banner/"; }

        }
        #endregion


        #region =========-- CKFinder
        public static string SessionCKEditor
        {
            get { return "UserLogin"; }
        }

        #endregion




        #region ======-- Email Config

        public static string FromEmailAddress
        {
            get { return ConfigurationManager.AppSettings["FromEmailAddress"]; }
        }

        public static string SMTPPassword
        {
            get { return ConfigurationManager.AppSettings["SMTPPassword"]; }
        }

        public static string SMTPUsername
        {
            get { return ConfigurationManager.AppSettings["SMTPUsername"]; }
        }

        public static string SMTPHost
        {
            get { return ConfigurationManager.AppSettings["SMTPHost"]; }
        }

        public static int SMTPPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["SMTPPort"]); }
        }

        public static bool EnabledSSL
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"]); }
        }

        #endregion Email Config


    }
}
