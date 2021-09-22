using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvApp.Business.Interfaces;
using CsvHelper.Configuration;

namespace CsvApp.Business.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsvIdentifierAttribute : Attribute
    {
    }

    public static class CsvIdentifierAttributeExtensions
    {
        public static object GetIdentifier(this IUniqueCsvEntity entity)
        {
            //var prop = typeof(IUniqueCsvEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(CsvIdentifierAttribute), false).Count() == 1);
            //object ret = prop != null ? prop.GetValue(entity, null) : null;

            var property = entity.GetType()
                .GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(CsvIdentifierAttribute)))
                .FirstOrDefault();

            if (property == null)
            {
                throw new ConfigurationException("Entity does not have a property decorated with the correct csv identifier");
            }

            return property.GetValue(entity);
        }
    }
}
