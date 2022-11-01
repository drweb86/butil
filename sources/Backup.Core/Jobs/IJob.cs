using System;
using BUtil.Core.Misc;

namespace BUtil.Core.Jobs
{
    internal interface IJob
    {
        void DoJob();

        EventHandler<JobThreadEventArgs> TaskFinished
        {
            get;
            set;
        }
    }
}
