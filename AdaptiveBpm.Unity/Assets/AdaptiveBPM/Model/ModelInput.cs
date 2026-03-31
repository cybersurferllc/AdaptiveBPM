using Microsoft.ML.Data;

namespace AdaptiveBpmML
{
    /// <summary>
    /// model input class for AdaptiveBpmMLModel.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [ColumnName(@"Intensity")]
        [LoadColumn(0)]
        public float Intensity { get; set; }

        [ColumnName(@"BPM")]
        [LoadColumn(1)]
        public float BPM { get; set; }

        [ColumnName(@"TargetBPM")]
        [LoadColumn(2)]
        public float TargetBPM { get; set; }

        [ColumnName(@"BPMDifference")]
        [LoadColumn(3)]
        public float BPMDifference { get; set; }

        [ColumnName(@"Label")]
        [LoadColumn(4)]
        public bool Label { get; set; }
    }

    #endregion
}