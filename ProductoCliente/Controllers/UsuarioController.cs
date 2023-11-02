using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductoCliente.Model;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;

namespace ProductoCliente.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly HttpClient _httpClient;
        public List<Usuario> listaUsuarios = new List<Usuario>();

        public UsuarioController(ILogger<UsuarioController> logger, UsuarioContext context)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

       
        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Entro en Login()");
            
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Login(Usuario model)
        {
            var dbContext = HttpContext.RequestServices.GetService<UsuarioContext>();
            var usuario = dbContext.Usuarios.FirstOrDefault(u => u.Mail == model.Mail);

            
            if (usuario != null && usuario.Password == model.Password)
            {
                
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Mail),
            
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            
            Response.Cookies.Delete("MiCookie");
         
            Response.Headers["Cache-Control"] = "no-cache, no-store";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";
          
            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Menu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var dbContext = HttpContext.RequestServices.GetService<UsuarioContext>();

                // Verificar si ya existe un usuario con el mismo correo electrónico
                var existingUser = dbContext.Usuarios.FirstOrDefault(u => u.Mail == model.Mail);
                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
                    return View(model);
                }

                dbContext.Usuarios.Add(model);
                dbContext.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}