using System;
using System.Collections.Generic;
using System.Linq;
using GuardScheduler.Models;
using MD.PersianDateTime;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace GuardScheduler.Services
{
    public interface IWordExportService
    {
        void ExportScheduleToWord(IEnumerable<ScheduleDay> scheduleDays, string filePath, List<Person> persons, List<Post> posts);
    }

    public class WordExportService : IWordExportService
    {
        /// <summary>
        /// Exports a list of ScheduleDay objects to a Word document.
        /// Each day has a table: posts as rows, hours as columns.
        /// Dates are exported in Jalali format.
        /// </summary>
        public void ExportScheduleToWord(IEnumerable<ScheduleDay> scheduleDays, string filePath, List<Person> persons, List<Post> posts)
        {
            using (var wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                foreach (var day in scheduleDays)
                {
                    // Convert date to Jalali
                    var jalaliDate = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");

                    var dayText = new Paragraph(
                        new Run(
                            new Text($"تاریخ: {jalaliDate}"))
                        )
                    {
                        ParagraphProperties = new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center })
                    };
                    body.AppendChild(dayText);

                    // Create table: first column Post, next 24 columns hours
                    Table table = new Table();

                    TableProperties tblProps = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                        )
                    );
                    table.AppendChild(tblProps);

                    // Header row
                    TableRow header = new TableRow();
                    header.AppendChild(CreateCell("پست/ساعت"));
                    for (int h = 0; h < 24; h++)
                        header.AppendChild(CreateCell(h.ToString("00") + ":00"));
                    table.AppendChild(header);

                    foreach (var post in posts)
                    {
                        var slots = day.ShiftSlots.Where(s => s.PostId == post.Id).ToList();
                        if (!slots.Any()) continue;

                        TableRow row = new TableRow();
                        row.AppendChild(CreateCell(post.Name));

                        // Build 24-hour row
                        for (int hour = 0; hour < 24; hour++)
                        {
                            var slot = slots.FirstOrDefault(s => s.Start.Hours <= hour && hour < s.Start.Hours + s.DurationHours);
                            if (slot != null)
                            {
                                var assignment = day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id);
                                if (assignment != null)
                                {
                                    var person = persons.FirstOrDefault(p => p.Id == assignment.PersonId);
                                    string name = person != null ? $"{person.FirstName} {person.LastName}" : "";
                                    row.AppendChild(CreateCell(name));
                                }
                                else
                                {
                                    row.AppendChild(CreateCell(""));
                                }
                            }
                            else
                            {
                                row.AppendChild(CreateCell(""));
                            }
                        }

                        table.AppendChild(row);
                    }

                    body.AppendChild(table);
                    body.AppendChild(new Paragraph(new Run(new Text("")))); // empty line between days
                }
            }
        }

        private TableCell CreateCell(string text)
        {
            TableCell cell = new TableCell();
            cell.Append(new Paragraph(new Run(new Text(text))));
            return cell;
        }
    }
}
