using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [Authorize]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class FilesController : Controller
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new System.ArgumentNullException(
                nameof(fileExtensionContentTypeProvider));
        }
        /// <summary>
        /// Getting file path
        /// </summary>
        /// <param name="fileId">id of the file to get</param>
        /// <returns>IAction Result</returns>
        /// /// <response code="200">File was found successfully</response>
        /// <response code="404">File could not be found</response>

        [HttpGet("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetFile(string fileId)
        {
            //look up the actual file, depending on the fieldId...
            //demo code
            var pathToFile = "creating-the-api-and-returning-resources-slides.pdf";

            //check whether the file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
