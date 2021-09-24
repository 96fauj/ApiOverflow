using System;
using System.Linq;
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
