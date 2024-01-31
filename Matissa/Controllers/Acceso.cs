using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Matissa.Models;

namespace TuProyecto.Controllers
{
    public class AccesoController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public AccesoController(dbMatissaNETContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult login()
        {
            return View();
        }
        // Este método se invoca cuando se envía el formulario de inicio de sesión.
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Aquí debes validar las credenciales del usuario con tu base de datos.
            // Si las credenciales son correctas, puedes iniciar la sesión del usuario.
            var user = _context.Usuario.SingleOrDefault(u => u.Correo == username);
            if (user == null)
            {
                //ModelState.AddModelError("NombreUsuario", "Usuario o contraseña incorrectos");
                return View();
            }
            var rol = _context.Rol.SingleOrDefault(u => u.IdRol == user.IdRol);

            if (user != null && user?.Estado == 1 && rol?.Estado == 1)
            {
                if (user.Correo == username && user.Contraseña == password)
                {
                    // Obtiene el usuario con sus roles
                    var usuarioConRoles = _context.Usuario
                        .Include(u => u.IdRolNavigation) // Asegúrate de tener una propiedad de navegación llamada IdRolNavigation en tu modelo Usuario
                        .FirstOrDefault(u => u.Correo == username);

                    if (usuarioConRoles != null)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),

                        // Agrega más claims según sea necesario.
                    };

                        // Agrega los roles del usuario a los claims
                        claims.AddRange(new[] { usuarioConRoles.IdRolNavigation?.NombreRol }
                            .Where(roleName => !string.IsNullOrEmpty(roleName))
                            .Select(roleName => new Claim(ClaimTypes.Role, roleName)));// Ajusta el nombre de la propiedad de navegación de roles

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        //// Almacena los roles en la sesión
                        //HttpContext.Session.SetString("Roles", JsonConvert.SerializeObject(usuarioConRoles.IdRolNavigation));

                        ViewBag.User = HttpContext.Items["User"];

                        return RedirectToAction("Index", "Home");
                    }
                } 
                //ModelState.AddModelError("NombreUsuario", "Usuario o contraseña incorrectos");
                TempData["Access"] = "Usuario o password incorrectos";

            }
            Console.WriteLine("No puede ingresar");
            //ModelState.AddModelError("NombreUsuario", "Usted no puede ingresar porque esta inhabilitado o su rol esta inactivo");
            TempData["Accesso"] = "\"Usuario o contraseña incorrectos\" o Esta inhabilitado";

            return View();
        }



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpia cualquier información adicional de la sesión o contexto
            HttpContext.Items.Remove("User");

            // Establece el usuario actual como no autenticado
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Elimina cualquier cookie adicional
            foreach (var cookieKey in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookieKey);
            }

            // Opcional: Redirige a la página de inicio u otra página después del cierre de sesión
            return RedirectToAction("Login", "Acceso");
        }


    }
}
