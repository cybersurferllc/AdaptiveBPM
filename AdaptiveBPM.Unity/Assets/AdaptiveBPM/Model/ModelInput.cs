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
        public float Intensity { get; set; }

        [ColumnName(@"BPM")]
        public float BPM { get; set; }

        [ColumnName(@"TargetBPM")]
        public float TargetBPM { get; set; }

        [ColumnName(@"BPMDifference")]
        public float BPMDifference { get; set; }

        [ColumnName(@"Label")]
        public bool Label { get; set; }
    }

    #endregion
}