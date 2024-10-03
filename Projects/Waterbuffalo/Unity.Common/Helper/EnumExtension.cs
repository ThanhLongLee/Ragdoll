using Unity.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Unity.Common.Helper
{
    public static partial class EnumSelectList
    {
        public static List<SelectListItem> StatusList(Status[] removeObjects = null, object selected = null)
        {
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(Status)))
            {
                var text = "";
                var value = "";
                switch ((Status)item)
                {
                    case Status.Activated:
                        text = "Hoạt động";
                        value = ((int)(Status)item).ToString();
                        break;
                    case Status.Disabled:
                        text = "Ngưng hoạt động";
                        value = ((int)(Status)item).ToString();
                        break;
                    case Status.Undefined:
                        text = "Không xác định";
                        value = ((int)(Status)item).ToString();
                        break;
                }
                if (!text.IsEmpty() && !value.IsEmpty())
                {
                    items.Add(new SelectListItem
                    {
                        Text = text,
                        Value = value,
                        Selected = selected != null && value == ((int)(Status)selected).ToString()
                    });
                }
            }
            if (removeObjects != null && removeObjects.Any())
            {
                var valuesRemove = removeObjects.Select(x => ((int)x).ToString()).ToList();
                items = items.Where(x => !valuesRemove.Contains(x.Value)).ToList();
            }
            return items;
        }

        public static List<SelectListItem> AccountStatusList(AccountStatus[] removeObjects = null, object selected = null)
        {
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(AccountStatus)))
            {
                var text = "";
                var value = "";
                switch ((AccountStatus)item)
                {
                    case AccountStatus.Active:
                        text = "Hoạt động";
                        value = ((int)(AccountStatus)item).ToString();
                        break;
                    case AccountStatus.Locked:
                        text = "Khoá";
                        value = ((int)(AccountStatus)item).ToString();
                        break;
                    case AccountStatus.Disabled:
                        text = "Ngưng hoạt động";
                        value = ((int)(AccountStatus)item).ToString();
                        break;
                    case AccountStatus.Undefined:
                        text = "Tất cả";
                        value = ((int)(AccountStatus)item).ToString();
                        break;
                }
                if (!text.IsEmpty() && !value.IsEmpty())
                {
                    items.Add(new SelectListItem
                    {
                        Text = text,
                        Value = value,
                        Selected = selected != null && value == ((int)(AccountStatus)selected).ToString()
                    });
                }
            }
            if (removeObjects != null && removeObjects.Any())
            {
                var valuesRemove = removeObjects.Select(x => ((int)x).ToString()).ToList();
                items = items.Where(x => !valuesRemove.Contains(x.Value)).ToList();
            }
            return items;
        }

        public static List<SelectListItem> EditionStatusList(EditionStatus[] removeObjects = null, object selected = null)
        {
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(EditionStatus)))
            {
                var text = "";
                var value = "";
                switch ((EditionStatus)item)
                {
                    case EditionStatus.Activated:
                        text = "Đang hiển thị";
                        value = ((int)(Status)item).ToString();
                        break;
                    case EditionStatus.Disabled:
                        text = "Đang ẩn";
                        value = ((int)(Status)item).ToString();
                        break;
                    case EditionStatus.Undefined:
                        text = "Không xác định";
                        value = ((int)(Status)item).ToString();
                        break;
                }
                if (!text.IsEmpty() && !value.IsEmpty())
                {
                    items.Add(new SelectListItem
                    {
                        Text = text,
                        Value = value,
                        Selected = selected != null && value == ((int)(EditionStatus)selected).ToString()
                    });
                }
            }
            if (removeObjects != null && removeObjects.Any())
            {
                var valuesRemove = removeObjects.Select(x => ((int)x).ToString()).ToList();
                items = items.Where(x => !valuesRemove.Contains(x.Value)).ToList();
            }
            return items;
        }

    }

    public static partial class EnumExtension
    {
        public static string GetName(this Status value)
        {
            return EnumSelectList.StatusList().FirstOrDefault(x => x.Value == ((int)value).ToString())?.Text + "";
        }

        public static string GetName(this AccountStatus value)
        {
            return EnumSelectList.AccountStatusList().FirstOrDefault(x => x.Value == ((int)value).ToString())?.Text + "";
        }

        public static string GetColor(this Status value)
        {
            var color = "inherit";
            switch (value)
            {
                case Status.Activated:
                    color = "#28a745";
                    break;
                case Status.Disabled:
                    color = "#dc3545";
                    break;
            }
            return color;
        }

        public static string GetColor(this AccountStatus value)
        {
            var color = "inherit";
            switch (value)
            {
                case AccountStatus.Active:
                    color = "#28a745";
                    break;
                case AccountStatus.Locked:
                    color = "#dc3545";
                    break;
            }
            return color;
        }

        public static bool IsDisable(this AccountStatus value)
        {
            return value == AccountStatus.Disabled;
        }


        public static string GetName(this EditionStatus value)
        {
            return EnumSelectList.EditionStatusList().FirstOrDefault(x => x.Value == ((int)value).ToString())?.Text + "";
        }

        public static string GetColor(this EditionStatus value)
        {
            var color = "inherit";
            switch (value)
            {
                case EditionStatus.Activated:
                    color = "#28a745";
                    break;
                case EditionStatus.Disabled:
                    color = "#dc3545";
                    break;
            }
            return color;
        }

    }
}
