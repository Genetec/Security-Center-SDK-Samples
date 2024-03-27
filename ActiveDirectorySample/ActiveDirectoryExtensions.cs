using System;
using System.DirectoryServices;
using System.Linq;
namespace ActiveDirectorySample
{
    public static class ActiveDirectoryExtensions
    {
        public static T GetValue<T>(this ResultPropertyCollection items, string propertyName)
        {
            Validate(items, propertyName);
            return (T)items[propertyName][0];
        }

        public static T GetValueOrDefault<T>(this ResultPropertyCollection items, string propertyName)
        {
            Validate(items, propertyName);
            return items.PropertyNames.Cast<string>().Contains(propertyName.ToLowerInvariant()) ?
                items.GetValue<T>(propertyName) :
                default(T);
        }

        private static void Validate(ResultPropertyCollection items, string propertyName)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException(nameof(propertyName));
        }
    }
}
