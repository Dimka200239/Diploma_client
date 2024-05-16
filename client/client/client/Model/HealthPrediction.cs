using Microsoft.ML.Data;

namespace client.Model
{
    public class HealthPrediction
    {
        [ColumnName("PredictedLabel")]
        public float Prediction { get; set; }

        public float[] Score { get; set; }
    }
}
