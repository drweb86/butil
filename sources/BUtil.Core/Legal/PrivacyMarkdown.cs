using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BUtil.Core.Legal;

public abstract record PrivacyMdBlock
{
    public sealed record Heading(string Text, int Level) : PrivacyMdBlock;
    public sealed record Paragraph(IReadOnlyList<PrivacyInline> Inlines) : PrivacyMdBlock;
    public sealed record Bullet(IReadOnlyList<PrivacyInline> Inlines) : PrivacyMdBlock;
}

public abstract record PrivacyInline
{
    public sealed record Text(string Value, bool Bold = false) : PrivacyInline;
    public sealed record Link(string Label, string Url) : PrivacyInline;
    public sealed record Code(string Value) : PrivacyInline;
}

public static class PrivacyMarkdown
{
    public static IReadOnlyList<PrivacyMdBlock> Parse(string src)
    {
        var blocks = new List<PrivacyMdBlock>();
        var paragraph = new StringBuilder();

        void FlushParagraph()
        {
            var text = paragraph.ToString().Trim();
            paragraph.Clear();
            if (text.Length > 0)
                blocks.Add(new PrivacyMdBlock.Paragraph(ParseInlines(text)));
        }

        foreach (var raw in src.Replace("\r\n", "\n").Split('\n'))
        {
            var trimmed = raw.Trim();
            if (trimmed.Length == 0 || trimmed == "[Languages](README.md)")
            {
                FlushParagraph();
                continue;
            }

            if (trimmed.StartsWith("### ", StringComparison.Ordinal))
            {
                FlushParagraph();
                blocks.Add(new PrivacyMdBlock.Heading(trimmed[4..].Trim(), 3));
            }
            else if (trimmed.StartsWith("## ", StringComparison.Ordinal))
            {
                FlushParagraph();
                blocks.Add(new PrivacyMdBlock.Heading(trimmed[3..].Trim(), 2));
            }
            else if (trimmed.StartsWith("# ", StringComparison.Ordinal))
            {
                FlushParagraph();
                blocks.Add(new PrivacyMdBlock.Heading(trimmed[2..].Trim(), 1));
            }
            else if (trimmed.StartsWith("- ", StringComparison.Ordinal) || trimmed.StartsWith("* ", StringComparison.Ordinal))
            {
                FlushParagraph();
                blocks.Add(new PrivacyMdBlock.Bullet(ParseInlines(trimmed[2..].Trim())));
            }
            else
            {
                if (paragraph.Length > 0)
                    paragraph.Append(' ');
                paragraph.Append(trimmed);
            }
        }

        FlushParagraph();
        return blocks;
    }

    public static IReadOnlyList<PrivacyInline> ParseInlines(string text)
    {
        var result = new List<PrivacyInline>();
        var regex = new Regex(
            @"\*\*(.+?)\*\*|\[([^\]]+)\]\(([^)]+)\)|`([^`]+)`",
            RegexOptions.Singleline);
        var index = 0;
        foreach (Match match in regex.Matches(text))
        {
            if (match.Index > index)
                result.Add(new PrivacyInline.Text(text[index..match.Index]));

            if (match.Groups[1].Success)
                result.Add(new PrivacyInline.Text(match.Groups[1].Value, Bold: true));
            else if (match.Groups[2].Success)
                result.Add(new PrivacyInline.Link(match.Groups[2].Value, match.Groups[3].Value));
            else
            {
                var code = match.Groups[4].Value;
                if (code.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                    || code.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    result.Add(new PrivacyInline.Link(code, code));
                else
                    result.Add(new PrivacyInline.Code(code));
            }

            index = match.Index + match.Length;
        }

        if (index < text.Length)
            result.Add(new PrivacyInline.Text(text[index..]));

        return result;
    }
}
