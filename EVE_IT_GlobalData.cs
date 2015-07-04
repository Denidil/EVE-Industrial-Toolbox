using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;
using System.Configuration;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace EVE_Industrial_Toolbox
{

    public static class EVE_IT_GlobalData
    {
        public static ObservableCollection<API_Key_Store> API_Keys = new ObservableCollection<API_Key_Store>();

        public static string SerializeList<T>(List<T> items)
        {
            string Retval = "";

            for (int i = 0; i < items.Count; i++)
            {
                Retval += "{" + items[i].ToString() + "}";

                if (i < (items.Count - 1))
                    Retval += ",";
            }

            return Retval;
        }

        public static List<T> DeserializeList<T>(string serialized)
        {
            List<T> Retval = new List<T>();

            if (serialized.Length == 0)
                return Retval;


            string[] parts = serialized.Split(',');

            for (int i = 0; i < parts.Length; i++ )
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                if (parts[i].Substring(0,1) == "{" && converter.CanConvertFrom(serialized.GetType()))
                {
                    T val = (T)converter.ConvertFrom(parts[i].Substring(1, parts[i].Length - 2));
                    Retval.Add(val);
                }
                else
                {
                    throw new FormatException("Invalid List format");
                }

            }

            return Retval;
        }

        public static bool SerializeSettings()
        {
            Properties.Settings.Default.API_Key_List = EVE_IT_GlobalData.SerializeList<API_Key_Store>(EVE_IT_GlobalData.API_Keys.ToList());
            Properties.Settings.Default.Save();
            var path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            return true;
        }

        public static bool UnserializeSettings()
        {
            EVE_IT_GlobalData.API_Keys = new ObservableCollection<API_Key_Store>(EVE_IT_GlobalData.DeserializeList<API_Key_Store>(Properties.Settings.Default.API_Key_List));
            return true;
        }
    }


}
