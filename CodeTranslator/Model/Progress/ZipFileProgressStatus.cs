namespace CodeTranslator.Model
{
    public struct ZipFileProgressStatus
    {
        public long CurrentExtractedBytes { get; internal set; }
        public long TotalExtractedBytes { get; internal set; }
        public double ZipFileTotalBytes { get; internal set; }
        public double ExtractionPercentage
            => (CurrentExtractedBytes + TotalExtractedBytes) / ZipFileTotalBytes;
        public string AsString
            => $"{CurrentExtractedBytes + TotalExtractedBytes} / {ZipFileTotalBytes} bytes";

        public ZipFileProgressStatus(double zipFileTotalBytes)
        {
            CurrentExtractedBytes = TotalExtractedBytes = 0L;
            ZipFileTotalBytes = zipFileTotalBytes;
        }
    }
}
