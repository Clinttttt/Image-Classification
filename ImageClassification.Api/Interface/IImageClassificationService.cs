using ImageClassification.Api.Helper;
using ImageClassification.Api.Models;

namespace ImageClassification.Api.Interface
{
    public interface IImageClassificationService
    {
        Result<PredictionResult> ClassifyImage(Result<byte[]> request);
    }
}
