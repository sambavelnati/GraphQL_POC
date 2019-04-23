using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace EFCore3Sample{
    public class DateUtil
    {
        /// <summary>
        /// Converts passed string in BALM date format to
        /// datetime data
        /// </summary>
        /// <param name="dtStr">date string in BALM date format</param>
        /// <returns>date string converted to datetime data</returns>
        public static DateTime ToDate(string dtStr)
        {
            string _DtFmt = "DD-MM-yyyy";
            DateTime _RetDate;

            _RetDate = DateTime.ParseExact(dtStr, _DtFmt, new CultureInfo("en-GB"));

            return _RetDate;
        }

        public static DateTime ToDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static string ToDateStr(DateTime date, string dateFormat)
        {
            return date.ToString(dateFormat);
        }

        public static string ToDateStr(DateTime date)
        {
            string _DtFmt = "DD-MM-yyyy";

            return date.ToString(_DtFmt);
        }

        //for date time input/ display
        public static DateTime ToDateTime(string dateTime)
        {
            string format = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern;
            DateTime RetDate;

            RetDate = DateTime.ParseExact(dateTime, format, null);

            return RetDate;
        }

        public static DateTime ToDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public static string ToDateTimeStr(DateTime dateTime, string dateTimeFormat)
        {
            return dateTime.ToString(dateTimeFormat);
        }

        //for converting to DB date format
        public static string ToDBDate(DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
        }

        //for use in search queries
        public static DateTime ToEndDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        //for converting to timespan time format
        // by default 0 days
        public static TimeSpan ToTime(DateTime date)
        {
            return new TimeSpan(0, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static string GetStrDBMaxDate(List<string> lstDates)
        {
            string MaxDate = ToDateStr((from d in lstDates select ToDate(d)).Max());
            return MaxDate;
        }

        public static string GetStrDBMinDate(List<string> lstDates)
        {
            string MinDate = ToDateStr((from d in lstDates select ToDate(d)).Min());
            return MinDate;
        }

        public static List<string> GetStrDBDatesSortDesc(List<string> dates)
        {
            List<string> datesOrderByDesc = (dates.OrderByDescending(date => ToDate(date))).ToList();
            return datesOrderByDesc;
        }

        public static List<string> GetStrDBDatesSortAsc(List<string> dates)
        {
            List<string> datesOrderByAsc = (dates.OrderBy(date => ToDate(date))).ToList();
            return datesOrderByAsc;
        }

        public static string[] GetStrDBDatesMonthEnds(List<string> dates)
        {
            DateTime[] date = dates.Select(_dt => ToDate(_dt)).ToArray();
            string[] endMonthDate;

            var result = (from _dt in date
                          group _dt by new { month = _dt.Month, year = _dt.Year } into grp
                          select new { Dates = grp.OrderByDescending(d => d.Date).FirstOrDefault() });

            endMonthDate = result.Select(_dt => ToDateStr(_dt.Dates)).ToArray();

            return endMonthDate;
        }
    }
}