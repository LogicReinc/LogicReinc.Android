using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Newtonsoft.Json;

namespace LogicReinc.Android
{
    public class Settings<T> where T : Settings<T>, new()
    {
        private static string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settings");
        private static T current = null;
        public static T Current
        {
            get
            {
                if (current == null)
                    current = Settings<T>.LoadSettings();
                return current;
            }
        }

        public static T LoadSettings()
        {
            if (System.IO.File.Exists(path))
            {
                return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
            }
            else
                return new T();
        }

        public void SaveSettings()
        {
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }
    }
}