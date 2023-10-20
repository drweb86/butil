using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BUtil.Core;
using BUtil.Core.ConfigurationFileModels.V1;
using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using BUtil.Core.State;
using System.Linq;
using System.Collections.ObjectModel;
using Avalonia.Data.Converters;
using Avalonia;
using System.Globalization;
using System.Reflection;
using Avalonia.Media;
using butil_ui.ViewModels;

namespace butil_ui.Controls
{
    public class VersionsListViewModel : ObservableObject
    {
        #region Labels

        public string Field_Version => Resources.Field_Version;
        public string BackupVersion_Changes_Title => Resources.BackupVersion_Changes_Title;

        #endregion

        #region SelectedVersion

        private VersionViewItem _selectedVersion = new VersionViewItem(new VersionState());

        public VersionViewItem SelectedVersion
        {
            get
            {
                return _selectedVersion;
            }
            set
            {
                if (value == _selectedVersion)
                    return;
                _selectedVersion = value;
                OnPropertyChanged(nameof(SelectedVersion));
                if (value != null)
                    OnVersionChanged();
            }
        }

        #endregion

        #region Versions

        private ObservableCollection<VersionViewItem> _versions = new();

        public ObservableCollection<VersionViewItem> Versions
        {
            get
            {
                return _versions;
            }
            set
            {
                if (value == _versions)
                    return;
                _versions = value;
                OnPropertyChanged(nameof(Versions));
            }
        }

        #endregion

        #region StorageSize

        private string _storageSize = string.Empty;

        public string StorageSize
        {
            get
            {
                return _storageSize;
            }
            set
            {
                if (value == _storageSize)
                    return;
                _storageSize = value;
                OnPropertyChanged(nameof(StorageSize));
            }
        }

        #endregion

        #region FileChangeViewItems

        private ObservableCollection<FileChangeViewItem> _fileChangeViewItems = new();

        public ObservableCollection<FileChangeViewItem> FileChangeViewItems
        {
            get
            {
                return _fileChangeViewItems;
            }
            set
            {
                if (value == _fileChangeViewItems)
                    return;
                _fileChangeViewItems = value;
                OnPropertyChanged(nameof(FileChangeViewItems));
            }
        }

        #endregion

        public SolidColorBrush HeaderBackground { get; } = new SolidColorBrush(ColorPalette.GetColor(SemanticColor.HeaderBackground));

        public ProgressTaskViewModel ProgressTaskViewModel { get; } = new ProgressTaskViewModel();

        private IncrementalBackupState _state;
        public void Initialize(IncrementalBackupState state)
        {
            _state = state;
            Versions = new ObservableCollection<VersionViewItem>(state.VersionStates
                .OrderByDescending(x => x.BackupDateUtc)
                .Select(x => new VersionViewItem(x))
                .ToList());

            SelectedVersion = Versions.First();

            var storageSize = state.VersionStates
                .SelectMany(x => x.SourceItemChanges)
                .SelectMany(x =>
                {
                    var storageFiles = new List<StorageFile>();
                    storageFiles.AddRange(x.UpdatedFiles);
                    storageFiles.AddRange(x.CreatedFiles);
                    return storageFiles;
                })
                .GroupBy(x => x.StorageFileName)
                .Select(x => x.First().StorageFileNameSize)
                .Sum();

            StorageSize = string.Format(BUtil.Core.Localization.Resources.BackupVersion_Storage_TitleWithSize, SizeHelper.BytesToString(storageSize));
        }

        private void OnVersionChanged()
        {
            ProgressTaskViewModel.Activate(async reportProgress =>
            {
                reportProgress(0);
                ProgressTaskViewModel.IsVisible = true;
                var changes = GetChangesViewItems(SelectedVersion.Version);
                reportProgress(25);
                var treeViewFiles = GetTreeViewFiles(_state, SelectedVersion.Version);
                reportProgress(45);
                RefreshChanges(changes);
                reportProgress(85);
                // RefreshTreeView(treeViewFiles);
                reportProgress(100);
                await Task.Delay(3000);
                ProgressTaskViewModel.IsVisible = false;
            });
        }

        private void RefreshChanges(IEnumerable<Tuple<ChangeState, string>> changes)
        {
            FileChangeViewItems.Clear();

            changes
                .Select(x => new FileChangeViewItem(x.Item2, x.Item1))
                .ToList()
                .ForEach(FileChangeViewItems.Add);
        }

        private static List<Tuple<SourceItemV2, List<StorageFile>>> GetTreeViewFiles(
            IncrementalBackupState state,
            VersionState selectedVersion)
        {
            var result = new List<Tuple<SourceItemV2, List<StorageFile>>>();

            var sourceItems = selectedVersion.SourceItemChanges
                .Select(a => a.SourceItem)
                .OrderBy(x => x.Target)
                .ToList();

            foreach (var sourceItem in sourceItems)
            {
                result.Add(new Tuple<SourceItemV2, List<StorageFile>>(
                    sourceItem,
                    BuildVersionFiles(state, sourceItem, selectedVersion)
                    ));
            }

            return result;
        }

