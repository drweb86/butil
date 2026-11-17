using BUtil.Core.Options;

namespace BUtil.Tests.Options;

[TestClass]
public sealed class ScheduleInfoTests
{
    [TestMethod]
    public void NewScheduleUsesCurrentHourWithoutMinutes()
    {
        var before = DateTime.Now;
        var schedule = new ScheduleInfo();
        var after = DateTime.Now;

        Assert.AreEqual(0, schedule.Time.Minutes);
        Assert.AreEqual(0, schedule.Time.Seconds);
        Assert.IsTrue(schedule.Time.Hours == before.Hour || schedule.Time.Hours == after.Hour);
        Assert.AreEqual(0, schedule.Days.Count);
    }
}
