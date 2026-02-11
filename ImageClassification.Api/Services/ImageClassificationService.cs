using ImageClassification.Api.Helper;
using ImageClassification.Api.Interface;
using ImageClassification.Api.Models;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using System.Runtime.CompilerServices;

namespace ImageClassification.Api.Services
{
    public class ImageClassificationService : IImageClassificationService
    {
        private readonly PredictionEnginePool<ImageData, ImagePrediction> prediction_engine_pool;

        private string? message { get; set; }
        public ImageClassificationService(PredictionEnginePool<ImageData, ImagePrediction> prediction_engine_pool)
        {
            this.prediction_engine_pool = prediction_engine_pool;     
        }

        
        public Result<PredictionResult> ClassifyImage(Result<byte[]> request)
        {
            string temp_image_path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

            try
            {
                File.WriteAllBytes(temp_image_path, request.value!);
                var image_input = new ImageData { ImageSource = temp_image_path };
                var prediction = prediction_engine_pool.Predict(image_input);


                if (prediction.Score == null || prediction.Score.Length == 0)
                   return Result<PredictionResult>.Failure("Prediction score array is empty.");

                // get confidence
                float max_score = prediction.Score.Max();

                const float threshold = 0.8f;
  
                prediction.PredictedLabel = max_score < threshold ? "Unkown"  : prediction.PredictedLabel!;

                var dto = ImageClassification.Api.Helper.Helper.HelperResult(prediction.PredictedLabel);

                var result = new PredictionResult
                {
                    Message = dto.description,
                    type = dto.type,
                    Confidence = max_score,
                    PredictedClass = dto.Name,
                };
                return Result<PredictionResult>.Success(result);
            }
            finally
            {
                if (File.Exists(temp_image_path))
                    File.Delete(temp_image_path);
            }
        }
    }





}
