using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;


namespace PopcornRemoteTest1
{
    public class Settings
    {
        private const string ServerIPKey = "ServerIP";
        private const string TimeOutKey = "TimeOut";
        private const string Allow3GKey = "Allow3G";
        private const string PortKey = "Port";
        private T RetrieveSetting<T>(string settingKey)
        {
            object settingValue;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(settingKey, out settingValue))
                return (T)settingValue;
            //Do other shit to test git
            return default(T);
        }

        public string ServerIP
        {
            get {
                
                return RetrieveSetting<string>(ServerIPKey);
            }
            set
            {
                string ip=value;

                if (System.Uri.IsWellFormedUriString(ip, UriKind.RelativeOrAbsolute))
                {
                    ip = ip.TrimStart("http://".ToCharArray());
                    IsolatedStorageSettings.ApplicationSettings[ServerIPKey] = ip;
                }
                else
                {
                   MessageBox.Show("Not a valid IP or URL address."); 
                }
               
            }
        }

        public int TimeOut
        {
            get
            {
                return RetrieveSetting<int>(TimeOutKey);
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[TimeOutKey] = value;
            }
        }

        public bool Allow3G
        {
            get
            {
                return RetrieveSetting<bool>(Allow3GKey);
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[Allow3GKey] = value;
            }
        }

        public int Port
        {
            get
            {
                int result = RetrieveSetting<int>(PortKey);
                if (result == 0 || result==null)
                    result = 9999;
                return result ;
            }
            set
            {
                if (value > 0 && value < 65536)
                    IsolatedStorageSettings.ApplicationSettings[PortKey] = value;
                else
                {
                    MessageBox.Show("Port should be between 1 and 65535");
                    IsolatedStorageSettings.ApplicationSettings[PortKey] = 9999;
                 }
            }
        }

    }
}
