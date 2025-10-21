using Microsoft.AspNetCore.Mvc;
using CMCS.Services;

namespace CMCS.Controllers
{
    public class FilesController : Controller
    {
        private readonly IClaimService _service;
        private readonly IWebHostEnvironment _env;

        public FilesController(IClaimService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

       
        public async Task<IActionResult> Get(int id)
        {
            var doc = await _service.GetDocumentAsync(id);
            if (doc == null)
                return NotFound();

           
            var fullPath = Path.Combine(_env.WebRootPath, doc.FilePath.TrimStart('/'));

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var stream = System.IO.File.OpenRead(fullPath);
            return File(stream, "application/octet-stream", doc.FileName);
        }
    }
}
