using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class TasksMainMenuView : UserControl
    {
        public TasksMainMenuView()
        {
            InitializeComponent();

            var viewModel = new Controls.TasksMainMenu.TasksMainMenuViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
