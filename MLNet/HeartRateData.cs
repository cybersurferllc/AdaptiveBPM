using Microsoft.ML.Data;

namespace MLNet;

public class HeartRateData
{
    [LoadColumn(0)]
    public float Intensity { get; set; }
    [LoadColumn(1)]
    public float Bpm { get; set; }
    [LoadColumn(2)]
    public TimeSpan Time { get; set; }
    [ColumnName("TimeInSeconds")]
    public float TimeInSeconds => (float)Time.TotalSeconds;
}

public class HeartRatePrediction
{
    [ColumnName("Score")]
    public float Intensity { get; set; }
}