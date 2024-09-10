using Microsoft.ML;

namespace MLNet;

public class GamePrediction
{
    private readonly float _minBpm;
    private readonly float _maxBpm;
    private readonly float _minDifficulty;
    private readonly float _maxDifficulty;

    // Constructor to initialize BPM and difficulty ranges
    public GamePrediction(float minBpm, float maxBpm, float minDifficulty, float maxDifficulty)
    {
        _minBpm = minBpm;
        _maxBpm = maxBpm;
        _minDifficulty = minDifficulty;
        _maxDifficulty = maxDifficulty;
    }

    // Method to predict the game difficulty
    public void PredictGameDifficulty(PredictionEngine<HeartRateData, HeartRatePrediction>? predictionEngine, HeartRateData newData)
    {
        // Predict intensity using the model
        float predictedIntensity = predictionEngine.Predict(newData).Intensity;

        // Normalize the BPM to a 0-1 scale based on min and max BPM
        float normalizedBpm = (newData.Bpm - _minBpm) / (_maxBpm - _minBpm);

        // Invert the predicted intensity so that lower intensity gives higher difficulty
        float invertedIntensity = 1 - predictedIntensity;

        // Scale the inverted intensity to the game's difficulty range
        float scaledDifficulty = _minDifficulty + invertedIntensity * (_maxDifficulty - _minDifficulty);

        // Adjust difficulty based on normalized BPM
        float finalDifficulty = _minDifficulty + (scaledDifficulty * (1 - normalizedBpm) * (_maxDifficulty - _minDifficulty));

        // Output the predicted intensity and game difficulty
        Console.WriteLine($"BPM: {newData.Bpm}");
        Console.WriteLine($"Time: {newData.Time}");
        Console.WriteLine($"Predicted Intensity: {predictedIntensity}");
        Console.WriteLine($"Normalized BPM: {normalizedBpm}");
        Console.WriteLine($"Game Difficulty: {finalDifficulty}");
        Console.WriteLine("----");
    }
}