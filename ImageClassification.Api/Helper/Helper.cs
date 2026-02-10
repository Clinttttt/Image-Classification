using ImageClassification.Api.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace ImageClassification.Api.Helper
{
    public static class Helper
    {
        public static DescriptionDto HelperResult(string request)
        {
            var dto = new DescriptionDto();
            switch (request)
            {
                case "bird":
                    dto.description = "this is a bird broo";
                    dto.type = "Non Venomous";
                    dto.Name = "bird";
                    break;

                case "snake":
                    dto.description = "this is a snake brooo";
                    dto.type = "Venomous";
                    dto.Name = "snake";
                    break;

                default:
                    dto.description = "this species didn't exists on the choices";
                    dto.Name = "Unknown";
                    dto.type = "N/A";
                    break;
            }
            return dto;
        }
       public async static Task<Result<byte[]>> HelperResult(IFormFile image)
        {
            if (image is null || image.Length == 0)           
                return Result<byte[]>.Failure("No image uploaded");
                
            var allowed_extensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName);

            if (!allowed_extensions.Contains(extension))
            {
                return Result<byte[]>.Failure("invalid type");
            }

            byte[] image_data;
            using (var memory_stream = new MemoryStream())
            {
                await image.CopyToAsync(memory_stream);
                image_data = memory_stream.ToArray();
            }
            return Result<byte[]>.Success(image_data);
        }

 



    }
}
