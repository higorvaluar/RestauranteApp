using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using RestauranteApp.ViewModels;

namespace RestauranteApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool emailJaExiste = await _context.Clientes
                .AnyAsync(c => c.Email == vm.Email);

            if (emailJaExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(vm);
            }

            var cliente = new Cliente
            {
                Nome = vm.Nome,
                Email = vm.Email,
                Senha = vm.Senha,
                Telefone = vm.Telefone
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("ClienteId", cliente.Id);
            HttpContext.Session.SetString("ClienteNome", cliente.Nome);

            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == vm.Email && c.Senha == vm.Senha);

            if (cliente == null)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(vm);
            }

            HttpContext.Session.SetInt32("ClienteId", cliente.Id);
            HttpContext.Session.SetString("ClienteNome", cliente.Nome);

            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}