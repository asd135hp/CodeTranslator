namespace CodeTranslator.Utility
{
    public struct ProgressStatus
    {
        public int TasksCompleted { get; internal set; }
        public int TotalTaskCount { get; internal set; }

        public double Percentage => TotalTaskCount == 0 ? 0 : TasksCompleted / TotalTaskCount;
        public string AsString => $"{TasksCompleted}/{TotalTaskCount}";

        public ProgressStatus(int totalTaskCount = 0)
        {
            TasksCompleted = 0;
            TotalTaskCount = totalTaskCount;
        }
    }
}
