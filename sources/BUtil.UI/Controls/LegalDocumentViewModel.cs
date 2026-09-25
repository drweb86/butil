using BUtil.Core.Legal;
using BUtil.Core.Localization;
using System.Collections.Generic;

namespace BUtil.UI.Controls;

public enum LegalDocumentKind
{
    Privacy,
    License,
}

public class LegalDocumentViewModel : ViewModelBase
{
    private readonly LegalDocumentKind _kind;

    public IReadOnlyList<DocumentLanguage> Languages { get; }
    public string LanguageLabel => Resources.Document_Language;
    public string CloseLabel => Resources.Button_Close;
    public string ThirdPartyNote => Resources.License_ThirdPartyNote;
    public bool ShowThirdPartyNote => _kind == LegalDocumentKind.License;

    private DocumentLanguage _selectedLanguage;

    public DocumentLanguage SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (value == _selectedLanguage)
                return;
            _selectedLanguage = value;
            DocumentLanguageStore.SetLanguageCode(value.Code);
            OnPropertyChanged(nameof(SelectedLanguage));
        }
    }

    public LegalDocumentViewModel(LegalDocumentKind kind)
    {
        _kind = kind;
        Languages = kind == LegalDocumentKind.Privacy
            ? DocumentLanguages.Privacy
            : DocumentLanguages.License;
        WindowTitle = kind == LegalDocumentKind.Privacy
            ? Resources.Menu_Privacy
            : Resources.Menu_License;
        IsFullMenuVisible = true;

        var saved = DocumentLanguageStore.LanguageCode();
        _selectedLanguage = saved != null
            ? DocumentLanguages.ByCode(Languages, saved)
            : DocumentLanguages.MatchDevice(Languages);
        if (saved == null)
            DocumentLanguageStore.SetLanguageCode(_selectedLanguage.Code);
    }

    public string LoadMarkdown() =>
        _kind == LegalDocumentKind.Privacy
            ? PrivacyDocuments.LoadMarkdown(SelectedLanguage.AssetFile)
            : LicenseDocuments.LoadMarkdown(SelectedLanguage.AssetFile);

    public void CloseCommand()
    {
        WindowManager.SwitchView(new TasksViewModel());
    }
}
