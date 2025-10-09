using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public class WordTemplateExporter
    {
        /// <summary>
        /// Export a schedule day to a Word document by replacing placeholders.
        /// </summary>
        public void Export(string templatePath, string outputPath,
                           ScheduleDay scheduleDay, List<Post> posts, List<Person> persons)
        {
            // Copy template to output file
            File.Copy(templatePath, outputPath, true);

            

            var replacements = ScheduleMapper.MapAssignmentsToTemplate(scheduleDay, posts, persons);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                // Replace placeholders in main body
                ReplacePlaceholdersInBody(wordDoc.MainDocumentPart.Document.Body, replacements);

                // Replace placeholders in headers
                foreach (var headerPart in wordDoc.MainDocumentPart.HeaderParts)
                    ReplacePlaceholdersInBody(headerPart.Header, replacements);

                // Replace placeholders in footers
                foreach (var footerPart in wordDoc.MainDocumentPart.FooterParts)
                    ReplacePlaceholdersInBody(footerPart.Footer, replacements);

                wordDoc.MainDocumentPart.Document.Save();
            }
        }

        private void ReplacePlaceholdersInBody(OpenXmlCompositeElement body, Dictionary<string, string> replacements)
        {
            foreach (var paragraph in body.Descendants<Paragraph>())
            {
                ReplacePlaceholdersInParagraph(paragraph, replacements);
            }
        }

        private void ReplacePlaceholdersInParagraph(Paragraph paragraph, Dictionary<string, string> replacements)
        {
            var runs = paragraph.Elements<Run>().ToList();
            if (!runs.Any()) return;

            // Merge all text in paragraph
            string paragraphText = string.Concat(runs.Select(r => r.GetFirstChild<Text>()?.Text));

            bool replaced = false;

            foreach (var kv in replacements)
            {
                string placeholder = $"{{{{{kv.Key}}}}}"; // e.g., {{Date}}
                if (paragraphText.Contains(placeholder))
                {
                    paragraphText = paragraphText.Replace(placeholder, kv.Value);
                    replaced = true;
                }
            }

            if (replaced)
            {
                // Remove old runs
                paragraph.RemoveAllChildren<Run>();

                // Add one new run with replaced text
                Run newRun = new Run();
                newRun.AppendChild(new Text(paragraphText) { Space = SpaceProcessingModeValues.Preserve });
                paragraph.AppendChild(newRun);
            }
        }
    }
}
