using Avalonia.Controls;
using BUtil.Core.Legal;
using System.ComponentModel;

namespace BUtil.UI.Controls;

public partial class LegalDocumentView : UserControl, IViewLocatorAware<LegalDocumentViewModel>
{
    public LegalDocumentView()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => Attach();
    }

    private void Attach()
    {
        if (DataContext is not LegalDocumentViewModel viewModel)
            return;

        viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        viewModel.PropertyChanged += OnViewModelPropertyChanged;
        Render(viewModel);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LegalDocumentViewModel.SelectedLanguage)
            && sender is LegalDocumentViewModel viewModel)
            Render(viewModel);
    }

    private void Render(LegalDocumentViewModel viewModel)
    {
        MarkdownDocumentPresenter.Render(
            this,
            DocumentHost,
            viewModel.LoadMarkdown(),
            viewModel.SelectedLanguage.Rtl);
    }
}
