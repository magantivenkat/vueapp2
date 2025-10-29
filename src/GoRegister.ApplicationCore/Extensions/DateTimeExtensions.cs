using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Extensions
{
    public static class DateTimeExtensions
    {
        public static List<SelectListItem> GetTimeZoneList(this List<SelectListItem> selectList)
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            List<SelectListItem> timeZoneList = new List<SelectListItem>();

            foreach (var timeZone in timeZones)
            {
                // Linux has mutiple variations of the same zonetimes 
                // The filter below is needed so that only 1 is displayed 
                var exists = selectList.SingleOrDefault(tz => tz.Text == timeZone.DisplayName);
                if (exists == null) selectList.Add(new SelectListItem() { Text = timeZone.DisplayName, Value = timeZone.Id });
            }

            return selectList;
        }

        public static List<SelectListItem> GetDateTimeFormatList(this List<SelectListItem> selectList)
        {
            selectList.Add(new SelectListItem("dd/MM/yyyy hh:mm tt", "dd/MM/yyyy hh:mm tt"));
            selectList.Add(new SelectListItem("yyyy-MM-dd HH:mm", "yyyy-MM-dd HH:mm"));

            return selectList;
        }

        public static string SetUserProfileDateTimeFormat(this DateTime dateTime, string timeZone, string dateFormat)
        {
            dateTime = dateTime.ConvertToUserProfileTimeZone(timeZone);

            var userProfileDateFormat = dateTime.ToString(dateFormat);

            dateTime = DateTime.ParseExact(userProfileDateFormat, dateFormat, null);

            return dateTime.ToString(dateFormat);
        }

        public static string SetUserProfileShortDateFormat(this DateTime dateTime, string timeZone, string dateFormat)
        {
            dateTime = dateTime.ConvertToUserProfileTimeZone(timeZone);

            var userProfileDateFormat = dateTime.ToString(dateFormat);

            dateTime = DateTime.ParseExact(userProfileDateFormat, dateFormat, null);

            return dateTime.ToString(dateFormat.Split(' ')[0]);
        }

        public static DateTime ConvertToUserProfileTimeZone(this DateTime dateTime, string timeZone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);

            return dateTime;
        }

        public static string HumanizedString(this string humanizedString, DateTime dateTime, string timeZone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);

            var currentDateTimeFromUserSettingsTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            var totalSeconds = (currentDateTimeFromUserSettingsTimeZone - dateTime).TotalSeconds;

            return DateTime.UtcNow.AddSeconds(-totalSeconds).Humanize();

        }

        public static string HumanizeDateTime(this DateTime dateTime, string timeZone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);

            var currentDateTimeFromUserSettingsTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            var totalSeconds = (currentDateTimeFromUserSettingsTimeZone - dateTime).TotalSeconds;

            var humanized = DateTime.UtcNow.AddSeconds(-totalSeconds).Humanize();

            return humanized;
        }

        public static string PrettyDate(this DateTime date)
        {
            return date.ToString("ddd dd MMM yyyy HH:MM tt").Insert(6, GetDaySuffix(date.Day));
        }

        private static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

    }
}
