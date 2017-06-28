using System.Drawing;
using BUtil.Core.PL;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    class PageInfo
    {
        #region Properties

        public string Title { get; private set; }
        public string Description { get; private set; }
        public BackUserControl ControlToShow { get; private set; }
        public Bitmap Image { get; private set; }

        #endregion

        #region Constructors

        public PageInfo(string title, string description, BackUserControl controlToShow, Bitmap image)
        {
            Title = title;
            Description = description;
            ControlToShow = controlToShow;
            Image = image;
        }

        #endregion
    }
}
