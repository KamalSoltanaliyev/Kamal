using cay_verersen.Contexts;
using cay_verersen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cay_verersen.Controllers
{
    public class HomeController : Controller
    {
        private readonly CayverersenDbContext _context;

        public HomeController(CayverersenDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Sliders = await _context.Sliders.ToListAsync();
            var products = await _context.Products.ToListAsync();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                Sliders = Sliders,
                Products = products
            };

            return View(homeViewModel);
        }
    }
}
