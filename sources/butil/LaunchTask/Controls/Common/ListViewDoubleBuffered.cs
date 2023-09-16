using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BUtil.Configurator.Configurator.Controls.Common
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ListViewDoubleBuffered))]
    public class ListViewDoubleBuffered : ListView
    {
        public ListViewDoubleBuffered()
        {
            this.DoubleBuffered = true;
        }
    }
}
