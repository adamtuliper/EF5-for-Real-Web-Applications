using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace System.Web.Mvc
{
    public static class ListExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection) action(item);
            return collection;
        }

        public static SelectList ToSelectList(this DataTable dataTable, string dataValueField, string dataTextField)
        {
            var items = dataTable.AsEnumerable().AsQueryable().Select(o => new { Key = o[dataTextField], Value = o[dataValueField] });

            return new SelectList(items, "Key", "Value");
        }

        public static SelectList ToSelectList(this DataTable dataTable, string dataValueField, string dataTextField, string selectedValue)
        {
            var items = dataTable.AsEnumerable().AsQueryable().Select(o => new { Key = o[dataTextField], Value = o[dataValueField] });
            return new SelectList(items, "Key", "Value", selectedValue);
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection)
        {
            return new SelectList(collection, "Key", "Value");
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection, string selectedValue)
        {
            return new SelectList(collection, "Key", "Value", selectedValue);
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection,
                             string dataValueField, string dataTextField)
        {
            return new SelectList(collection, dataValueField, dataTextField);
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection,
                             string dataValueField, string dataTextField, string selectedValue)
        {
            return new SelectList(collection, dataValueField, dataTextField, selectedValue);
        }
    }

}