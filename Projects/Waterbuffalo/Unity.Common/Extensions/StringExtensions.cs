using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Common.Configuration;

namespace Unity.Common.Extensions
{
    public static class StringExtends
    {

        public static Guid ToGuid(this string value)
        {
            try
            {
                return Guid.Parse(value);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static DataTable ToDataTable(this string values)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("id", typeof(int)) });

            foreach (string id in values.Split(','))
            {
                if (!String.IsNullOrEmpty(id.Trim()) && id.ToNumber() > 0)
                    dt.Rows.Add(id.Trim());
            }

            return dt;
        }

        public static DateTime ToDateTime(this string value, string format)
        {
            if (String.IsNullOrEmpty(value))
                return new DateTime(1900, 01, 01);

            return DateTime.ParseExact(value, format, null);
        }
        
        public static double ToNumber(this string value)
        {
            double num = 0;
            double.TryParse(value, out num);

            return num;
        }

        public static int ToInt(this string value)
        {
            int num = 0;
            int.TryParse(value, out num);

            return num;
        }

        public static long ToLong(this string value)
        {
            long num = 0;
            long.TryParse(value, out num);

            return num;
        }

        public static decimal ToDecimal(this string value)
        {
            decimal num = 0;
            decimal.TryParse(value, out num);

            return num;
        }

        public static string ToFilePath(this string file, string folder)
        {
            return string.IsNullOrEmpty(file) ? "" : AppSettings.ImageHosting + folder + file;
        }

        public static string ToPathThumbnail(this string file)
        {
            return string.IsNullOrEmpty(file) ? AppSettings.ImageHosting + AppSettings.Common + "no-image.jpg" : file;
        }

        public static bool IsDate(this string value, string format)
        {
            try
            {
                DateTime.ParseExact(value, format, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNumber(this string value)
        {
            try
            {
                double.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEmail(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;

            try
            {
                return Regex.IsMatch(value,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsMobileNumber(this string value)
        {
            if (value != null)
            {
                var arrNum = value.ToString().Select(c => c.ToString()).ToArray();
                var firstNum = (int)arrNum[0].ToNumber();
                var secondNum = (int)arrNum[1].ToNumber();

                if (firstNum == 0 && arrNum.Count() == 10)
                {
                    switch (secondNum)
                    {
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 9:
                            return true;
                    }
                }
            }

            return false;
        }

        public static string RemoveUnicode(this string value)
        {
            string strFormD = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            string str = "";
            for (int i = 0; i <= strFormD.Length - 1; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(strFormD[i]);
                if (uc == UnicodeCategory.NonSpacingMark == false)
                {
                    if (strFormD[i] == 'đ')
                        str = "d";
                    else if (strFormD[i] == 'Đ')
                        str = "D";
                    else
                        str = strFormD[i].ToString();

                    sb.Append(str);
                }
            }

            return sb.ToString().ToLower();
        }

        public static string RemoveUnicodeUrl(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return String.Empty;

            string strFormD = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            string str = "";
            for (int i = 0; i <= strFormD.Length - 1; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(strFormD[i]);
                if (uc == UnicodeCategory.NonSpacingMark == false)
                {
                    if (strFormD[i] == 'đ')
                        str = "d";
                    else if (strFormD[i] == 'Đ')
                        str = "D";
                    else
                        str = strFormD[i].ToString();

                    sb.Append(str);
                }
            }//Change 
            var s = Regex.Replace(sb.ToString(), @"[^0-9a-zA-Z\s\/\\]+", "")
                       .Replace("/", "-")
                       .Replace("\\", "-")
                       .Replace("  ", "-")
                       .Replace(" ", "-")
                       .Replace(System.Environment.NewLine, "");

            return s.ToLower();
        }

        public static string KeywordContains(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            var keyword = value.RemoveUnicode().TrimEmpty();

            if (string.IsNullOrEmpty(keyword)) return keyword;

            var arrWord = keyword.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            if (arrWord.Length > 1)
                keyword = string.Join(" AND ", arrWord);

            return keyword;
        }

        public static string TrimEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input) ? input.Trim() : input;
        }

        public static string ToLowerEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input) ? input.ToLower() : input;
        }

        public static string SplitCode(this string value)
        {
            var code = value;
            if (string.IsNullOrEmpty(code)) return code;

            var index = code.IndexOf("_");
            if (index != -1)
            {
                code = code.Substring(index + 1, (code.Length - 1) - index);
            }
            return code;
        }

        public static string GetFirstLetter(this string value)
        {
            var letter = String.Empty;
            if (string.IsNullOrEmpty(value)) return letter;

            var firstWord = value.Split(' ');
            if (!firstWord.Any()) return letter;

            if (firstWord.Last().Length > 0)
                letter = firstWord.Last().Substring(0, 1);
            return letter;
        }

        public static string WithCountryCode(this string phoneNumber, string countryCode)
        {
            try
            {
                int firstChar = int.Parse(phoneNumber.GetFirstLetter());
                if (firstChar == 0)
                    return countryCode + phoneNumber.Remove(0, 1);
            }
            catch { }

            return countryCode + phoneNumber;
        }

        public static bool IsGuid(this string value)
        {
            return Guid.TryParse(value, out _);
        }
    }
}
