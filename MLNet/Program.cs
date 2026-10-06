// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using MLNet;

Console.WriteLine("Hello, World!");

var featureColumnName = "Features";
var labelColumnName = "Intensity";
var inputColumnNames = new[]
{
    nameof(HeartRateData.Bpm),
    nameof(HeartRateData.TimeInSeconds)
};
var modelFileName = "model.zip";
var context = new MLContext();

// Load data
var testData = CSV.ReadCSVFiles<HeartRateData>().ToList();
var playtestData = CSV.ReadCSVFiles<HeartRateData>("master").ToList();
testData.AddRange(playtestData);

var maxBpm = testData.Max(x => x.Bpm);
var minBpm = testData.Min(x => x.Bpm);
var maxIntensity = 100;
var minIntensity = testData.Min(x => x.Intensity);

Console.WriteLine($"Data count: {testData.Count}");

// Transform data to DataView
var gameDataView = context.Data.LoadFromEnumerable(testData);
var split = context.Data.TrainTestSplit(gameDataView, testFraction: 0.2);

// Build and train model
var pipeline = context.Transforms.Concatenate(featureColumnName, inputColumnNames)
    .Append(context.Transforms.NormalizeMinMax(featureColumnName))
    .Append(context.Regression.Trainers.FastTree(labelColumnName: labelColumnName, featureColumnName: featureColumnName));
var model = pipeline.Fit(split.TrainSet);

// Evaluate model
var predictions = model.Transform(split.TestSet);
var metrics = context.Regression.Evaluate(predictions, labelColumnName: labelColumnName);

// write model to zip
context.Model.Save(model, split.TrainSet.Schema, modelFileName);

// Model metrics
Console.WriteLine($"R^2: {metrics.RSquared}");
Console.WriteLine($"RMS: {metrics.RootMeanSquaredError}");

// Predict
var predictionEngine = context.Model.CreatePredictionEngine<HeartRateData, HeartRatePrediction>(model);
var prediction = predictionEngine.Predict(new HeartRateData { Bpm = 140, Time = new TimeSpan(0,0,1,30)});
Console.WriteLine($"Predicted Intensity: {prediction.Intensity}");

// Random Heart rate data
var heartRateData = new List<HeartRateData>()
{
    new HeartRateData { Bpm = 140, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 120, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 100, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 80, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 60, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 120, Time = new TimeSpan(0, 0, 0, 5) },
};

GamePrediction gamePrediction = new GamePrediction(predictionEngine, minBpm, maxBpm, minIntensity, maxIntensity);
foreach (var record in heartRateData)
{
    gamePrediction.PredictGameDifficulty(record);
}

string input = null;
do
{
    input = Console.ReadLine();
    if (input == "exit")
    {
        break;
    }

    var bpm = float.Parse(input);
    var time = new TimeSpan(0,0,1,30);
    gamePrediction.PredictGameDifficulty(new HeartRateData() { Bpm = bpm, Time = time });
} while (true);

DataStatistics.DisplayPlayerDataStatistics(playtestData);
