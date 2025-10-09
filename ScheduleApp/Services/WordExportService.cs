using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GuardScheduler.Models;
using MD.PersianDateTime;

namespace GuardScheduler.Services
{
    public class WordTemplateExporter
    {
        /// <summary>
        /// Fills a Word template with ScheduleDay data and saves it to outputPath.
        /// </summary>
        public void FillTemplate(
            string templatePath,
            string outputPath,
            ScheduleDay day,
            List<Post> posts,
            List<Person> persons)
        {
            if (!System.IO.File.Exists(templatePath))
                throw new ArgumentException("Template file does not exist.", nameof(templatePath));

            // Build key-value dictionary
            var keyValues = BuildKeyValues(day, posts, persons);

            // Copy template
            System.IO.File.Copy(templatePath, outputPath, true);

            using (WordprocessingDocument doc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                // Replace placeholders
                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var kvp in keyValues)
                    {
                        if (text.Text.Contains(kvp.Key))
                            text.Text = text.Text.Replace(kvp.Key, kvp.Value);
                    }
                }

                doc.MainDocumentPart.Document.Save();
            }
        }

        private Dictionary<string, string> BuildKeyValues(ScheduleDay day, List<Post> posts, List<Person> persons)
        {
            var dict = new Dictionary<string, string>();

            // Dates
            dict["{{Date}}"] = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");
            dict["{{DateNow}}"] = new PersianDateTime(DateTime.Now).ToString("yyyy/MM/dd");

            // پاسبخش {{p1}}..{{p5}}
            FillSlots(day, posts, persons, "پاس‌بخش", 5, "p", dict);

            // دژبان {{d1}}..{{d12}}
            FillSlots(day, posts, persons, "دژبان", 12, "d", dict);

            // نگهبانی {{nd1}}..{{nd12}}, {{sh1}}..{{sh12}}, {{gh1}}..{{gh8}}
            var nightSlots = day.ShiftSlots
                .Where(s => posts.FirstOrDefault(p => p.Id == s.PostId)?.Name.Contains("نگهبانی") ?? false)
                .OrderBy(s => s.Start)
                .ToList();

            for (int i = 0; i < nightSlots.Count; i++)
            {
                var slot = nightSlots[i];
                var assignment = day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id);
                var name = assignment != null
                    ? persons.FirstOrDefault(p => p.Id == assignment.PersonId)?.FullName() ?? ""
                    : "";

                dict[$"{{nd{i + 1}}}"] = name;
                dict[$"{{sh{i + 1}}}"] = name;
                if (i < 8) dict[$"{{gh{i + 1}}}"] = name;
            }

            // Other placeholders
            dict["{{na}}"] = GetAssignedPerson(day, posts, persons, "نیروی آماده");
            dict["{{R}}"] = GetAssignedPerson(day, posts, persons, "راننده آماده");
            dict["{{mn}}"] = GetAssignedPerson(day, posts, persons, "مسئول نظافت");
            dict["{{agh}}"] = GetAssignedPerson(day, posts, persons, "افسر قرارگاه");
            dict["{{a1}}"] = GetAssignedPerson(day, posts, persons, "آشپزخانه1");
            dict["{{a2}}"] = GetAssignedPerson(day, posts, persons, "آشپزخانه2");

            return dict;
        }

        private void FillSlots(ScheduleDay day, List<Post> posts, List<Person> persons,
            string postName, int maxCount, string keyPrefix, Dictionary<string, string> dict)
        {
            var slots = day.ShiftSlots
                .Where(s => posts.FirstOrDefault(p => p.Id == s.PostId)?.Name.Contains(postName) ?? false)
                .OrderBy(s => s.Start)
                .ToList();

            for (int i = 0; i < maxCount; i++)
            {
                var slot = slots.ElementAtOrDefault(i);
                var assignment = slot != null ? day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id) : null;
                dict[$"{{{keyPrefix}{i + 1}}}"] = assignment != null
                    ? persons.FirstOrDefault(p => p.Id == assignment.PersonId)?.FullName() ?? ""
                    : "";
            }
        }

        private string GetAssignedPerson(ScheduleDay day, List<Post> posts, List<Person> persons, string postName)
        {
            var slot = day.ShiftSlots
                .FirstOrDefault(s => posts.FirstOrDefault(p => p.Id == s.PostId)?.Name.Contains(postName) ?? false);

            var assignment = slot != null ? day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id) : null;

            return assignment != null ? persons.FirstOrDefault(p => p.Id == assignment.PersonId)?.FullName() ?? "" : "";
        }
    }

    public static class PersonExtensions
    {
        public static string FullName(this Person p) => p != null ? $"{p.FirstName} {p.LastName}" : "";
    }
}
