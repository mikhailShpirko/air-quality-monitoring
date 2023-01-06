namespace Monitoring.Service
{
    internal sealed class WorkerOptions
    {
        public int CollectMeasurementsIntervalSeconds { get; private set; }

        public WorkerOptions(int collectMeasurementsIntervalSeconds)
        {
            CollectMeasurementsIntervalSeconds = collectMeasurementsIntervalSeconds;
        }
    }
}
