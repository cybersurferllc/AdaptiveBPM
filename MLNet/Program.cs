// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Microsoft.ML;
using MLNet;

Console.WriteLine("Hello, World!");

var featureColumnName = "Features";
var labelColumnName = "Intensity";
var inputColumnNames = typeof(HeartRateData)
    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
    .Where(prop => prop.PropertyType == typeof(float))
    .Select(prop => prop.Name)
    .ToArray();
var modelFileName = "model.zip";
var context = new MLContext();

// Load data
var testData = CSV.ReadCSVFiles<HeartRateData>().ToList();
var playtestData = CSV.ReadCSVFiles<HeartRateData>("master").ToList();
testData.AddRange(playtestData);

Console.WriteLine($"Data count: {testData.Count}");

// Transform data to DataView
var gameDataView = context.Data.LoadFromEnumerable(testData);
var split = context.Data.TrainTestSplit(gameDataView, testFraction: 0.2);

// Build and train model
var pipeline = context.Transforms.Concatenate(featureColumnName, inputColumnNames)
    //.Append(context.Transforms.NormalizeMinMax(featureColumnName)) // Feature scaling
    .Append(context.Regression.Trainers.FastTree(labelColumnName: labelColumnName)); // Using a more complex model
var model = pipeline.Fit(gameDataView);

// Evaluate model
var predictions = model.Transform(gameDataView);
var metrics = context.Regression.Evaluate(predictions, labelColumnName: labelColumnName);

// write model to zip
context.Model.Save(model, gameDataView.Schema, modelFileName);

Console.WriteLine($"R^2: {metrics.RSquared}");
Console.WriteLine($"RMS: {metrics.RootMeanSquaredError}");

// Predict
var predictionEngine = context.Model.CreatePredictionEngine<HeartRateData, HeartRatePrediction>(model);
var prediction = predictionEngine.Predict(new HeartRateData { Bpm = 140, Time = new TimeSpan(0,0,1,30)});

Console.WriteLine($"Predicted Intensity: {prediction.Intensity}");

var heartRateData = new List<HeartRateData>()
{
    // random heart rate data
    new HeartRateData { Bpm = 140, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 120, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 100, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 80, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 60, Time = new TimeSpan(0, 0, 1, 30) },
    new HeartRateData { Bpm = 120, Time = new TimeSpan(0, 0, 0, 5) },
};

foreach (var record in heartRateData){
    var predictedIntensity = predictionEngine.Predict(record).Intensity;
    Console.WriteLine($"BPM Data: {record.Bpm}, Time: {record.Time}, Predicted Intensity: {predictedIntensity}");
}

var transformer = MLExtensions.GetMLContext(ref context, modelFileName);
if (transformer != null){
    Console.WriteLine("Model loaded successfully");
}
var predictionEngine2 = MLExtensions.Predict(context, transformer, new HeartRateData(){Bpm = 120, Time = new TimeSpan(0,0,1,30)});
Console.WriteLine($"Predicted Intensity: {predictionEngine2}");

Console.ReadLine();
