namespace MLNet;

public static class DataStatistics
{
    public static void DisplayPlayerDataStatistics(List<HeartRateData> data)
    {
        // Group playtest data by Time and calculate the average time
        var playtestDataTimeGrouped = data
            .GroupBy(x => x.Time)
            .Select(x => new { Time = x.Key, AverageTime = x.Average(y => y.Time.TotalSeconds) });

        // Calculate the average time across all playtest data
        var averageTime = playtestDataTimeGrouped.Average(x => x.AverageTime);
        Console.WriteLine("Average time: " + averageTime);

        // Group playtest data by Bpm and calculate the average BPM
        var playtestDataBPMGrouped = data
            .GroupBy(x => x.Bpm)
            .Select(x => new { Bpm = x.Key, AverageBPM = x.Average(y => y.Bpm) });

        // Calculate the average BPM across all playtest data
        var averageBPM = playtestDataBPMGrouped.Average(x => x.AverageBPM);
        Console.WriteLine("Average BPM: " + averageBPM);
    }
}