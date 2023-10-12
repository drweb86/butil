using BUtil.Core.Localization;
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
    }
}
