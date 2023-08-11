using System.Collections;
using System.Collections.Generic;
using Terminal.Gui;

namespace BUtil.ConsoleBackup.UI
{
    class ListViewItemDataSource : List<ListViewItem>, IListDataSource
    {
        public int Length => Count;

        public bool IsMarked(int item)
        {
            return false;
        }

        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width, int start = 0)
        {
        }

        public void SetMark(int item, bool value)
        {
            // choosing not to support marking
        }

        public IList ToList()
        {
            return this;
        }
    }
}
