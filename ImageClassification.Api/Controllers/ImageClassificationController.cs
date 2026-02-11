using ImageClassification.Api.Interface;
using ImageClassification.Api.Models;
using ImageClassification.Api.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;

namespace ImageClassification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageClassificationController : ApiBaseController
    {
        private readonly IImageClassificationService _classification_service;
        public ImageClassificationController(IImageClassificationService classification_service)
        {
            _classification_service = classification_service;
        }

        [HttpPost("predict")]
        public async Task<ActionResult<PredictionResult>> PredictImage(IFormFile image)
        {
            var helper_byte = await ImageClassification.Api.Helper.Helper.HelperResult(image);
            if (!helper_byte.is_success)
                return BadRequest(helper_byte.error);
            var result =  _classification_service.ClassifyImage(helper_byte);
            return HandleResponse(result);        
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Service is running", message = "Image classification API is ready" });
        }
    }
}
