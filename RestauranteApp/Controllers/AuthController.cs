using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<Cliente> _passwordHasher = new();

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

            vm.Email = vm.Email.Trim();

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
                Telefone = vm.Telefone
            };

            cliente.Senha = _passwordHasher.HashPassword(cliente, vm.Senha);

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
                .FirstOrDefaultAsync(c => c.Email == vm.Email);

            if (cliente == null)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(vm);
            }

            PasswordVerificationResult resultado;

            try
            {
                resultado = _passwordHasher.VerifyHashedPassword(cliente, cliente.Senha, vm.Senha);

                if (resultado == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "E-mail ou senha inválidos.");
                    return View(vm);
                }

                if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    cliente.Senha = _passwordHasher.HashPassword(cliente, vm.Senha);
                    await _context.SaveChangesAsync();
                }
            }
            catch (FormatException)
            {
                // Compatibilidade com usuários antigos em texto puro
                if (cliente.Senha != vm.Senha)
                {
                    ModelState.AddModelError("", "E-mail ou senha inválidos.");
                    return View(vm);
                }

                cliente.Senha = _passwordHasher.HashPassword(cliente, vm.Senha);
                await _context.SaveChangesAsync();
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