using Unity.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Unity.Common.Extensions
{
    public static class DropdownExtensions
    {

        public static void InsertByIndex(this List<SelectListItem> origList, int index, SelectListItem item)
        {
            origList.Insert(index, item);
        }
    }
}
