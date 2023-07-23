namespace BUtil.ConsoleBackup.UI{
    using System.Collections.Generic;
    using System.Linq;
        
    public partial class MyView {
        private List<string> _taskNames;
        
        public MyView(IEnumerable<string> taskNames) {
            InitializeComponent();

            _taskNames = taskNames.ToList();
            this.itemsListView.SetSource(_taskNames);
        }
    }
}
