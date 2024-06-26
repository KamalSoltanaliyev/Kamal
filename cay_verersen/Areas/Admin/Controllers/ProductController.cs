﻿using cay_verersen.Areas.Admin.ViewModels.ProductViewModel;
using cay_verersen.Contexts;
using cay_verersen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cay_verersen.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly CayverersenDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(CayverersenDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            //if (!product.Image.CheckFileSize(3000))
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
            string fileName = $"{Guid.NewGuid()}-{product.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await product.Image.CopyToAsync(stream);
            }
            //using FileStream stream = new FileStream(path, FileMode.Create);
            //await product.Image.CopyToAsync(stream);

            Product newProduct = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = fileName,
                CategoryId = product.CategoryId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null)
                return NotFound();

            ProductUpdateViewModel productUpdateViewModel = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };

            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(productUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateViewModel productUpdateViewModel)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
                return View();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null)
                return NotFound();

            if (productUpdateViewModel.Image != null)
            {
                //if (productUpdateViewModel.Image.CheckFileSize(3000))
                //{
                //    ModelState.AddModelError("Image", "Get ariqla");
                //    return View();
                //}

                //if (!productUpdateViewModel.Image.CheckFileType("image/"))
                //{
                //    ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
                //    return View();
                //}

                string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs");
                string path = Path.Combine(basePath, product.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                string fileName = $"{Guid.NewGuid()}-{productUpdateViewModel.Image.FileName}";
                path = Path.Combine(basePath, fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await productUpdateViewModel.Image.CopyToAsync(stream);
                }
                product.Image = fileName;
            }

            product.Name = productUpdateViewModel.Name;
            product.Description = productUpdateViewModel.Description;
            product.Price = productUpdateViewModel.Price;
            product.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (product == null)
                return NotFound();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", product.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
