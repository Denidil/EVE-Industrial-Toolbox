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
using System.Runtime.Serialization;

namespace EVE_Industrial_Toolbox
{
    
    [TypeConverter(typeof(API_Key_StoreSettingConverter))]
    //[Serializable()]
    public class API_Key_Store
    {
        public bool Active { get; set; } // whether or not we've marked this key active
        public int KeyID { get; set; }
        public ApiKeyType KeyType { get; set; }
        public string Description { get; set; } //char or corporation name
        public DateTime Expiration { get; set; } // key expiration date
        public string VerificationCode { get; set; }

        public API_Key_Store(int ID, string VCode, ApiKeyType Type, DateTime Expires, string Desc)
        {
            this.KeyID = ID;
            this.VerificationCode = VCode;
            this.KeyType = Type;
            this.Expiration = Expires;
            this.Description = Desc;
            this.Active = true;
        }

        public API_Key_Store()
        {

            this.KeyID = 0;
            this.KeyType = ApiKeyType.Account;
            this.Active = false;
        }


        /*public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("Active", Active, typeof(bool));
            info.AddValue("KeyID", KeyID, typeof(int));
            info.AddValue("KeyType", KeyType, typeof(ApiKeyType));
            info.AddValue("Description", Description, typeof(string));
            info.AddValue("Expiration", Expiration, typeof(DateTime));
            info.AddValue("VerificationCode", VerificationCode, typeof(string));

        }

        public API_Key_Store(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            Active = (bool)info.GetValue("Active", typeof(bool));
            KeyID = (int)info.GetValue("KeyID", typeof(int));
            KeyType = (ApiKeyType)info.GetValue("KeyType", typeof(ApiKeyType));
            Description = (string)info.GetValue("Description", typeof(string));
            Expiration = (DateTime)info.GetValue("Expiration", typeof(DateTime));
            VerificationCode = (string)info.GetValue("VerificationCode", typeof(string));
        }*/


        static public string APIKeyTypeToString(ApiKeyType type)
        {
            switch (type)
            {
                case ApiKeyType.Account:
                    return "Account";

                case ApiKeyType.Character:
                    return "Character";

                case ApiKeyType.Corporation:
                    return "Corporation";
            }

            return "";
        }

        static public ApiKeyType StringToApiKeyType(string str)
        {
            if (str == "Account")
                return ApiKeyType.Account;

            if (str == "Corporation")
                return ApiKeyType.Corporation;

            return ApiKeyType.Character;
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5}", Active, KeyID, API_Key_Store.APIKeyTypeToString(KeyType), Description, Expiration, VerificationCode);
        }
    }


    public class API_Key_StoreSettingConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if( sourceType == typeof(string) )
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if( stringValue != null )
            {
                // Obviously, using more robust parsing in production code is recommended.
                string[] parts = stringValue.Split(';');
                if( parts.Length == 6 )
                    return new API_Key_Store() { Active = Convert.ToBoolean(parts[0]), KeyID = int.Parse(parts[1]), KeyType = API_Key_Store.StringToApiKeyType(parts[2]), Description = parts[3], Expiration = DateTime.Parse(parts[4]), VerificationCode = parts[5] };
                else
                    throw new FormatException("Invalid API_Key_Store format");
            }
            else
                return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                API_Key_Store APIKS = value as API_Key_Store;
                return APIKS.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
