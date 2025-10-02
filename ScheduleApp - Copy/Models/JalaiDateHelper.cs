using System;
using System.Globalization;

namespace GuardScheduler.Models
{
    public static class JalaliDateHelper
    {
        private static readonly PersianCalendar _persian = new PersianCalendar();

        public static (int Year, int Month, int Day) ToJalali(DateTime g)
        {
            return (_persian.GetYear(g), _persian.GetMonth(g), _persian.GetDayOfMonth(g));
        }

        public static DateTime FromJalali(int jy, int jm, int jd)
        {
            return _persian.ToDateTime(jy, jm, jd, 0, 0, 0, 0);
        }

        public static string ToJalaliString(DateTime g)
            => $"{_persian.GetYear(g)}/{_persian.GetMonth(g):D2}/{_persian.GetDayOfMonth(g):D2}";
    }
}