        private static List<StorageFile> BuildVersionFiles(IncrementalBackupState state, SourceItemV2 sourceItem, VersionState selectedVersion)
        {
            List<StorageFile> result = null;

            foreach (var versionState in state.VersionStates)
            {
                var sourceItemChanges = versionState.SourceItemChanges.FirstOrDefault(x => x.SourceItem.CompareTo(sourceItem));
                if (sourceItemChanges == null)
                {
                    result = null;
                }
                else
                {
                    if (result == null)
                    {
                        result = sourceItemChanges.CreatedFiles.ToList();
                    }
                    else
                    {
                        result.AddRange(sourceItemChanges.CreatedFiles);
                        foreach (var deletedFile in sourceItemChanges.DeletedFiles)
                        {
                            var itemToRemove = result.First(x => x.FileState.FileName == deletedFile);
                            result.Remove(itemToRemove);
                        }
                        foreach (var updatedFile in sourceItemChanges.UpdatedFiles)
                        {
                            var itemToRemove = result.First(x => x.FileState.FileName == updatedFile.FileState.FileName);
                            result.Remove(itemToRemove);

                            result.Add(updatedFile);
                        }
                    }
                }

                if (versionState == selectedVersion)
                    break;
            }

            return result
                .OrderBy(x => x.FileState.FileName)
                .ToList();
        }

        private static IEnumerable<Tuple<ChangeState, string>> GetChangesViewItems(VersionState state)
        {
            var result = new List<Tuple<ChangeState, string>>();

            foreach (var sourceItemChanges in state.SourceItemChanges
                .OrderBy(x => x.SourceItem.Target)
                .ToList())
            {
                if (!sourceItemChanges.CreatedFiles.Any() &&
                    !sourceItemChanges.UpdatedFiles.Any() &&
                    !sourceItemChanges.DeletedFiles.Any())
                    continue;

                sourceItemChanges.UpdatedFiles
                    .OrderBy(x => x.FileState.FileName)
                    .ToList()
                    .ForEach(updateFile => result.Add(new Tuple<ChangeState, string>(ChangeState.Updated, updateFile.FileState.FileName)));

                sourceItemChanges.CreatedFiles
                    .OrderBy(x => x.FileState.FileName)
                    .ToList()
                    .ForEach(updateFile => result.Add(new Tuple<ChangeState, string>(ChangeState.Created, updateFile.FileState.FileName)));

                sourceItemChanges.DeletedFiles
                    .OrderBy(x => x)
                    .ToList()
                    .ForEach(deletedFile => result.Add(new Tuple<ChangeState, string>(ChangeState.Deleted, deletedFile)));
            }
            return result;
        }

        public string Title { get; }
        public string DirectoryStorage => Resources.DirectoryStorage;

        #region Labels
        public string LeftMenu_Where => Resources.LeftMenu_Where;
        public string DataStorage_Field_UploadQuota => Resources.DataStorage_Field_UploadQuota;
        public string DataStorage_Field_UploadQuota_Help => Resources.DataStorage_Field_UploadQuota_Help;
        public string DataStorage_Script_Help => Resources.DataStorage_Script_Help;
        public string DataStorage_Field_ConnectScript => Resources.DataStorage_Field_ConnectScript;
        public string DataStorage_Field_DisconnectionScript => Resources.DataStorage_Field_DisconnectionScript;
        public string Field_Folder => Resources.Field_Folder;

        public string Url_Field => Resources.Url_Field;
        public string User_Field => Resources.User_Field;
        public string Password_Field => Resources.Password_Field;
        public string Server_Field_Address => Resources.Server_Field_Address;
        public string Server_Field_Port => Resources.Server_Field_Port;
        public string Ftps_Field_Encryption => Resources.Ftps_Field_Encryption;
        public string Field_Device => Resources.Field_Device;
        public string Field_Folder_Browse => Resources.Field_Folder_Browse;
        public string Task_Launch => Resources.Task_Launch;
        public string Field_TransportProtocol => Resources.Field_TransportProtocol;
        #endregion

    }

    public class FileChangeViewItem
    {
        public FileChangeViewItem(string title, ChangeState state)
        {
            Title = title;

            switch (state)
            {
                case ChangeState.Created:
                    ImageSource = "/Assets/VC-Created.png";
                    break;
                case ChangeState.Deleted:
                    ImageSource = "/Assets/VC-Deleted.png";
                    break;
                case ChangeState.Updated:
                    ImageSource = "/Assets/VC-Updated.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }

        }
        public string Title { get; }
        public string ImageSource { get; }
    }


    public class VersionViewItem
    {
        public VersionViewItem(VersionState version)
        {
            var totalSize = GetSizeOfVersion(version);
            Title = $"{version.BackupDateUtc} ({SizeHelper.BytesToString(totalSize)})";
            Version = version;
        }

        public string Title { get; }
        public VersionState Version { get; }

        private static long GetSizeOfVersion(VersionState version)
        {
            var versionFolder = SourceItemHelper.GetVersionFolder(version.BackupDateUtc);
            return version.SourceItemChanges
                .SelectMany(x =>
                {
                    var storageFiles = new List<StorageFile>();
                    storageFiles.AddRange(x.UpdatedFiles);
                    storageFiles.AddRange(x.CreatedFiles);
                    return storageFiles;
                })
                .Where(x => x.StorageRelativeFileName.StartsWith(versionFolder))
                .GroupBy(x => x.StorageFileName)
                .Select(x => x.First().StorageFileNameSize)
                .Sum();
        }
    }

    static class SizeHelper
    {
        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }

    public enum ChangeState
    {
        Created = 3, // match image indexes
        Deleted = 4,
        Updated = 5
    }

    public class BitmapAssetValueConverter : IValueConverter
    {
        public static BitmapAssetValueConverter Instance = new BitmapAssetValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is string rawUri)
            {
                Uri uri;

                // Allow for assembly overrides
                if (rawUri.StartsWith("avares://"))
                {
                    uri = new Uri(rawUri);
                }
                else
                {
                    string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                    uri = new Uri($"avares://{assemblyName}{rawUri}");
                }

                var asset = AssetLoader.Open(uri);

                return new Bitmap(asset);
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
