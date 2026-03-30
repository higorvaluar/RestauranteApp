using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using RestauranteApp.ViewModels;
using System.Diagnostics;

namespace RestauranteApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.Today;

            var vm = new HomeIndexViewModel
            {
                ProdutosAlmoco = await _context.Produtos
                    .AsNoTracking()
                    .Where(p => p.Periodo == PeriodoEnum.Almoco)
                    .OrderBy(p => p.Nome)
                    .ToListAsync(),

                ProdutosJantar = await _context.Produtos
                    .AsNoTracking()
                    .Where(p => p.Periodo == PeriodoEnum.Jantar)
                    .OrderBy(p => p.Nome)
                    .ToListAsync(),

                SugestaoAlmoco = await _context.SugestoesChefe
                    .AsNoTracking()
                    .Include(s => s.Produto)
                    .FirstOrDefaultAsync(s =>
                        s.Data.Date == hoje &&
                        s.Periodo == PeriodoEnum.Almoco),

                SugestaoJantar = await _context.SugestoesChefe
                    .AsNoTracking()
                    .Include(s => s.Produto)
                    .FirstOrDefaultAsync(s =>
                        s.Data.Date == hoje &&
                        s.Periodo == PeriodoEnum.Jantar)
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}