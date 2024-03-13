namespace BUtil.Core.Options;

public interface ITaskSchedulerService
{
    ScheduleInfo GetSchedule(string taskName);
    void Schedule(string taskName, ScheduleInfo scheduleInfo);
    void Unschedule(string taskName);
}
