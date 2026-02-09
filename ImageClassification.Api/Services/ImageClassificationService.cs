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

        public ImageClassificationService(string model_path)
        {
            ml_context = new MLContext();

            //trained model ni
            ITransformer trained_model = ml_context.Model.Load(model_path, out var model_schema);

            //create prediction ni
            prediction_engine = ml_context.Model.CreatePredictionEngine<ImageData, ImagePrediction>(trained_model);

        }
        public PredictionResult ClassifyImage(byte[] image_data)
        {
            string temp_image_path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

            try
            {
                File.WriteAllBytes(temp_image_path, image_data);

                var image_input = new ImageData { ImageSource = temp_image_path };

                var prediction = prediction_engine.Predict(image_input);


                if (prediction.Score == null || prediction.Score.Length == 0)
                    throw new Exception("Prediction score array is empty.");

                // get confidence
                float max_score = prediction.Score.Max();

                const float threshold = 0.8f;
                string predictedClass;

                if (max_score < threshold)
                {
                    predictedClass = "Unknown";
                }
                else
                {
                    predictedClass = prediction.PredictedLabel!;
                }

                return new PredictionResult
                {
                    Message = predictedClass == "Unknown" ? "This animal does not exist in training data" : $"This is a {predictedClass}",
                    Confidence = max_score,
                    PredictedClass = predictedClass
                };
            }
            finally
            {
                if (File.Exists(temp_image_path))
                    File.Delete(temp_image_path);
            }
        }
    }


}
