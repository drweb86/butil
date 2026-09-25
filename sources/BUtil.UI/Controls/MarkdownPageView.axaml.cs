using Avalonia.Controls;

namespace BUtil.UI.Controls;

public partial class MarkdownPageView : UserControl, IViewLocatorAware<MarkdownPageViewModel>
{
    public MarkdownPageView()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => Render();
    }

    private void Render()
    {
        if (DataContext is not MarkdownPageViewModel viewModel)
            return;

        MarkdownDocumentPresenter.Render(this, DocumentHost, viewModel.Markdown, rtl: false);
    }
}
