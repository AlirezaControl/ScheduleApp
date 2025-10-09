using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WordOutputTest
{
    public static class WordHelper
    {
        /// <summary>
        /// Replaces placeholders in Word document. Works even if placeholders are split across runs.
        /// </summary>
        public static void ReplacePlaceholders(string filePath, Dictionary<string, string> replacements)
        {
            using (var doc = WordprocessingDocument.Open(filePath, true))
            {
                // Main body
                ReplaceInBody(doc.MainDocumentPart.Document.Body, replacements);

                // Headers
                foreach (var header in doc.MainDocumentPart.HeaderParts)
                    ReplaceInBody(header.Header, replacements);

                // Footers
                foreach (var footer in doc.MainDocumentPart.FooterParts)
                    ReplaceInBody(footer.Footer, replacements);

                doc.MainDocumentPart.Document.Save();
            }
        }

        private static void ReplaceInBody(OpenXmlCompositeElement body, Dictionary<string, string> replacements)
        {
            foreach (var paragraph in body.Descendants<Paragraph>())
            {
                // Merge all text in paragraph
                string paragraphText = string.Concat(paragraph.Descendants<Text>().Select(t => t.Text));
                bool replaced = false;

                foreach (var kv in replacements)
                {
                    if (paragraphText.Contains(kv.Key))
                    {
                        paragraphText = paragraphText.Replace(kv.Key, kv.Value);
                        replaced = true;
                    }
                }

                if (replaced)
                {
                    // Remove old runs
                    paragraph.RemoveAllChildren<Run>();

                    // Add a single run with replaced text
                    Run run = new Run();
                    run.AppendChild(new Text(paragraphText) { Space = SpaceProcessingModeValues.Preserve });
                    paragraph.AppendChild(run);
                }
            }
        }
    }
}
