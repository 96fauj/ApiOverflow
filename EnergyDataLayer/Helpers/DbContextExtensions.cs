using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EnergyDataLayer.Helpers
{
    public static class DbContextExtensions
    {
        public static IList<object> GetPrimaryKeyValues<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            var primaryKeyNames = context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name).ToList();

            var valueList = new List<object>();

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                if (primaryKeyNames.Contains(propertyInfo.Name))
                {
                    valueList.Add(propertyInfo.GetValue(entity));
                }
            }

            return valueList;
        }

        public static Dictionary<object, object> GetPrimaryKeyValuesPairs<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            var primaryKeyNames = context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name).ToList();

            var valueDictionary = new Dictionary<object, object>();

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                if (primaryKeyNames.Contains(propertyInfo.Name))
                {
                    valueDictionary.Add(propertyInfo.Name, propertyInfo.GetValue(entity));
                }
            }

            return valueDictionary;
        }
    }
}
