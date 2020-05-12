using Dapper;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlankApiModel.Extension
{
    /// <summary>
    /// Extension method to make querys in database with dapper
    /// </summary>
    public static class DapperExtension
    {
        /// <summary>
        /// Can update one object or a list of objects of <typeparamref name="T"/>
        /// Make update in database with tables that contains params of <typeparamref name="T"/> object
        /// <typeparamref name="T"/> must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="KeyAttribute"/>
        /// ForeignKey must constains just <see cref="ForeignKeyAttribute"/>
        /// </summary>
        /// <param name="connection">Type of <see cref="NpgsqlConnection"/></param>
        public async static Task<bool> UpdatObject<T>(this IDbConnection connection, T obj)
        {
            try
            {
                if (obj is IList)
                {
                    foreach (object o in (obj as IList))
                    {
                        await connection.QueryAsync(UpdateFormat(o));
                    }
                }
                else
                {
                    await connection.QueryAsync(UpdateFormat(obj));
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Create sql query to <see cref="UpdatObject{T}(NpgsqlConnection, T)"/>
        /// This ignore items by <see cref="IsDefaultOrIgnore(PropertyInfo, object)"/>
        /// The query is: update <see cref="TableAttribute"/> set attributes (name1=value1,name2,value2) where id = <see cref="KeyAttribute"/>
        /// Get <see cref="ColumnAttribute"/> names by <see cref="GetPropertyDataBaseName(PropertyInfo)"/>
        /// Get value of parameters by <see cref="TransformValueWhenString(PropertyInfo, object)"/>
        /// </summary>
        private static string UpdateFormat(object obj)
        {
            var id = obj.GetType()
                        .GetProperties()
                        .SingleOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null)
                        .GetValue(obj, null);

            var props = obj.GetType().GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() == null).ToList();

            var name = obj.GetType().GetCustomAttributes<TableAttribute>().Select((TableAttribute table) => table.Name).SingleOrDefault();

            var valuesParams = $"{props.Select(x => $"{x.GetPropertyDataBaseName()}={x.TransformValueWhenString(obj)}").Aggregate((x, y) => $"{x},{y}")}";

            return $"update {name} set {valuesParams} where id = {id}";
        }

        /// <summary>
        /// Can insert one object or a list of objects of <typeparamref name="T"/>
        /// Make insert in database with tables that contains params of <typeparamref name="T"/> object
        /// <typeparamref name="T"/> must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="KeyAttribute"/>
        /// ForeignKey must constains just <see cref="ForeignKeyAttribute"/>
        /// </summary>
        /// <param name="connection">Type of <see cref="NpgsqlConnection"/></param>
        /// <returns>Return a object that contains the id inserted if has item in <paramref name="obj"/> or return a list of ids inseted if contains a list in <paramref name="obj"/></returns>
        public async static Task<object> InsertObject<T>(this IDbConnection connection, T obj)
        {
            var listIds = new List<int>();

            try
            {
                if (obj is IList)
                {
                    foreach (object o in (obj as IList))
                    {
                        var item = await connection.QueryAsync<int>(InsertFormat(o));
                        listIds.Add(item.Single());
                    }
                }
                else
                {
                    var item = await connection.QueryAsync<int>(InsertFormat(obj));
                    return item.Single();
                }

                return listIds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<int>();
            }
        }

        /// <summary>
        /// Create sql query to <see cref="InsertObject{T}(NpgsqlConnection, T)"/>
        /// This ignore items by <see cref="IsDefaultOrIgnore(PropertyInfo, object)"/>
        /// The query is: insert into <see cref="TableAttribute"/> (list of names in <see cref="ColumnAttribute"/>) values (values in items that contains <see cref="ColumnAttribute"/>) RETURNING id
        /// Get <see cref="ColumnAttribute"/> names by <see cref="GetPropertyDataBaseName(PropertyInfo)"/>
        /// Get value of parameters by <see cref="TransformValueWhenString(PropertyInfo, object)"/>
        /// </summary>
        public static string InsertFormat(object obj)
        {
            var props = obj
                        .GetType()
                        .GetProperties()
                        .Where(x => !x.IsDefaultOrIgnore(x.GetValue(obj, null)))
                        .ToList();

            var valuesParams = $"({props.Select(x => x.GetPropertyDataBaseName()).Aggregate((x, y) => $"{x},{y}")})";
            var listValues = $"({props.Select(x => x.TransformValueWhenString(obj)).Aggregate((x, y) => $"{x},{y}")})";

            var name = obj.GetType().GetCustomAttributes<TableAttribute>().Select((TableAttribute table) => table.Name).SingleOrDefault();

            return $"insert into {name} {valuesParams} values {listValues}; select max(id) from {name};";
        }

        /// <summary>
        /// Get value of propety
        /// If is string return the value with 'value'
        /// </summary>
        public static object TransformValueWhenString(this PropertyInfo property, object obj)
        {
            if (property.GetValue(obj, null).GetType().Equals(typeof(string)))
            {
                return $"\'{property.GetValue(obj, null)}\'";
            }

            return property.GetValue(obj, null);
        }

        /// <summary>
        /// Get the <see cref="ColumnAttribute"/> name
        /// </summary>
        public static string GetPropertyDataBaseName(this PropertyInfo property)
        {
            var customAttribute = property.GetCustomAttributes<Attribute>()?.SingleOrDefault();
            var columName = (customAttribute as object)?.GetType().GetProperty("Name")?.GetValue(customAttribute, null);
            if (columName != null)
                return columName.ToString();

            return property.Name;
        }

        /// <summary>
        /// Verify if the <paramref name="value"/> is a ignorecase by <see cref="IsDefaultValue(PropertyInfo, object)"/> and <see cref="IsIgnoredParameter(PropertyInfo)"/>
        /// </summary>
        public static bool IsDefaultOrIgnore(this PropertyInfo property, object value)
        {
            if (property.IsDefaultValue(value))
                return true;

            return property.IsIgnoredParameter();
        }

        /// <summary>
        /// Verify if the <paramref name="property"/> contains the <see cref="NotMappedAttribute"/>
        /// </summary>
        public static bool IsIgnoredParameter(this PropertyInfo property)
        {
            return property
                    .GetCustomAttributes<NotMappedAttribute>()
                    ?.Any() ?? false;
        }

        /// <summary>
        /// Verify if the <paramref name="property"/> is a default param if not is type of <see cref="Boolean"/>
        /// </summary>
        public static bool IsDefaultValue(this PropertyInfo property, object value)
        {
            if (property.PropertyType == typeof(Boolean))
                return false;

            var defaultValue = property.GetDefaultValueForProperty();

            try
            {
                return value.Equals(defaultValue);
            }
            catch
            {
                return value == defaultValue;
            }

        }

        /// <summary>
        /// Used by <see cref="IsDefaultValue(PropertyInfo, object)"/> to get the default value of the type of <paramref name="property"/>
        /// </summary>
        public static object GetDefaultValueForProperty(this PropertyInfo property)
        {
            var defaultAttr = property.GetCustomAttribute(typeof(DefaultValueAttribute));
            if (defaultAttr != null)
                return (defaultAttr as DefaultValueAttribute).Value;

            var propertyType = property.PropertyType;
            return propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
        }


    }
}
