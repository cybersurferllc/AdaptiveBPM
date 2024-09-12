using Microsoft.ML;

namespace MLNet;

public class GamePrediction
{
    private readonly float _minBpm;
    private readonly float _maxBpm;
    private readonly float _minDifficulty;
    private readonly float _maxDifficulty;
    private readonly PredictionEngine<HeartRateData, HeartRatePrediction>? _predictionEngine;

    // Constructor to initialize BPM and difficulty ranges
    public GamePrediction(
        PredictionEngine<HeartRateData, HeartRatePrediction>? predictionEngine,
        float minBpm,
        float maxBpm,
        float minDifficulty,
        float maxDifficulty)
    {
        _predictionEngine = predictionEngine;
        _minBpm = minBpm;
        _maxBpm = maxBpm;
        _minDifficulty = minDifficulty;
        _maxDifficulty = maxDifficulty;
    }

    // Method to predict the game difficulty
    public void PredictGameDifficulty(HeartRateData newData)
    {
        // Predict intensity using the model
        float predictedIntensity = _predictionEngine.Predict(newData).Intensity;

        // Normalize the predicted intensity to a 0-1 scale
        float normalizedIntensity = (predictedIntensity - _minDifficulty) / (_maxDifficulty - _minDifficulty);

        // Normalize the BPM to a 0-1 scale based on min and max BPM
        float normalizedBpm = (newData.Bpm - _minBpm) / (_maxBpm - _minBpm);

        // Invert the normalized intensity so that lower intensity gives higher difficulty
        float invertedIntensity = 1 - normalizedIntensity;

        // Scale the inverted intensity to the game's difficulty range
        float scaledDifficulty = _minDifficulty + invertedIntensity * (_maxDifficulty - _minDifficulty);

        // Adjust difficulty based on normalized BPM
        float finalDifficulty = scaledDifficulty * (1 - normalizedBpm);

        // Ensure final difficulty is within the range
        finalDifficulty = Math.Max(_minDifficulty, Math.Min(finalDifficulty, _maxDifficulty));

        // Output the predicted intensity and game difficulty
        Console.WriteLine($"BPM: {newData.Bpm}");
        Console.WriteLine($"Time: {newData.Time}");
        Console.WriteLine($"Predicted Intensity: {predictedIntensity}");
        Console.WriteLine($"Normalized BPM: {normalizedBpm}");
        Console.WriteLine($"Game Difficulty: {finalDifficulty}");
        Console.WriteLine("----");
    }
}