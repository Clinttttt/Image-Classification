using ImageClassification.Api.Models;

namespace ImageClassification.Api.Interface
{
    public interface IImageClassificationService
    {
        PredictionResult ClassifyImage(byte[] imageData);
    }
}
