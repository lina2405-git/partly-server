using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace PickNPlay.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImageController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ImageController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> UploadImageToCloud(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Image not selected");
            }

            if (!IsImage(image))
            {
                return BadRequest("Given file is not image");
            }

            await using var stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Transformation = new Transformation().Height(500).Width(700).Crop("fill"),
                UniqueFilename = false,
            };

            Cloudinary cloudinary = GetInstanceOfCloudinary();
            var uploaded = await cloudinary.UploadAsync(uploadParams);
            return Ok(uploaded.Url);
        }

        [NonAction]
        public async Task<string> UploadImageToCloudNoActionResult(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentNullException();
            }

            if (!IsImage(image))
            {
                throw new InvalidCastException();
            }

            await using var stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
                UniqueFilename = false,
            };

            Cloudinary cloudinary = GetInstanceOfCloudinary();
            var uploaded = await cloudinary.UploadAsync(uploadParams);
            return uploaded.Url.ToString();
        }
        private Cloudinary GetInstanceOfCloudinary()
        {
            var api_key = configuration["Cloudinary:api_key"];
            var api_secret = configuration["Cloudinary:api_secret"];
            var cloudname = configuration["Cloudinary:cloudname"];

            return new($"cloudinary://{api_key}:{api_secret}@{cloudname}");
        }

        private bool IsImage(IFormFile image)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            return allowedTypes.Contains(image.ContentType);
        }
    }
}
