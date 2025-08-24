using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SWSM.Extensions
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            DataTable table = new DataTable(typeof(T).Name);
            if (data == null || data.Count == 0)
                return table;

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                Type propType = prop.PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    propType = Nullable.GetUnderlyingType(propType);
                table.Columns.Add(prop.Name, propType);
            }

            foreach (T item in data)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
