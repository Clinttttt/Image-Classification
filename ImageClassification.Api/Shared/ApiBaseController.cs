using ImageClassification.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageClassification.Api.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected ActionResult<T> HandleResponse<T>(Result<T> value)
        {
            if(value.status_code == 200)
            {
                return Ok(value);
            }
            return value.status_code switch
            {
                404 => NotFound(),
                401 => Unauthorized(),
                403 => Forbid(),
                409 => Conflict(),
                204 => NoContent(),
                500 => StatusCode(500, "Internal Server Error"),
                400 => BadRequest(),
                _ => BadRequest()
            };
        }
    }
}
