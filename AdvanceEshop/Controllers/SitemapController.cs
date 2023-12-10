using Microsoft.AspNetCore.Mvc;

namespace AdvanceEshop.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SitemapController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("sitemap.xml")]
        public IActionResult SitemapXml()
        {
            var wwwrootPath = _hostingEnvironment.WebRootPath;
            var sitemapFilePath = Path.Combine(wwwrootPath, "sitemap.xml");

            if (System.IO.File.Exists(sitemapFilePath))
            {
                return PhysicalFile(sitemapFilePath, "application/xml");
            }
            else
            {
                // Trả về lỗi 404 nếu không tìm thấy file sitemap.xml
                return NotFound();
            }
        }
    }
}
