using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestTrack.Models;

namespace TestTrack.Helpers
{
    public class SessionWrapper
    {
        public SessionWrapper()
        {
            // To avoid null references.
            if (this.UserSettings == null)
            {
                this.UserSettings = new UserSettings();
            }
        }

        private T GetFromSession<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        private void SetInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public UserSettings UserSettings
        {
            get { return GetFromSession<UserSettings>("UserSettings"); }
            set { SetInSession("UserSettings", value); }
        }
    }
}