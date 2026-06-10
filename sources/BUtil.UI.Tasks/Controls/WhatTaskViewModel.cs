using BUtil.Interop.Tasks;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BUtil.UI.Controls;

public class WhatTaskViewModel : ObservableObject
{
    private readonly ObservableCollection<SourceItemV2ViewModel> _items = [];

    public WhatTaskViewModel(
        List<SourceItemV2> items,
        List<string> fileExcludePatterns,
        bool isExpanded = false)
    {
        IsExpanded = isExpanded;
        items.ForEach(x => _items.Add(new SourceItemV2ViewModel(x.Id, x.Target, x.IsFolder, _items)));
        FileExcludePatterns = string.Join(Environment.NewLine, fileExcludePatterns);
    }

    public bool IsExpanded { get; }

    public void AddFolder(string path)
    {
        _items.Add(new SourceItemV2ViewModel(Guid.NewGuid(), path, true, _items));
    }

    #region Labels
    public static string StorageItem_ExcludePattern_Help => PlatformSpecificExperience.Instance.GetFolderService().GetStorageItemExcludePatternHelp();
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string SourceItem_AddFolders => Resources.SourceItem_AddFolders;
    public static string StorageItem_ExcludePattern_ExpanderHeader => Resources.StorageItem_ExcludePattern_ExpanderHeader;
    public static string StorageItem_Field_ExcludePattern => Resources.StorageItem_Field_ExcludePattern;
    public static string StorageItem_ExcludePattern_GlobbingNetPatterns => Resources.StorageItem_ExcludePattern_GlobbingNetPatterns;
    #endregion

    #region Items

    public ObservableCollection<SourceItemV2ViewModel> Items => _items;

    public List<SourceItemV2> GetListSourceItemV2s()
    {
        return _items
            .Select(x => new SourceItemV2(x.Target, x.IsFolder) { Id = x.Id })
            .ToList();
    }

    #endregion

    #region FileExcludePatterns

    private string _fileExcludePatterns = string.Empty;

    public string FileExcludePatterns
    {
        get { return _fileExcludePatterns; }
        set
        {
            if (value == _fileExcludePatterns) return;
            _fileExcludePatterns = value;
            OnPropertyChanged(nameof(FileExcludePatterns));
        }
    }

    public List<string> GetListFileExcludePatterns()
    {
        return FileExcludePatterns
                    .Split(Environment.NewLine)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
    }

    #endregion

    #region Commands

#pragma warning disable CA1822
    public void GlobbingHelpCommand()
#pragma warning restore CA1822
    {
        ProcessHelper.ShellExecute("https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats");
    }

    #endregion
}
