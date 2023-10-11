using Avalonia.Controls;

namespace butil_ui.Controls
{
    public partial class WhatTaskView : UserControl
    {
        public WhatTaskView()
        {
            InitializeComponent();

            var viewModel = new Controls.WhatTaskViewModel();
            this.DataContext = viewModel;
            viewModel.Initialize();
        }
    }
}
