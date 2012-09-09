using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gecko.Framework.Mvc.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UnsecuredActionAttribute : Attribute
    {
    }
}