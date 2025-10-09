using System;
using System.Collections.Generic;
using System.IO;

namespace WordOutputTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string template = @"Template.docx"; // Put your template path here
            string output = @"ScheduleOutput.docx";

            // Copy template to output file
            File.Copy(template, output, true);

            // Prepare all placeholders
            var replacements = new Dictionary<string, string>
            {
                {"{{Date}}","1404/07/17"},
                {"{{DateNow}}", DateTime.Now.ToString("yyyy/MM/dd")},
                {"{{p1}}","علی رضایی"},
                {"{{p2}}","رضا محمدی"},
                {"{{p3}}","حسین حسینی"},
                {"{{p4}}","مهدی کریمی"},
                {"{{p5}}","نیما منتظری"},
                {"{{d1}}","دژبان1"},
                {"{{d2}}","دژبان2"},
                {"{{d3}}","دژبان3"},
                {"{{d4}}","دژبان4"},
                {"{{d5}}","دژبان5"},
                {"{{d6}}","دژبان6"},
                {"{{d7}}","دژبان7"},
                {"{{d8}}","دژبان8"},
                {"{{d9}}","دژبان9"},
                {"{{d10}}","دژبان10"},
                {"{{d11}}","دژبان11"},
                {"{{d12}}","دژبان12"},
                {"{{nd1}}","نوبت صبح 1"},
                {"{{nd2}}","نوبت صبح 2"},
                {"{{nd3}}","نوبت صبح 3"},
                {"{{nd4}}","نوبت صبح 4"},
                {"{{nd5}}","نوبت صبح 5"},
                {"{{nd6}}","نوبت صبح 6"},
                {"{{nd7}}","نوبت عصر 1"},
                {"{{nd8}}","نوبت عصر 2"},
                {"{{nd9}}","نوبت عصر 3"},
                {"{{nd10}}","نوبت عصر 4"},
                {"{{nd11}}","نوبت عصر 5"},
                {"{{nd12}}","نوبت عصر 6"},
                {"{{sh1}}","شرقی صبح 1"},
                {"{{sh2}}","شرقی صبح 2"},
                {"{{sh3}}","شرقی صبح 3"},
                {"{{sh4}}","شرقی صبح 4"},
                {"{{sh5}}","شرقی صبح 5"},
                {"{{sh6}}","شرقی صبح 6"},
                {"{{sh7}}","شرقی عصر 1"},
                {"{{sh8}}","شرقی عصر 2"},
                {"{{sh9}}","شرقی عصر 3"},
                {"{{sh10}}","شرقی عصر 4"},
                {"{{sh11}}","شرقی عصر 5"},
                {"{{sh12}}","شرقی عصر 6"},
                {"{{gh1}}","غربی1"},
                {"{{gh2}}","غربی2"},
                {"{{gh3}}","غربی3"},
                {"{{gh4}}","غربی4"},
                {"{{gh5}}","غربی5"},
                {"{{gh6}}","غربی6"},
                {"{{gh7}}","غربی7"},
                {"{{gh8}}","غربی8"},
                {"{{na}}","نیروی آماده"},
                {"{{R}}","راننده آماده"},
                {"{{mn}}","نظافت آسایشگاه"},
                {"{{agh}}","افسر قرارگاه"},
                {"{{a1}}","شیفت آشپزخانه1"},
                {"{{a2}}","شیفت آشپزخانه2"}
            };

            WordHelper.ReplacePlaceholders(output, replacements);

            Console.WriteLine("All placeholders replaced successfully!");
            Console.WriteLine("Output file: " + output);
        }
    }
}
