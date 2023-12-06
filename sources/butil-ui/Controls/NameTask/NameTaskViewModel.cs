using BUtil.Core.Localization;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace butil_ui.Controls
{
    public class NameTaskViewModel : ObservableObject
    {
        public NameTaskViewModel(bool isExpanded, string help, string name)
        {
            Help = help;
            IsExpanded = isExpanded;
            _name = name;
        }

        #region Labels
        public string Name_Title => Resources.Name_Title;
        public string Name_Field => Resources.Name_Field;
        public string Icons_Help_Link => Resources.Icons_Help_Link;
        public string Help { get; }

        #endregion

        public bool IsExpanded { get; }

        #region Name

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        #endregion

        #region Commands

        public void OpenCharsPageCommand()
        {
            ProcessHelper.ShellExecute(@"https://github.com/drweb86/butil/blob/master/help/Icons.md");
        }

        #endregion
    }
}
