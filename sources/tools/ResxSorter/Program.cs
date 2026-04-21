using System.Text;
using System.Xml;

namespace Codice.SortResX
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var sourceDir = Directory.GetCurrentDirectory();
            while (Path.GetFileName(sourceDir) != "sources")
                sourceDir = Directory.GetParent(sourceDir)!.FullName;
            var localizationDir = Path.Combine(sourceDir, "BUtil.Core", "Localization");

            var dictionary = new Dictionary<string, int>();
            var allResx = Directory
                .GetFiles(localizationDir, "*.resx")
                .OrderBy(x => x.Length);
            var mainFile = allResx.First();

            foreach (var resx in Directory
                .GetFiles(localizationDir, "*.resx")
                .OrderBy(x => x.Length))
            {
                Console.WriteLine($"Sorting {resx}");
                int countKeys = new FileProcessor(resx)
                    .Process();

                dictionary.Add(resx, countKeys);
                
                if (countKeys != dictionary[mainFile])
                {
                    var percent = (countKeys * 100.0) / (dictionary[mainFile] * 1.0);
                    if (percent < 95)
                    {
                        throw new Exception($"{Path.GetFileNameWithoutExtension(resx)} needs attention {percent}.");
                    }
                }
            }

            NsisLanguageGenerator.Generate(sourceDir);
        }
    }

    public class FileProcessor
    {
        public FileProcessor(string path)
        {
            mPath = path;
            mResourceNameList = new List<string>();
            mResourceNodes = new Dictionary<string, XmlNode>();
            mDoc = new XmlDocument();
            mDoc.Load(mPath);
        }

        public int Process()
        {
            ExtractResources("data/@name");
            var sortedNames = SortResourceList();
            WriteOrderedResources(sortedNames);
            return sortedNames.Count();
        }

        void ExtractResources(string query)
        {
            var nodesFileNames = new[] {
                "LogFile_Marker_Errors",
                "LogFile_Marker_Successful",
                "File_IntegrityVerificationScript_Ps1",
		"BackupVersion_Button_Delete"
            };

            foreach (XmlAttribute attribute in mDoc.DocumentElement!.SelectNodes(query)!)
            {
                var element = attribute.OwnerElement!;
                if (nodesFileNames.Contains(attribute.Value))
                {
                    foreach (XmlNode child in element.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            var value = child.InnerText;

                            if (Path.GetInvalidPathChars().Any(x => value.Contains(x)) ||
                                Path.GetInvalidFileNameChars().Any(x => value.Contains(x)))
                                throw new Exception($"{attribute.Name} contains invalid path chars");
                        }
                    }

                }
                AddXmlNode(element, attribute);
                element.ParentNode!.RemoveChild(element);
            }
        }

        void AddXmlNode(XmlNode node, XmlAttribute attribute)
        {
            if (mResourceNodes.ContainsKey(attribute.Value.ToString()))
                return;

            mResourceNodes.Add(attribute.Value.ToString(), node);
            mResourceNameList.Add(attribute.Value.ToString());
        }

        string[] SortResourceList()
        {
            string[] names = new string[mResourceNameList.Count];

            for (int i = 0; i < mResourceNameList.Count; i++)
                names[i] = mResourceNameList[i];

            Array.Sort(names);
            return names;
        }

        void WriteOrderedResources(string[] names)
        {
            foreach (string key in names)
            {
                mDoc.DocumentElement!.AppendChild(mResourceNodes[key]);
            }

            mDoc.Save(mPath);
        }

        private List<string> mResourceNameList = null!;
        private Dictionary<string, XmlNode> mResourceNodes = null!;
        private XmlDocument mDoc = null!;
        private string mPath = null!;
    }

    public static class NsisLanguageGenerator
    {
        private static readonly Dictionary<string, string> ResxToNsisLanguage = new()
        {
            { "", "English" },
            { "ar", "Arabic" },
            { "de", "German" },
            { "es", "Spanish" },
            { "fr", "French" },
            { "hi", "Hindi" },
            { "id", "Indonesian" },
            { "it", "Italian" },
            { "ja", "Japanese" },
            { "pt", "PortugueseBR" },
            { "ru", "Russian" },
            { "tr", "Turkish" },
            { "uk", "Ukrainian" },
            { "vi", "Vietnamese" },
            { "zh-Hans", "SimpChinese" },
        };

        public static void Generate(string sourceDir)
        {
            var localizationDir = Path.Combine(sourceDir, "BUtil.Core", "Localization");
            var outputPath = Path.Combine(sourceDir, "setup-languages.nsh");

            var allResx = Directory.GetFiles(localizationDir, "*.resx")
                .OrderBy(x => x.Length);

            var languageData = new Dictionary<string, Dictionary<string, string>>();

            foreach (var resxPath in allResx)
            {
                var culture = ExtractCulture(resxPath);
                if (!ResxToNsisLanguage.ContainsKey(culture))
                    continue;

                var doc = new XmlDocument();
                doc.Load(resxPath);

                var entries = new Dictionary<string, string>();
                foreach (XmlNode node in doc.SelectNodes("//data")!)
                {
                    var name = node.Attributes?["name"]?.Value;
                    if (name != null && name.StartsWith("Installer_"))
                    {
                        var value = node.SelectSingleNode("value")?.InnerText ?? "";
                        entries[name] = value;
                    }
                }

                languageData[culture] = entries;
            }

            if (!languageData.TryGetValue("", out var mainEntries) || mainEntries.Count == 0)
            {
                Console.WriteLine("No Installer_ keys found in main resources, skipping NSH generation.");
                return;
            }

            using var writer = new StreamWriter(outputPath, false, new UTF8Encoding(false));
            writer.WriteLine("; Auto-generated by ResxSorter from .resx resources - do not edit manually");
            writer.WriteLine();

            writer.WriteLine($"!insertmacro MUI_LANGUAGE \"English\"");
            foreach (var kvp in ResxToNsisLanguage.Where(x => x.Key != "").OrderBy(x => x.Value))
            {
                if (languageData.ContainsKey(kvp.Key))
                    writer.WriteLine($"!insertmacro MUI_LANGUAGE \"{kvp.Value}\"");
            }
            writer.WriteLine();

            foreach (var key in mainEntries.Keys.OrderBy(x => x))
            {
                foreach (var kvp in ResxToNsisLanguage.OrderBy(x => x.Value == "English" ? "" : x.Value))
                {
                    string? value = null;
                    if (languageData.TryGetValue(kvp.Key, out var entries))
                        entries.TryGetValue(key, out value);
                    value ??= mainEntries[key];

                    var escapedValue = EscapeForNsis(value);
                    writer.WriteLine($"LangString {key} ${{LANG_{kvp.Value.ToUpperInvariant()}}} \"{escapedValue}\"");
                }
                writer.WriteLine();
            }

            Console.WriteLine($"Generated {outputPath}");
        }

        private static string ExtractCulture(string resxPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(resxPath);
            var dotIndex = fileName.IndexOf('.');
            return dotIndex >= 0 ? fileName[(dotIndex + 1)..] : "";
        }

        private static string EscapeForNsis(string value)
        {
            var sb = new StringBuilder();
            foreach (var ch in value)
            {
                switch (ch)
                {
                    case '$': sb.Append("$$"); break;
                    case '"': sb.Append("$\\\""); break;
                    case '\n': sb.Append("$\\n"); break;
                    case '\r': break;
                    case '\t': sb.Append("$\\t"); break;
                    case '`': sb.Append("$\\`"); break;
                    default: sb.Append(ch); break;
                }
            }
            return sb.ToString();
        }
    }
}
