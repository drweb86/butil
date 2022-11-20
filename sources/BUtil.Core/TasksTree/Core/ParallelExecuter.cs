using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Core.TasksTree.Core
{
    public class ParallelExecuter
    {
        private readonly List<Thread> _threads = new();
        private readonly ConcurrentQueue<BuTask> tasks = new ();

        public ParallelExecuter(IEnumerable<BuTask> tasks, int parallel)
        {
            tasks
                .ToList()
                .ForEach(this.tasks.Enqueue);
            for (int i = 0; i < parallel; i++)
            {
                var thread = new Thread(ExecuteThread);
                _threads.Add(thread);
                thread.Start();
            }
        }

        private void ExecuteThread()
        {
            while (tasks.TryDequeue(out var task))
            {
                task.Execute();
            }
        }

        public void Wait()
        {
            _threads
                .ForEach(x => x.Join());
        }
    }
}
