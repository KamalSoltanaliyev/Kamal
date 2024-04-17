using cay_verersen.Areas.Admin.ViewModels.SliderViewModel;
using cay_verersen.Contexts;
using cay_verersen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cay_verersen.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly CayverersenDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(CayverersenDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders
                .AsNoTracking()
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateViewModel slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //if (product.Image.CheckFileSize(3000))
            //{
            //    ModelState.AddModelError("Image", "Get ariqla");
            //    return View();
            //}

            //if (!product.Image.CheckFileType("image/"))
            //{
            //    ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
            //    return View();
            //}

            //string path = @$"{_webHostEnvironment.WebRootPath}\assets\images\website-images\{product.Image.FileName}";
            string fileName = $"{Guid.NewGuid()}-{slider.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {   
                await slider.Image.CopyToAsync(stream);
            }
            //using FileStream stream = new FileStream(path, FileMode.Create);
            //await product.Image.CopyToAsync(stream);

            Slider newSlider = new()
            {
                Title = slider.Title,
                Description = slider.Description,
                Image = fileName
            };


            await _context.Sliders.AddAsync(newSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var slider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null)
                return NotFound();

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

            //var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            //if (slider == null)
            //    return NotFound();

            //string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", slider.Image);

            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}

            //slider.IsDeleted = true;

            //await _context.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider slider)
        {
            var dbSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
                return NotFound();

            dbSlider.Title = slider.Title;
            dbSlider.Description = slider.Description;
            dbSlider.Image = slider.Image;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
