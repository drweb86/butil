using System.Collections.Concurrent;

namespace BUtil.Interop.Tasks.Core;

public class ParallelExecuter
{
    private readonly List<Thread> _threads = [];
    private readonly ConcurrentQueue<BuTask> _tasks = new();

    public ParallelExecuter(IEnumerable<BuTask> tasks, int parallel)
    {
        foreach (var task in tasks)
            _tasks.Enqueue(task);
        for (var i = 0; i < parallel; i++)
        {
            var thread = new Thread(ExecuteThread);
            _threads.Add(thread);
            thread.Start();
        }
    }

    private void ExecuteThread()
    {
        while (_tasks.TryDequeue(out var task))
            task.Execute();
    }

    public void Wait() => _threads.ForEach(x => x.Join());
}
