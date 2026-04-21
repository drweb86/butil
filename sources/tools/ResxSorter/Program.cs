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
            WingetLocaleGenerator.Generate(sourceDir);
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
        public static void Generate(string sourceDir)
        {
            var localizationDir = Path.Combine(sourceDir, "BUtil.Core", "Localization");
            var outputPath = Path.Combine(sourceDir, "setup-languages.nsh");

            var allResx = Directory.GetFiles(localizationDir, "*.resx")
                .OrderBy(x => x.Length);

            var languageData = new Dictionary<string, (string NsisLanguage, Dictionary<string, string> Entries)>();

            foreach (var resxPath in allResx)
            {
                var doc = new XmlDocument();
                doc.Load(resxPath);

                string? nsisLanguage = null;
                var entries = new Dictionary<string, string>();

                foreach (XmlNode node in doc.SelectNodes("//data")!)
                {
                    var name = node.Attributes?["name"]?.Value;
                    if (name == "_Technical_NsisLanguage")
                        nsisLanguage = node.SelectSingleNode("value")?.InnerText;
                    else if (name != null && name.StartsWith("Installer_"))
                    {
                        var value = node.SelectSingleNode("value")?.InnerText ?? "";
                        entries[name] = value;
                    }
                }

                if (nsisLanguage == null)
                    continue;

                var culture = ExtractCulture(resxPath);
                languageData[culture] = (nsisLanguage, entries);
            }

            if (!languageData.TryGetValue("", out var mainData) || mainData.Entries.Count == 0)
            {
                Console.WriteLine("No Installer_ keys found in main resources, skipping NSH generation.");
                return;
            }

            using var writer = new StreamWriter(outputPath, false, new UTF8Encoding(false));
            writer.WriteLine("; Auto-generated by ResxSorter from .resx resources - do not edit manually");
            writer.WriteLine();

            writer.WriteLine($"!insertmacro MUI_LANGUAGE \"English\"");
            foreach (var kvp in languageData.Where(x => x.Key != "").OrderBy(x => x.Value.NsisLanguage))
            {
                writer.WriteLine($"!insertmacro MUI_LANGUAGE \"{kvp.Value.NsisLanguage}\"");
            }
            writer.WriteLine();

            foreach (var key in mainData.Entries.Keys.OrderBy(x => x))
            {
                foreach (var kvp in languageData.OrderBy(x => x.Value.NsisLanguage == "English" ? "" : x.Value.NsisLanguage))
                {
                    string? value = null;
                    kvp.Value.Entries.TryGetValue(key, out value);
                    value ??= mainData.Entries[key];

                    var escapedValue = EscapeForNsis(value);
                    writer.WriteLine($"LangString {key} ${{LANG_{kvp.Value.NsisLanguage.ToUpperInvariant()}}} \"{escapedValue}\"");
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

    public static class WingetLocaleGenerator
    {
        public static void Generate(string sourceDir)
        {
            var localizationDir = Path.Combine(sourceDir, "BUtil.Core", "Localization");
            var wingetPkgsDir = Path.Combine(sourceDir, "tools", "winget-pkgs");

            var allResx = Directory.GetFiles(localizationDir, "*.resx")
                .OrderBy(x => x.Length);

            foreach (var resxPath in allResx)
            {
                var doc = new XmlDocument();
                doc.Load(resxPath);

                string? wingetLocale = null;
                string? shortDescription = null;
                string? description = null;

                foreach (XmlNode node in doc.SelectNodes("//data")!)
                {
                    var name = node.Attributes?["name"]?.Value;
                    if (name == "_Technical_WingetLocale")
                        wingetLocale = node.SelectSingleNode("value")?.InnerText;
                    else if (name == "Winget_ShortDescription")
                        shortDescription = node.SelectSingleNode("value")?.InnerText;
                    else if (name == "Winget_Description")
                        description = node.SelectSingleNode("value")?.InnerText;
                }

                if (wingetLocale == null)
                    continue;

                if (shortDescription == null || description == null)
                {
                    Console.WriteLine($"Missing Winget_ keys in {resxPath}, skipping locale generation.");
                    continue;
                }

                var culture = ExtractCulture(resxPath);
                var isDefaultLocale = culture == "";
                var schemaType = isDefaultLocale ? "defaultLocale" : "locale";
                var manifestType = isDefaultLocale ? "defaultLocale" : "locale";
                var outputFileName = $"SiarheiKuchuk.BUtil.locale.{wingetLocale}.yaml";
                var outputPath = Path.Combine(wingetPkgsDir, outputFileName);

                using var writer = new StreamWriter(outputPath, false, new UTF8Encoding(false));
                writer.WriteLine($"# yaml-language-server: $schema=https://aka.ms/winget-manifest.{schemaType}.1.10.0.schema.json");
                writer.WriteLine();
                writer.WriteLine("PackageIdentifier: SiarheiKuchuk.BUtil");
                writer.WriteLine("PackageVersion: APP_VERSION_STRING");
                writer.WriteLine($"PackageLocale: {wingetLocale}");
                writer.WriteLine("Publisher: Siarhei Kuchuk");
                writer.WriteLine("PublisherUrl: https://github.com/drweb86");
                writer.WriteLine("PublisherSupportUrl: https://github.com/drweb86/butil/issues");
                writer.WriteLine("PrivacyUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/Privacy%20Policy.md");
                writer.WriteLine("Author: Siarhei Kuchuk");
                writer.WriteLine("PackageName: BUtil");
                writer.WriteLine("PackageUrl: https://github.com/drweb86/butil");
                writer.WriteLine("License: MIT, GPL, MSPL");
                writer.WriteLine("LicenseUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/License.md");
                writer.WriteLine("Copyright: 2011-CURRENT_YEAR Siarhei Kuchuk");
                writer.WriteLine("CopyrightUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/License.md");
                writer.WriteLine($"ShortDescription: {shortDescription}");
                writer.WriteLine("Description: |");
                foreach (var line in description.Split('\n'))
                {
                    var trimmedLine = line.TrimEnd('\r');
                    writer.WriteLine($"  {trimmedLine}");
                }
                writer.WriteLine("Moniker: butil");
                writer.WriteLine("Tags:");
                writer.WriteLine("- backup");
                writer.WriteLine("- sync");
                writer.WriteLine("- synchronization");
                writer.WriteLine("- p2p");
                writer.WriteLine("ReleaseNotesUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/CHANGELOG.md");
                writer.WriteLine($"ManifestType: {manifestType}");
                writer.WriteLine("ManifestVersion: 1.10.0");

                Console.WriteLine($"Generated {outputPath}");
            }
        }

        private static string ExtractCulture(string resxPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(resxPath);
            var dotIndex = fileName.IndexOf('.');
            return dotIndex >= 0 ? fileName[(dotIndex + 1)..] : "";
        }
    }
}
