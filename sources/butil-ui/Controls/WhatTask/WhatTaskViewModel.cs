﻿using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Misc;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentFTP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace butil_ui.Controls;

public class WhatTaskViewModel : ObservableObject
{
    public WhatTaskViewModel(
        List<SourceItemV2> items,
        List<string> fileExcludePatterns)
    {
        items.ForEach(x => Items.Add(new SourceItemV2ViewModel(x.Id, x.Target, x.IsFolder, _items)));
        FileExcludePatterns = fileExcludePatterns.Join(Environment.NewLine);
    }

    #region Labels
    public static string StorageItem_ExcludePattern_Help => PlatformSpecificExperience.Instance.GetFolderService().GetStorageItemExcludePatternHelp();
    public static string LeftMenu_What => Resources.LeftMenu_What;
    public static string SourceItem_AddFolders => Resources.SourceItem_AddFolders;
    public static string StorageItem_Field_ExcludePattern => Resources.StorageItem_Field_ExcludePattern;
    public static string StorageItem_ExcludePattern_GlobbingNetPatterns => Resources.StorageItem_ExcludePattern_GlobbingNetPatterns;
    #endregion

    #region Items

    public ObservableCollection<SourceItemV2ViewModel> _items = [];

    public ObservableCollection<SourceItemV2ViewModel> Items
    {
        get
        {
            return _items;
        }
        set
        {
            if (value == _items)
                return;
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

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
        get
        {
            return _fileExcludePatterns;
        }
        set
        {
            if (value == _fileExcludePatterns)
                return;
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

#pragma warning disable CA1822 // Mark members as static
    public void GlobbingHelpCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        ProcessHelper.ShellExecute("https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats");
    }

    #endregion
}
