using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Layout;
using Avalonia.Media;
using BUtil.Core.Legal;
using System;
using System.Collections.Generic;

namespace BUtil.UI.Controls;

internal static class MarkdownDocumentPresenter
{
    public static void Render(Control hostRoot, StackPanel host, string markdown, bool rtl)
    {
        hostRoot.FlowDirection = rtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        host.Children.Clear();

        foreach (var block in PrivacyMarkdown.Parse(markdown))
        {
            switch (block)
            {
                case PrivacyMdBlock.Heading heading:
                    host.Children.Add(new TextBlock
                    {
                        Text = heading.Text,
                        FontWeight = FontWeight.Bold,
                        FontSize = heading.Level == 1 ? 22 : heading.Level == 2 ? 18 : 16,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, heading.Level == 1 ? 4 : 16, 0, 8),
                    });
                    break;
                case PrivacyMdBlock.Paragraph paragraph:
                    host.Children.Add(CreateRichText(paragraph.Inlines, 0, 10));
                    break;
                case PrivacyMdBlock.Bullet bullet:
                    host.Children.Add(CreateRichText(bullet.Inlines, 12, 6, "•  "));
                    break;
            }
        }
    }

    private static Control CreateRichText(
        IReadOnlyList<PrivacyInline> inlines,
        double leftMargin,
        double bottomMargin,
        string? prefix = null)
    {
        var block = new TextBlock
        {
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(leftMargin, 0, 0, bottomMargin),
            FontSize = 14,
        };
        var runs = block.Inlines ?? [];
        block.Inlines = runs;
        if (!string.IsNullOrEmpty(prefix))
            runs.Add(new Run { Text = prefix });

        foreach (var inline in inlines)
        {
            switch (inline)
            {
                case PrivacyInline.Text text:
                    runs.Add(new Run
                    {
                        Text = text.Value,
                        FontWeight = text.Bold ? FontWeight.Bold : FontWeight.Normal,
                    });
                    break;
                case PrivacyInline.Link link when Uri.TryCreate(link.Url, UriKind.Absolute, out var uri):
                    runs.Add(new InlineUIContainer
                    {
                        Child = new HyperlinkButton
                        {
                            Content = link.Label,
                            NavigateUri = uri,
                            Padding = new Thickness(0),
                            FontSize = 14,
                            VerticalAlignment = VerticalAlignment.Center,
                        },
                    });
                    break;
                case PrivacyInline.Link link:
                    runs.Add(new Run { Text = link.Label });
                    break;
                case PrivacyInline.Code code:
                    runs.Add(new Run
                    {
                        Text = code.Value,
                        FontFamily = new FontFamily("Consolas, Menlo, monospace"),
                    });
                    break;
            }
        }

        return block;
    }
}
