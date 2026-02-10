namespace ImageClassification.Api.Models
{
    public class PredictionResult
    {
        public string? Message { get; set; }
        public string? PredictedClass { get; set; }
        public string? type { get; set; }
        public float Confidence { get; set; }
    }
}
