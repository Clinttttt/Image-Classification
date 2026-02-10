using ImageClassification.Api.Helper;
using ImageClassification.Api.Interface;
using ImageClassification.Api.Models;
using Microsoft.ML;
using System.Runtime.CompilerServices;

namespace ImageClassification.Api.Services
{
    public class ImageClassificationService : IImageClassificationService
    {
        private readonly PredictionEngine<ImageData, ImagePrediction> prediction_engine;
        private readonly MLContext ml_context;
        private string? message { get; set; }
        public ImageClassificationService(string model_path)
        {
            ml_context = new MLContext();

            //trained model ni
            ITransformer trained_model = ml_context.Model.Load(model_path, out var model_schema);

            //create prediction ni
            prediction_engine = ml_context.Model.CreatePredictionEngine<ImageData, ImagePrediction>(trained_model);

        }

        
        public Result<PredictionResult> ClassifyImage(Result<byte[]> request)
        {
            string temp_image_path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

            try
            {
                File.WriteAllBytes(temp_image_path, request.value!);
                var image_input = new ImageData { ImageSource = temp_image_path };
                var prediction = prediction_engine.Predict(image_input);


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
