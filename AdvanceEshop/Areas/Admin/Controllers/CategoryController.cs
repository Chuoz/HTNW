using AdvanceEshop.Data;
using AdvanceEshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdvanceEshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderByDescending(p => p.CategoryId).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId == category.CategoryId);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

                if (category.CategoryUpLoadImage != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                    string imageName = Guid.NewGuid().ToString() + "_" + category.CategoryUpLoadImage.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await category.CategoryUpLoadImage.CopyToAsync(fs);
                    fs.Close();
                    category.CategoryPhoto = imageName;
                }

                _context.Add(category);
                TempData["success"] = "Thêm danh mục thành công";
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Category category = await _context.Categories.FindAsync(Id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryName == category.CategoryName);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

                if (category.CategoryUpLoadImage != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                    string imageName = Guid.NewGuid().ToString() + "_" + category.CategoryUpLoadImage.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await category.CategoryUpLoadImage.CopyToAsync(fs);
                    fs.Close();
                    category.CategoryPhoto = imageName;
                }

                _context.Update(category);
                TempData["success"] = "Cập nhật danh mục thành công";
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(category);
        }


        public async Task<IActionResult> Delete(int Id)
        {
            Category category = await _context.Categories.FindAsync(Id);
            if (!string.Equals(category.CategoryPhoto, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                string oldFileImage = Path.Combine(uploadsDir, category.CategoryPhoto);
                if (System.IO.File.Exists(oldFileImage))
                {
                    System.IO.File.Delete(oldFileImage);
                }
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["error"] = "Danh mục đã xóa";
            return RedirectToAction("Index");
        }

    }
}
