using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;

namespace Gecko.Framework.Mvc.Extensions
{
    public class EnumExtensions
    {
        //dont forget about the html helper methods I have too in the HtmlExtensions class for enums as well.

        public static IEnumerable<SelectListItem> EnumToSelectList<TEnum>(System.Enum enumToConvert, bool isNullable)
        {
            IEnumerable<TEnum> values = Enum.GetValues(enumToConvert.GetType()).Cast<TEnum>();

            TypeConverter converter = TypeDescriptor.GetConverter(enumToConvert);

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = converter.ConvertToString(value),
                    Value = value.ToString(),
                    Selected = value.Equals(enumToConvert)
                };

            //Add an empty item if its nullable
            if (isNullable)
            {
                SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };
                items = SingleEmptyItem.Concat(items);
            }

            return items;
        }
    }
}
