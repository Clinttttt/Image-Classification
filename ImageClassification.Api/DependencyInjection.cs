using ImageClassification.Api.Interface;
using ImageClassification.Api.Models;
using ImageClassification.Api.Services;
using Microsoft.Extensions.ML;

namespace ImageClassification.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection service)
        {

            service.AddControllers();
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
            service.AddScoped<IImageClassificationService, ImageClassificationService>();
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TrainedModel", "MLModel.zip");
            service.AddPredictionEnginePool<ImageData, ImagePrediction>()
                .FromFile(modelPath);

            return service;
        }
    }
}
