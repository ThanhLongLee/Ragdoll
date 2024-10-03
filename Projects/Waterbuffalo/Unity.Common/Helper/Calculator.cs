using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Configuration;

namespace Unity.Common.Helper
{
    public static class Calculator
    {
        public static int BeginRow(int pageNo, int itemPerPage = 0)
        {
            if (itemPerPage == 0)
                itemPerPage = AppSettings.ItemPerPage;

            return (pageNo - 1) * itemPerPage + 1;
        }

        public static int NumRows(int pageNo, int itemPerPage = 0)
        {
            if (itemPerPage == 0)
                itemPerPage = AppSettings.ItemPerPage;

            return (((pageNo - 1) * itemPerPage + 1) + itemPerPage) - 1;
        }

        public static double TotalPage(int totalRows)
        {
            return Math.Ceiling(1.0 * totalRows / AppSettings.ItemPerPage);
        }


        public static int DiscountPercent(decimal salePrice, decimal marketPrice)
        {
            if (salePrice != 0 && marketPrice != 0)
            {
                return (int)(((marketPrice - salePrice) * 100) / marketPrice);
            }

            return 0;
        }

        public static decimal RoundStar(this decimal value)
        {
            if (value % 1 > 0)
                return Math.Round(value, 1, MidpointRounding.AwayFromZero);

            return Math.Round(value, MidpointRounding.AwayFromZero);

        }
    }
}
