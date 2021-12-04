namespace CodeTranslator.Model
{
    public struct ProgressStatus
    {
        public int TasksCompleted { get; set; }

        public int TotalTaskCount { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCancelled { get; set; }

        public double Percentage => TotalTaskCount == 0 ? 0 : TasksCompleted / TotalTaskCount;

        public string AsString => $"{TasksCompleted}/{TotalTaskCount}";

        public ProgressStatus(int totalTaskCount = 0)
        {
            TasksCompleted = 0;
            TotalTaskCount = totalTaskCount;
            IsCompleted = IsCancelled = false;
        }
    }
}
