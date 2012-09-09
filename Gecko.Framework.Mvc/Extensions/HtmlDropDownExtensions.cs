using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace Gecko.Extensions
{
    public static class HtmlDropDownExtensions
    {
        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        public static IEnumerable<SelectListItem> EnumToSelectList<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            TypeConverter converter = TypeDescriptor.GetConverter(enumType);

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = converter.ConvertToString(value),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                };

            //Add an empty item if its nullable
            if (metadata.IsNullableValueType)
            {
                items = SingleEmptyItem.Concat(items);
            }

            return items;
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //Type enumType = GetNonNullableModelType(metadata);
            //IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            //TypeConverter converter = TypeDescriptor.GetConverter(enumType);

            //IEnumerable<SelectListItem> items =
            //    from value in values
            //    select new SelectListItem
            //    {
            //        Text = converter.ConvertToString(value),
            //        Value = value.ToString(),
            //        Selected = value.Equals(metadata.Model)
            //    };

            //if (metadata.IsNullableValueType)
            //{
            //    items = SingleEmptyItem.Concat(items);
            //}

            return htmlHelper.DropDownListFor(
                expression,
                EnumToSelectList(htmlHelper, expression)
                );
        }

    }
}