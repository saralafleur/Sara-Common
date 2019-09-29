using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Sara.Common.Extension
{
    public static class DataTableExt
    {
        public static DataTable ObjectToDataTable<T>(T o) where T : class
        {
            var classType = typeof(T);

            var propertyList = classType.GetProperties().ToList();
            if (propertyList.Count < 1)
            {
                return new DataTable();
            }

            var className = classType.UnderlyingSystemType.Name;
            var result = new DataTable(className);
            result.Columns.Add("Name");
            result.Columns.Add("Value");

            foreach (var property in propertyList)
            {
                result.Rows.Add(property.Name, o.GetType().GetProperty(property.Name).GetValue(o, null));
            }

            return result;
        }
        public static DataTable ClassToDataTable<T>() where T : class
        {
            var classType = typeof(T);

            var propertyList = classType.GetProperties().ToList();
            if (propertyList.Count < 1)
            {
                return new DataTable();
            }

            var className = classType.UnderlyingSystemType.Name;
            var result = new DataTable(className);

            foreach (var property in propertyList)
            {
                var col = new DataColumn { ColumnName = property.Name };

                var dataType = property.PropertyType;

                if (IsNullable(dataType))
                    if (dataType.IsGenericType)
                        dataType = typeof(string);
                else
                    col.AllowDBNull = false;

                col.DataType = dataType;

                result.Columns.Add(col);
            }

            return result;
        }

        public static DataTable ClassListToDataTable<T>(List<T> classList) where T : class
        {
            var result = ClassToDataTable<T>();

            if (result.Columns.Count < 1)
            {
                return new DataTable();
            }
            if (classList.Count < 1)
            {
                return result;
            }

            foreach (T item in classList)
            {
                ClassToDataRow(ref result, item);
            }

            return result;
        }

        public static void ClassToDataRow<T>(ref DataTable table, T data) where T : class
        {
            var classType = typeof(T);
            var className = classType.UnderlyingSystemType.Name;

            if (!table.TableName.Equals(className))
            {
                return;
            }

            var row = table.NewRow();
            var propertyList = classType.GetProperties().ToList();

            foreach (var prop in propertyList)
            {
                if (!table.Columns.Contains(prop.Name)) continue;
                if (table.Columns[prop.Name] != null)
                {
                    row[prop.Name] = prop.GetValue(data, null);
                }
            }
            table.Rows.Add(row);
        }

        public static bool IsNullable(Type input)
        {
            if (!input.IsValueType) return true; // Is a ref-type, such as a class
            return Nullable.GetUnderlyingType(input) != null;
        }
        public static void ToCsv(this DataTable dtDataTable, string strFilePath)
        {
            var sw = new StreamWriter(strFilePath, false);
            //headers  
            for (var i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                    sw.Write(",");
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (var i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        var value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = $"\"{value}\"";
                            sw.Write(value);
                        }
                        else
                            sw.Write(dr[i].ToString());
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

    }
}
