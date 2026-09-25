using System.Text;
using System.Xml;

// =============================================================================
// TECHNICAL KEYS IN RESX FILES (DO NOT TRANSLATE)
// =============================================================================
// This tool reads special "_Technical_*" keys from .resx files to generate
// language-specific output files. These keys should NOT be translated - they
// contain technical identifiers used by external systems.
//
// Available technical keys:
//
// 1. _Technical_WingetLocale
//    - Legacy locale id. WinGet copy now comes from fastlane/metadata/microsoft.
//
// 2. _Technical_NsisLanguage
//    - Purpose: NSIS installer language name (must match NSIS built-in names)
//    - Example values: "English", "German", "SimpChinese", "PortugueseBR"
//    - Used by: NsisLanguageGenerator to create setup-languages.nsh
//    - Required for: Only languages supported by NSIS installer
//    - Reference: https://nsis.sourceforge.io/docs/Chapter5.html#langsinst
//
// Adding a new language:
//   1. Create Resources.{culture}.resx file
//   2. Add _Technical_NsisLanguage ONLY if NSIS supports that language
//   3. Add Installer_* translations if NSIS language is supported
//   4. Add fastlane/metadata/microsoft/<locale>/ for Store and WinGet copy
//   5. Run ResxSorter to generate output files
// =============================================================================

