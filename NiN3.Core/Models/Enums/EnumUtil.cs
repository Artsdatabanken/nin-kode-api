/// <summary>
/// Provides utility methods for working with Enums.
/// </summary>
/// <returns>
/// The parsed enum value or the description of the enum value.
/// </returns>

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace NiN3.Core.Models.Enums
{
    public static class EnumUtil
    {
        /*
        public static T ParseEnum<T>(string value)
        {
            //todo-sat: Add logger to this class and logg the error-event to logfile.
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception ex)
            {
                //todo-sat: Add logger to this class and logg the error-event to logfile.
                Console.WriteLine($"ERROR: EnumParseError: Value '{value}' not found, returning default");
                return default(T);
            }
        }*/
        /*
        public static T? ParseEnum<T>(string value) where T : struct
        {
            if (TryParseEnum<T>(value, out T result))
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            else {
                return default(T);
            }
        }*/

            
            public static T? ParseEnum<T>(string value) where T : struct
            {
                //todo-sat: Add logger to this class and logg the error-event to logfile.
                if (TryParseEnum<T>(value, out T result))
                {
                    // The value is valid and the result variable contains the corresponding enumeration value.
                    //return result;
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                else
                {
                    // The value is not valid.
                    return null;
                }
            }

            /*
            public static T? ParseEnum<T>(string value) where T : struct
            {
                //todo-sat: Add logger to this class and logg the error-event to logfile.
                if (TryParseEnum<T>(value, out T result))
                {
                    // The value is valid and the result variable contains the corresponding enumeration value.
                    //return result;
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                else
                {
                    // The value is not valid.
                    return null;
                }
            }*/

            public static bool TryParseEnum<T>(string value, out T result) where T : struct
        {
            // Get all names of the enum
            var enumNames = Enum.GetNames(typeof(T));

            // Check if the value is among the names of the enum
            if (enumNames.Contains(value))
            {
                // The value is valid and the result variable contains the corresponding enumeration value.
                result = (T)Enum.Parse(typeof(T), value);
                return true;
            }
            else
            {
                // The value is not valid.
                //Console.WriteLine($"Warning : Value '{value}' not found, returning null");
                result = default;
                return false;
            }
        }

        /*
        public static bool TryParseEnum<T>(string value, out T result) where T : struct
        {
            //todo-sat: Add logger to this class and logg the error-event to logfile.
            if (Enum.TryParse<T>(value, out result))
            {
                // The value is valid and the result variable contains the corresponding enumeration value.
                return Enum.IsDefined(typeof(T), result);
            }
            else
            {
                // The value is not valid.
                return false;
            }
        }*/

        public static string ToDescription(this Enum value)
        {
            if (value!=null) { 
            try
            {
                var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return da.Length > 0 ? da[0].Description : value.ToString();
            }
            catch (Exception ex)
            {
                //todo-sat: Add logger to this class and logg the error-event to logfile.
                return null;
            }
            }
            else
            {
                return null;
            }
        }

        public static string ToDescriptionBlankIfNull(this Enum value)
        {
            try
            {
                if(value!=null) { 
                var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return da.Length > 0 ? da[0].Description : value.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //todo-sat: Add logger to this class and logg the error-event to logfile.
                return "";
            }
        }
    }
}

