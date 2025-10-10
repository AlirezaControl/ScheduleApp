
// ScheduleMapper.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public static class ScheduleMapper
    {
        public static Dictionary<string, string> MapAssignmentsToTemplate(
            ScheduleDay scheduleDay,
            List<Post> posts,
            List<Person> persons)
        {
            var keyValues = new Dictionary<string, string>();

            // Add static fields safely
            keyValues["Date"] = ToPersianDate(scheduleDay.Date);
            keyValues["DateNow"] = ToPersianDate(DateTime.Now);

            // --- Time-based placeholder maps ---
            var ndMap = new Dictionary<int, string> // ضلع دژبانی
            {
                {6, "nd1"}, {8, "nd2"}, {10, "nd3"}, {12, "nd4"},
                {14, "nd5"}, {16, "nd6"}, {18, "nd7"}, {20, "nd8"},
                {22, "nd9"}, {0, "nd10"}, {2, "nd11"}, {4, "nd12"}
            };

            var shMap = new Dictionary<int, string> // ضلع شرقی
            {
                {6, "sh1"}, {8, "sh2"}, {10, "sh3"}, {12, "sh4"},
                {14, "sh5"}, {16, "sh6"}, {18, "sh7"}, {20, "sh8"},
                {22, "sh9"}, {0, "sh10"}, {2, "sh11"}, {4, "sh12"}
            };

            var ghMap = new Dictionary<int, string> // ضلع غربی
            {
                {6, "gh1"}, {8, "gh2"}, {10, "gh3"}, {12, "gh4"},
                {14, "gh5"}, {16, "gh6"}, {18, "gh7"}, {20, "gh8"},
                {22, "gh9"}, {0, "gh10"}, {2, "gh11"}, {4, "gh12"}
            };

            int pCounter = 1;
            int dCounter = 1;
            int aCounter = 1;

            foreach (var assignment in scheduleDay.Assignments)
            {
                var slot = scheduleDay.ShiftSlots.FirstOrDefault(s => s.Id == assignment.ShiftSlotId);
                if (slot == null) continue;

                var post = posts.FirstOrDefault(p => p.Id == slot.PostId);
                if (post == null) continue;

                var person = persons.FirstOrDefault(p => p.Id == assignment.PersonId);
                if (person == null) continue;

                string key = null;
                var postName = post.Name ?? "";

                // --- Determine key based on post type ---
                if (postName.Contains("پاسبخش"))
                    key = $"p{pCounter++}";
                else if (postName.Contains("دژبان") && !postName.Contains("نگهبان"))
                    key = $"d{dCounter++}";
                else if (postName.Contains("نگهبان بالای دژبانی") || postName.Contains("ضلع دژبانی"))
                    key = FindTimeMappedKey(slot, ndMap);
                else if (postName.Contains("نگهبان شرقی") || postName.Contains("ضلع شرقی"))
                    key = FindTimeMappedKey(slot, shMap);
                else if (postName.Contains("نگهبان غربی") || postName.Contains("ضلع غربی"))
                    key = FindTimeMappedKey(slot, ghMap);
                else if (postName.Contains("نیروی آماده"))
                    key = "na";
                else if (postName.Contains("راننده"))
                    key = "R";
                else if (postName.Contains("مسئول نظافت"))
                    key = "mn";
                else if (postName.Contains("افسر قرارگاه"))
                    key = "agh";
                else if (postName.Contains("آشپز") || postName.Contains("آشپزخانه"))
                    key = $"a{aCounter++}";
                else if (postName.Contains("افسر جانشین"))
                    key = "oj";
                else if (postName.Contains("مسئول پاسدارخانه"))
                    key = "mpk";
                else if (postName.Contains("نظافت پاسدارخانه"))
                    key = "npk";
                else if (postName.Contains("نگهبانی درب دژبانی"))
                    key = "ndb";
                else if (postName.Contains("نگهبانی ضلع شرقی روز بعد"))
                    key = "shd";
                else if (postName.Contains("نگهبانی ضلع غربی روز بعد"))
                    key = "ghd";

                if (string.IsNullOrEmpty(key))
                    continue;

                string fullName = $"{person.FirstName} {person.LastName}".Trim();

                // Combine duplicates with slash separator
                if (keyValues.ContainsKey(key))
                {
                    if (!keyValues[key].Contains(fullName))
                        keyValues[key] = $"{keyValues[key]} / {fullName}";
                }
                else
                {
                    keyValues[key] = fullName;
                }
            }

            return keyValues;
        }

        private static string FindTimeMappedKey(ShiftSlot slot, Dictionary<int, string> map)
        {
            if (slot == null || map == null || map.Count == 0)
                return null;

            int hour = slot.Start.Hours;

            // Treat midnight as 24 for mapping last slot
            if (hour == 0)
                hour = 24;

            if (map.ContainsKey(hour))
                return map[hour];

            var closestKey = map.Keys.OrderBy(k => Math.Abs(k - hour)).First();
            return map[closestKey];
        }

        private static string ToPersianDate(DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }
    }
}