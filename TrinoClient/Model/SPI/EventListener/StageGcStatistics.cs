using Newtonsoft.Json;

namespace TrinoClient.Model.SPI.EventListener
{
    /// <summary>
    /// From com.facebook.presto.spi.eventlistener.StageGcStatistics.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.spi.eventlistener.StageGcStatistics.java
                                 /// </summary>
    public class StageGcStatistics(
        int stageId,
        int tasks,
        int fullGcTasks,
        int minFullGcSec,
        int maxFullGcSec,
        int totalFullGcSec,
        int averageFullGcSec
            )
    {
        #region Public Properties

        public int StageId { get; } = stageId;

        public int Tasks { get; } = tasks;

        public int FullGcTasks { get; } = fullGcTasks;

        public int MinFullGcSec { get; } = minFullGcSec;

        public int MaxFullGcSec { get; } = maxFullGcSec;

        public int TotalFullGcSec { get; } = totalFullGcSec;

        public int AverageFullGcSec { get; } = averageFullGcSec;

        #endregion
        #region Constructors 

        #endregion
    }
}
