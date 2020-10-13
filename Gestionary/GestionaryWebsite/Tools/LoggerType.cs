using System;
using System.Linq;

namespace GestionaryWebsite
{

    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            // get all attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // FriendlyName is in a hidden Attribute class called DisplayAttribute
            //Must use dynamic because attributes is object[]
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return friendly name
            return displayAttribute ?? null;

        }
    }

    public static class LoggerTypeExtension
    {
        public static string GetFriendlyName(this LoggerType loggerType)
        {
            return loggerType.GetAttribute<FriendlyNameAttribute>().Name;
        }
    }

    internal class FriendlyNameAttribute : Attribute
    {
        public string Name;

        public FriendlyNameAttribute(string name)
        {
            Name = name;
        }
    }

    public enum LoggerType
    {
        [FriendlyName("None")]
        NONE = 0,
        [FriendlyName("Fatal")]
        FATAL,
        [FriendlyName("Error")]
        ERROR,
        [FriendlyName("Warning")]
        WARNING,
        [FriendlyName("Info")]
        INFO,
        [FriendlyName("None")]
        COUNT
    }

    
}
