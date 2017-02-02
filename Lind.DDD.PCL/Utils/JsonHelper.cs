using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Reflection;
using Lind.DDD.Paging;

/// <summary>
///JsonHelper 的摘要说明
/// </summary>
namespace Lind.DDD.Utils
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
        }


        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>  
        public static string ToJson(this object obj)
        {
            try
            {
                if (null == obj)
                    return null;

                if (obj.GetType() == typeof(System.Data.DataTable))
                {
                    System.Data.DataTable _sourceTable = (System.Data.DataTable)obj;
                    foreach (System.Data.DataRow row in _sourceTable.Rows)
                    {

                        foreach (System.Data.DataColumn column in _sourceTable.Columns)
                        {

                            if (column.DataType == typeof(System.Int32))
                            {
                                if (row[column] == DBNull.Value)
                                {

                                    row[column.ColumnName] = 0;
                                }
                            }
                            if (column.DataType == typeof(System.Decimal))
                            {

                                if (row[column] == DBNull.Value)
                                {
                                    row[column.ColumnName] = 0M;
                                }

                            }

                        }
                    }
                    _sourceTable.AcceptChanges();
                    return JsonConvert.SerializeObject(_sourceTable, Formatting.None, _jsonSettings);

                }
                else
                {
                    if (obj.GetType().Name == "PagedList`1")
                    {
                        var page = obj as IPagedList;
                        return JsonConvert.SerializeObject(new
                        {
                            Model = obj,
                            PageIndex = page.PageIndex,
                            PageSize = page.PageSize,
                            TotalCount = page.TotalCount,
                            TotalPages = page.TotalPages
                        }, Formatting.None, _jsonSettings);
                    }
                    else
                    {

                        return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
                    }
                }


            }
            catch (Exception)
            {
                return null;

            }


        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception)
            {

                return default(T);
            }
        }

        /// <summary>
        /// 功能:集合按需要序列化
        /// author:仓储大叔
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, List<string> param)
        {

            if (obj.GetType().Name == "List`1" || obj.GetType().Name == "PagedList`1")
            {
                //分页集合序列化
                if (obj.GetType().Name == "PagedList`1")
                {
                    var page = obj as IPagedList;
                    foreach (var t in (IEnumerable<object>)obj)
                    {
                        GeneratorJson(t, param);
                    }
                    return JsonConvert.SerializeObject(new
                    {
                        Model = obj,
                        PageIndex = page.PageIndex,
                        PageSize = page.PageSize,
                        TotalCount = page.TotalCount,
                        TotalPages = page.TotalPages
                    }, Formatting.None, _jsonSettings);
                }
                else
                {
                    foreach (var t in (IEnumerable<object>)obj)
                    {
                        GeneratorJson(t, param);
                    }
                }
            }

            else
            {
                GeneratorJson(obj, param);
            }


            return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
        }
        /// <summary>
        /// 为对象生成Json字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="t"></param>
        /// <param name="param"></param>
        static void GeneratorJson(object t, List<string> param)
        {
            var pList = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .Where(i => i.Name != "Capacity" && i.Name != "Count" && i.Name != "Item");
            foreach (var item in pList)
            {

                if (item != null && !param.Contains(item.Name, new IgnorEqualityComparer()) && item.CanWrite)
                {

                    item.SetValue(t, null);
                }
            }

        }

    }

    public class IgnorEqualityComparer : IEqualityComparer<string>
    {

        #region IEqualityComparer<string> 成员

        public bool Equals(string x, string y)
        {
            x = x ?? string.Empty;
            y = y ?? string.Empty;
            return x.ToLower() == y.ToLower();
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}