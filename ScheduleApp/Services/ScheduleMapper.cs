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

            keyValues["Date"] = ToPersianDate(scheduleDay.Date);
            keyValues["DateNow"] = ToPersianDate(DateTime.Now);

            var ndMap = new Dictionary<int, string>
            {
                {6, "nd1"}, {8, "nd2"}, {10, "nd3"}, {12, "nd4"},
                {14, "nd5"}, {16, "nd6"}, {18, "nd7"}, {20, "nd8"},
                {22, "nd9"}, {0, "nd10"}, {2, "nd11"}, {4, "nd12"}
            };

            var shMap = new Dictionary<int, string>
            {
                {6, "sh1"}, {8, "sh2"}, {10, "sh3"}, {12, "sh4"},
                {14, "sh5"}, {16, "sh6"}, {18, "sh7"}, {20, "sh8"},
                {22, "sh9"}, {0, "sh10"}, {2, "sh11"}, {4, "sh12"}
            };

            var ghMap = new Dictionary<int, string>
            {
                {6, "gh1"}, {8, "gh2"}, {10, "gh3"}, {12, "gh4"},
                {14, "gh5"}, {16, "gh6"}, {18, "gh7"}, {20, "gh8"},
                {22, "gh9"}, {0, "gh10"}, {2, "gh11"}, {4, "gh12"}
            };

            int pCounter = 1, dCounter = 1, aCounter = 1;

            foreach (var assignment in scheduleDay.Assignments)
            {
                var slot = scheduleDay.ShiftSlots.FirstOrDefault(s => s.Id == assignment.ShiftSlotId);
                if (slot == null) continue;

                var post = posts.FirstOrDefault(p => p.Id == slot.PostId);
                if (post == null) continue;

                var person = persons.FirstOrDefault(p => p.Id == assignment.PersonId);
                if (person == null) continue;

                string fullName = $"{person.FirstName} {person.LastName}".Trim();

                if (post.Name.Contains("پاسبخش"))
                    AddToKeyValues(keyValues, $"p{pCounter++}", fullName);
                else if (post.Name.Contains("دژبان") && !post.Name.Contains("نگهبان"))
                    AddToKeyValues(keyValues, $"d{dCounter++}", fullName);
                else if (post.Name.Contains("نگهبان بالای دژبانی") || post.Name.Contains("ضلع دژبانی"))
                    foreach (var k in FindTimeMappedKeys(slot, ndMap))
                        AddToKeyValues(keyValues, k, fullName);
                else if (post.Name.Contains("نگهبان شرقی") || post.Name.Contains("ضلع شرقی"))
                    foreach (var k in FindTimeMappedKeys(slot, shMap))
                        AddToKeyValues(keyValues, k, fullName);
                else if (post.Name.Contains("نگهبان غربی") || post.Name.Contains("ضلع غربی"))
                    foreach (var k in FindTimeMappedKeys(slot, ghMap))
                        AddToKeyValues(keyValues, k, fullName);
                else if (post.Name.Contains("نیروی آماده"))
                    AddToKeyValues(keyValues, "na", fullName);
                else if (post.Name.Contains("راننده"))
                    AddToKeyValues(keyValues, "R", fullName);
                else if (post.Name.Contains("مسئول نظافت"))
                    AddToKeyValues(keyValues, "mn", fullName);
                else if (post.Name.Contains("افسر قرارگاه"))
                    AddToKeyValues(keyValues, "agh", fullName);
                else if (post.Name.Contains("آشپز") || post.Name.Contains("آشپزخانه"))
                    AddToKeyValues(keyValues, $"a{aCounter++}", fullName);
                else if (post.Name.Contains("افسر جانشین"))
                    AddToKeyValues(keyValues, "oj", fullName);
                else if (post.Name.Contains("مسئول پاسدارخانه"))
                    AddToKeyValues(keyValues, "mpk", fullName);
                else if (post.Name.Contains("نظافت پاسدارخانه"))
                    AddToKeyValues(keyValues, "npk", fullName);
                else if (post.Name.Contains("نگهبانی درب دژبانی"))
                    AddToKeyValues(keyValues, "ndb", fullName);
                else if (post.Name.Contains("نگهبانی ضلع شرقی روز بعد"))
                    AddToKeyValues(keyValues, "shd", fullName);
                else if (post.Name.Contains("نگهبانی ضلع غربی روز بعد"))
                    AddToKeyValues(keyValues, "ghd", fullName);
            }

            return keyValues;
        }

        private static IEnumerable<string> FindTimeMappedKeys(ShiftSlot slot, Dictionary<int, string> map)
        {
            if (slot == null) yield break;

            for (int i = 0; i < slot.DurationHours; i++)
            {
                int hour = (slot.Start.Hours + i) % 24;
                if (map.ContainsKey(hour))
                    yield return map[hour];
            }
        }

        private static void AddToKeyValues(Dictionary<string, string> keyValues, string key, string fullName)
        {
            if (keyValues.ContainsKey(key))
            {
                if (!keyValues[key].Contains(fullName))
                    keyValues[key] = $"{keyValues[key]} / {fullName}";
            }
            else keyValues[key] = fullName;
        }

        private static string ToPersianDate(DateTime? date)
        {
            if (!date.HasValue) return "";
            var pc = new PersianCalendar();
            var d = date.Value;
            return $"{pc.GetYear(d)}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00}";
        }
    }
}
