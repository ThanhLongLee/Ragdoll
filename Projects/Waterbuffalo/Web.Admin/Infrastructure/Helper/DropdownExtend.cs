using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Admin.Infrastructure.Helper
{

    public static class DropdownExtend
    {
        public static List<SelectListItem> AppendItemByIndex(this List<SelectListItem> listSelectListItem, string text,
            string value, int index, bool selected)
        {
            listSelectListItem.Insert(index, new SelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selected
            });
            return listSelectListItem;
        }

        public static IEnumerable<SelectListItem> AppendItemByIndex(this IEnumerable<SelectListItem> datas, string text,
            string value, int index, bool selected)
        {
            var list = datas.ToList();

            list.Insert(index, new SelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selected
            });

            var listEnumerable = list.Select(i => new SelectListItem()
            {
                Text = i.ToString(),
                Value = i.ToString(),
                Selected = i.Selected
            });

            return listEnumerable;
        }
    }
}