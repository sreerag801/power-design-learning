using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace PowerToolLib
{
    public static class PowerToolHelpers
    {
        public static string CombineURL(this string part1, string part2)
        {
            return string.Concat
                (
                    part1.EndsWith("/") ? part1.Substring(0, part1.Length - 1) : part1,
                    "/",
                    part2.StartsWith("/") ? part2.Substring(1) : part2
                );
        }

        public static DataTable ConvetToDataTable<T>(this List<T> items)
        {
            var table = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
}
