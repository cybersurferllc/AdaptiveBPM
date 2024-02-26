using Microsoft.ML.Data;

namespace AdaptiveBpmML
{
    /// <summary>
    /// model output class for AdaptiveBpmMLModel.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        [ColumnName("Score")]
        public float Score { get; set; }
    }

    #endregion
}