namespace Codice.SortResX
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var sourceDir = Directory.GetCurrentDirectory();
            while (Path.GetFileName(sourceDir) != "sources")
                sourceDir = Directory.GetParent(sourceDir)!.FullName;
            var repoRoot = Directory.GetParent(sourceDir)!.FullName;
            if (args.Contains("--only-winget"))
            {
                WingetLocaleGenerator.Generate(sourceDir, repoRoot);
                return;
            }

            var localizationDir = Path.Combine(sourceDir, "BUtil.Core.Localization");

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
            WingetLocaleGenerator.Generate(sourceDir, repoRoot);
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
            var localizationDir = Path.Combine(sourceDir, "BUtil.Core.Localization");
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
        const int ShortDescriptionMin = 3;
        const int ShortDescriptionMax = 256;
        const int DescriptionMin = 3;
        const int DescriptionMax = 10000;
        const int TagMin = 1;
        const int TagMax = 40;
        const int TagsMaxItems = 16;
        const int PackageLocaleMax = 20;

        static readonly Dictionary<string, string> PublishedLocaleOverrides = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ha-latn-ng"] = "ha-NG",
            ["uz-latn-uz"] = "uz-UZ",
        };

        public static void Generate(string sourceDir, string repoRoot)
        {
            var metadataDir = Path.Combine(repoRoot, "fastlane", "metadata", "microsoft");
            var wingetPkgsDir = Path.Combine(sourceDir, "tools", "winget-pkgs");
            if (!Directory.Exists(metadataDir))
                throw new Exception($"Fastlane metadata directory not found: {metadataDir}");

            var manifests = new List<LocaleManifest>();
            var seenLocales = new HashSet<string>(StringComparer.Ordinal);
            foreach (var localeDir in Directory.GetDirectories(metadataDir).OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
            {
                var folderName = Path.GetFileName(localeDir);
                if (!File.Exists(Path.Combine(localeDir, "short_description.txt")))
                    continue;

                var wingetLocale = ToWingetLocale(folderName);
                if (!seenLocales.Add(wingetLocale))
                    throw new Exception($"Fastlane folder '{folderName}' maps to WinGet locale '{wingetLocale}', which is already used.");

                manifests.Add(ReadManifest(localeDir, folderName, wingetLocale));
            }

            if (!seenLocales.Contains("en-US"))
                throw new Exception("fastlane/metadata/microsoft/en-us is required as the WinGet default locale.");

            Directory.CreateDirectory(wingetPkgsDir);
            foreach (var stale in Directory.GetFiles(wingetPkgsDir, "SiarheiKuchuk.BUtil.locale.*.yaml"))
                File.Delete(stale);

            foreach (var manifest in manifests)
            {
                var outputPath = Path.Combine(wingetPkgsDir, $"SiarheiKuchuk.BUtil.locale.{manifest.Locale}.yaml");
                WriteManifest(outputPath, manifest);
                Console.WriteLine($"Generated {outputPath}");
            }
        }

        static LocaleManifest ReadManifest(string localeDir, string folderName, string wingetLocale)
        {
            var shortDescription = ReadRequired(localeDir, "short_description.txt", folderName);
            if (shortDescription.Contains('\n'))
                throw new Exception($"{folderName}: short_description.txt must be a single line.");
            RequireLength(folderName, "ShortDescription", shortDescription, ShortDescriptionMin, ShortDescriptionMax, allowNewlines: false);

            var description = NormalizeBlock(ReadRequired(localeDir, "description.txt", folderName));
            var features = ReadLines(ReadRequired(localeDir, "features.txt", folderName));
            if (features.Count == 0)
                throw new Exception($"{folderName}: features.txt has no feature lines.");

            var featureBlock = string.Join("\n", features.Select(line => line.StartsWith("- ", StringComparison.Ordinal) ? line : "- " + line));
            var fullDescription = description.Length == 0 ? featureBlock : description + "\n\n" + featureBlock;
            RequireLength(folderName, "Description", fullDescription, DescriptionMin, DescriptionMax, allowNewlines: true);

            var tags = ReadLines(ReadRequired(localeDir, "keywords.txt", folderName));
            if (tags.Count == 0 || tags.Count > TagsMaxItems)
                throw new Exception($"{folderName}: keywords.txt has {tags.Count} tags; WinGet allows 1-{TagsMaxItems}.");

            var uniqueTags = new HashSet<string>(StringComparer.Ordinal);
            foreach (var tag in tags)
            {
                if (!uniqueTags.Add(tag))
                    throw new Exception($"{folderName}: duplicate tag '{tag}'.");
                RequireLength(folderName, $"Tag '{tag}'", tag, TagMin, TagMax, allowNewlines: false);
            }

            return new LocaleManifest(wingetLocale, wingetLocale == "en-US", shortDescription, fullDescription, tags);
        }

        static void WriteManifest(string outputPath, LocaleManifest manifest)
        {
            var schemaType = manifest.IsDefault ? "defaultLocale" : "locale";
            using var writer = new StreamWriter(outputPath, false, new UTF8Encoding(false));
            writer.WriteLine($"# yaml-language-server: $schema=https://aka.ms/winget-manifest.{schemaType}.1.12.0.schema.json");
            writer.WriteLine();
            writer.WriteLine("PackageIdentifier: SiarheiKuchuk.BUtil");
            writer.WriteLine("PackageVersion: APP_VERSION_STRING");
            writer.WriteLine($"PackageLocale: {manifest.Locale}");
            writer.WriteLine("Publisher: Siarhei Kuchuk");
            writer.WriteLine("PublisherUrl: https://github.com/drweb86");
            writer.WriteLine("PublisherSupportUrl: https://github.com/drweb86/butil/issues");
            writer.WriteLine("PrivacyUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/PRIVACY_POLICY.md");
            writer.WriteLine("Author: Siarhei Kuchuk");
            writer.WriteLine("PackageName: BUtil");
            writer.WriteLine("PackageUrl: https://github.com/drweb86/butil");
            writer.WriteLine("License: CC0-1.0");
            writer.WriteLine("LicenseUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/LICENSE");
            writer.WriteLine("Copyright: 2011-CURRENT_YEAR Siarhei Kuchuk");
            writer.WriteLine("CopyrightUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/LICENSE");
            writer.WriteLine($"ShortDescription: {YamlDoubleQuoted(manifest.ShortDescription)}");
            writer.WriteLine("Description: |-");
            foreach (var line in manifest.Description.Split('\n'))
                writer.WriteLine($"  {line}");
            if (manifest.IsDefault)
                writer.WriteLine("Moniker: butil");
            writer.WriteLine("Tags:");
            foreach (var tag in manifest.Tags)
                writer.WriteLine($"- {YamlDoubleQuoted(tag)}");
            writer.WriteLine("ReleaseNotesUrl: https://raw.githubusercontent.com/drweb86/butil/refs/heads/master/CHANGELOG.md");
            writer.WriteLine($"ManifestType: {schemaType}");
            writer.WriteLine("ManifestVersion: 1.12.0");
        }

        static string ToWingetLocale(string folderName)
        {
            if (PublishedLocaleOverrides.TryGetValue(folderName, out var published))
                return published;

            var parts = folderName.Split('-');
            if (parts.Length == 0
                || parts[0].Length is < 2 or > 3
                || parts.Skip(1).Any(part => part.Length is < 1 or > 8)
                || parts.Any(part => part.Any(ch => !char.IsAsciiLetter(ch))))
                throw new Exception($"Fastlane folder '{folderName}' is not a WinGet locale.");

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (i == 0)
                    parts[i] = part.ToLowerInvariant();
                else if (part.Length == 4)
                    parts[i] = char.ToUpperInvariant(part[0]) + part[1..].ToLowerInvariant();
                else
                    parts[i] = part.ToUpperInvariant();
            }

            var locale = string.Join("-", parts);
            if (locale.Length > PackageLocaleMax)
                throw new Exception($"WinGet locale '{locale}' exceeds {PackageLocaleMax} characters.");
            return locale;
        }

        static string ReadRequired(string localeDir, string fileName, string folderName)
        {
            var path = Path.Combine(localeDir, fileName);
            if (!File.Exists(path))
                throw new Exception($"{folderName}: missing {fileName}.");
            return NormalizeBlock(File.ReadAllText(path));
        }

        static List<string> ReadLines(string text) =>
            text.Split('\n').Select(line => line.Trim()).Where(line => line.Length > 0).ToList();

        static string NormalizeBlock(string text)
        {
            var lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(line => line.TrimEnd());
            return string.Join("\n", lines).Trim();
        }

        static void RequireLength(string folderName, string field, string value, int min, int max, bool allowNewlines)
        {
            foreach (var rune in value.EnumerateRunes())
            {
                var code = rune.Value;
                if (code == '\n' && allowNewlines)
                    continue;
                if (code < 0x20 || code == 0x7F)
                    throw new Exception($"{folderName}: {field} contains a control character U+{code:X4}.");
            }

            var length = value.EnumerateRunes().Count();
            if (length < min || length > max)
                throw new Exception($"{folderName}: {field} is {length} characters; WinGet allows {min}-{max}.");
        }

        sealed record LocaleManifest(
            string Locale,
            bool IsDefault,
            string ShortDescription,
            string Description,
            List<string> Tags);

        // Double-quoted: plain YAML scalars fail on embedded ':' (e.g. trailing ':' in translations).
        private static string YamlDoubleQuoted(string value)
        {
            var sb = new StringBuilder(value.Length + 2);
            sb.Append('"');
            foreach (var ch in value)
            {
                switch (ch)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '"': sb.Append("\\\""); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': break;
                    case '\t': sb.Append("\\t"); break;
                    default: sb.Append(ch); break;
                }
            }
            sb.Append('"');
            return sb.ToString();
        }
    }
}
