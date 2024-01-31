using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Matissa.Models;
using Matissa.Model;

namespace Configuracion2._0.Controllers
{
    [Authorize(Roles = "Administrador,Usuario")]

    public class UsuariosController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public UsuariosController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(int buscar)
        {
            var usuario = from Usuario in _context.Usuario select Usuario;

            if (!(buscar == 0))
                usuario = usuario.Where(s => s.IdUsuario.Equals(buscar));

            return View(await usuario.ToListAsync());
            //var configuracionContext = _context.Usuario.Include(u => u.IdRolNavigation);
            //return View(await configuracionContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,NombreUsuario,Correo,Contraseña,Estado,IdRol")] Usuario usuario)
        {
            var cedulaExist = _context.Usuario.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
            if (cedulaExist != null)
            {
                ModelState.AddModelError("Estado", "El IdRol ya existe");
                ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
                return View();
            }

            var correoExist = _context.Usuario.FirstOrDefault(u => u.Correo == usuario.Correo);
            if (correoExist != null)
            {
                ModelState.AddModelError("Estado", "El correo ya existe");
                ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
                return View();
            }

            var rol = await _context.Rol.FindAsync(usuario.IdRol);
            if (rol.Estado == 0)
            {
                ModelState.AddModelError("Estado", "No se pueden otorgar permisos a roles con estado 0.");
                ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);

                // Puedes agregar algún mensaje adicional si es necesario
                return View(usuario);
            }

            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                // Almacenar el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "El usuario se ha creado con exito.";

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario,Correo,Contraseña,Estado,IdRol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> CambiarContraseña(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContraseña(int id, [Bind("IdUsuario,NombreUsuario,Correo,Contraseña,Estado,IdRol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }
        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'ConfiguracionContext.Usuarios'  is null.");
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();

                // Almacenar el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "El usuario se elimino con exito.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuario?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var cliente = await _context.Usuario
                //.Include(c => c.TipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (cliente == null)
            {
                return NotFound();
            }
            if (cliente != null)
            {
                cliente.Estado = (byte)((cliente.Estado == 1) ? 0 : 1);
                _context.Update(cliente);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
