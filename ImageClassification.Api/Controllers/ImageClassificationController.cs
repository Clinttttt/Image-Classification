using ImageClassification.Api.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;

namespace ImageClassification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageClassificationController : ControllerBase
    {
        private readonly IImageClassificationService _classification_service;
        public ImageClassificationController(IImageClassificationService classification_service)
        {
            _classification_service = classification_service;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictImage(IFormFile image)
        {
            if(image is null || image.Length == 0)
            {
                return BadRequest(new { error = "No image uploaded" });
            }

            var allowed_extensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName);

            if (!allowed_extensions.Contains(extension))
            {
                return BadRequest(new { error = "invalid type" });
            }
            try
            {
                byte[] image_data;
                using (var memory_stream = new MemoryStream())
                {
                    await image.CopyToAsync(memory_stream);
                    image_data = memory_stream.ToArray();
                }
                var result = _classification_service.ClassifyImage(image_data);

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    predictedClass = result.PredictedClass,
                    confidence = $"{result.Confidence * 100:F2}%"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Prediction failed: {ex.Message}" });
            }

        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Service is running", message = "Image classification API is ready" });
        }
    }
}
