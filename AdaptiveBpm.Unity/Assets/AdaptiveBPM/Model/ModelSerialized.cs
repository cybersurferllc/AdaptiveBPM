using Microsoft.ML.Data;

namespace AdaptiveBpmML
{
    /// <summary>
    /// model serialized class for AdaptiveBpmMLModel.
    /// </summary>
    #region model serialized class
    public class ModelSerialized
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
        public int Label { get; set; }
    }
    #endregion
}