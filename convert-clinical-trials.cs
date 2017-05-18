using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ClinicalTrialsDataImporter
{
    class Program
    {
        public static string outDir = @"<output-directory>";

        static void Main(string[] args)
        {
            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            // Recurse into subdirectories of this directory.
            DirectoryInfo d = new DirectoryInfo(@"<input-directory>");
            int counter = 0;
            foreach (var file in d.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                counter++;
                Console.WriteLine("{0} - " + file.FullName, counter);
                XDocument doc = XDocument.Load(file.FullName);
                string file_name = file.Name.Replace(".xml", "");


                string download_date = string.Empty;
                string url = string.Empty;
                string org_study_id = string.Empty;
                string nct_id = string.Empty;
                string brief_title = string.Empty;
                string official_title = string.Empty;
                string brief_summary = string.Empty;
                string detailed_description = string.Empty;
                string overall_status = string.Empty;
                string start_date = string.Empty;
                string phase = string.Empty;
                string study_type = string.Empty;
                string eligibility = string.Empty;
                string gender = string.Empty;
                string minimum_age = string.Empty;
                string maximum_age = string.Empty;
                string healthy_volunteers = string.Empty;
                List<string> keywords = null;

                download_date = GetNodeValue(doc, "required_header", "download_date");
                url = GetNodeValue(doc, "required_header", "url");

                org_study_id = GetNodeValue(doc, "id_info", "org_study_id");
                nct_id = GetNodeValue(doc, "id_info", "nct_id");

                brief_title = GetNodeValue(doc, "brief_title", "");
                official_title = GetNodeValue(doc, "official_title", "");

                brief_summary = GetNodeValue(doc, "brief_summary", "textblock");
                detailed_description = GetNodeValue(doc, "detailed_description", "textblock");

                overall_status = GetNodeValue(doc, "overall_status", "");
                start_date = GetNodeValue(doc, "start_date", "");
                phase = GetNodeValue(doc, "phase", "");

                gender = GetNodeValue(doc, "eligibility", "gender");
                minimum_age = GetNodeValue(doc, "eligibility", "minimum_age").Replace("Years", "").Trim();

                int number;
                if (Int32.TryParse(minimum_age, out number) == false)
                    minimum_age = "0";

                maximum_age = GetNodeValue(doc, "eligibility", "maximum_age").Replace("Years", "").Trim();
                if (Int32.TryParse(maximum_age, out number) == false)
                    maximum_age = "0";

                healthy_volunteers = GetNodeValue(doc, "eligibility", "healthy_volunteers");

                keywords = GetNodeArray(doc, "keyword", "");
                string keywordsJson = "";
                if (keywords.Count > 0)
                {
                    keywordsJson = "[";
                    foreach (string item in keywords)
                    {
                        keywordsJson += "'" + item.Replace("\"", "").Replace("'", "").Replace("\\", " ") + "',";
                    }
                    keywordsJson = keywordsJson.Substring(0, keywordsJson.Length - 1) + "]";
                }

                string content = string.Empty;
                if (download_date != string.Empty)
                    content += "Date: " + download_date + "\r\n\r\n";
                if (url != string.Empty)
                    content += "URL: " + url + "\r\n\r\n";
                if (org_study_id != string.Empty)
                    content += "Org Study Id: " + org_study_id + "\r\n\r\n";
                if (nct_id != string.Empty)
                    content += "NCT ID:" + nct_id + "\r\n\r\n";
                if (brief_title != string.Empty)
                    content += "Title:" + brief_title + "\r\n\r\n";
                if (official_title != string.Empty)
                    content += "Official Title: " + official_title + "\r\n\r\n";
                if (brief_summary != string.Empty)
                    content += "Summary: " + brief_summary + "\r\n\r\n";
                if (detailed_description != string.Empty)
                    content += "Description: " + detailed_description + "\r\n\r\n";
                if (overall_status != string.Empty)
                    content += "Overall Status: " + overall_status + "\r\n\r\n";
                if (start_date != string.Empty)
                    content += "Start Date: " + start_date + "\r\n\r\n";
                if (phase != string.Empty)
                    content += "Phase: " + phase + "\r\n\r\n";
                if (study_type != string.Empty)
                    content += "Study Type: " + study_type + "\r\n\r\n";
                if (eligibility != string.Empty)
                    content += "Eligibility: " + eligibility + "\r\n\r\n";
                if (gender != string.Empty)
                    content += "Gender: " + gender + "\r\n\r\n";
                if (minimum_age != string.Empty)
                    content += "Minimum Age: " + minimum_age + "\r\n\r\n";
                if (maximum_age != string.Empty)
                    content += "Maximum Age: " + maximum_age + "\r\n\r\n";
                if (healthy_volunteers != string.Empty)
                    content += "Healthy Volunteers: " + healthy_volunteers + "\r\n\r\n";
                if (keywordsJson != string.Empty)
                    content += "Keywords: " + keywordsJson;

                File.WriteAllText(Path.Combine(outDir, file_name + ".txt"), content);


            }
        }

        static string GetNodeValue(XDocument doc, string topLevel, string nodeValue)
        {
            string Value = string.Empty;
            foreach (XElement element in doc.Descendants(topLevel))
            {
                try
                {
                    if (nodeValue != "")
                        Value = element.Element(nodeValue).Value;
                    else
                        Value = element.Value;
                }
                catch
                {
                    Console.WriteLine("No " + nodeValue + " found");
                }
            }

            return Value;

        }

        static List<string> GetNodeArray(XDocument doc, string topLevel, string nodeValue)
        {
            List<string> Value = new List<string>();
            foreach (XElement element in doc.Descendants(topLevel))
            {
                try
                {
                    Value.Add(element.Value);
                }
                catch
                {
                    Console.WriteLine("No " + topLevel + "/" + nodeValue + " found");
                }
            }
            return Value;
        }
    }
}

