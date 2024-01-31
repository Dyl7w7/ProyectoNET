using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Matissa.Models;
using Microsoft.AspNetCore.Authorization;

namespace Matissa.Controllers
{
    public class RolesController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public RolesController(dbMatissaNETContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador")]
        // GET: Roles
        public async Task<IActionResult> Index()
        {
              return _context.Rol != null ? 
                          View(await _context.Rol.ToListAsync()) :
                          Problem("Entity set 'dbMatissaNETContext.Rol'  is null.");
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rol == null)
            {
                return NotFound();
            }

            var rol = await _context.Rol
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }



        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,NombreRol,Estado")] Rol rol)
        {
            // Verificar si ya existe un rol con el mismo nombre
            var rolExistente = _context.Rol.FirstOrDefault(r => r.IdRol == rol.IdRol);

            if (rolExistente != null)
            {
                // Si ya existe, agregar un error al modelo y volver a la vista
                ModelState.AddModelError("IdRol", "Ya existe el Id.");
                return View(rol);
            }

            if (ModelState.IsValid)
            {
                _context.Add(rol);
                await _context.SaveChangesAsync();

                // Almacenar el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "El rol se ha creado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }




        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rol == null)
            {
                return NotFound();
            }

            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,NombreRol,Estado")] Rol rol)
        {
            if (id != rol.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.IdRol))
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
            return View(rol);
        }

        // GET: Roles/Delete/5
       public async Task<IActionResult> Delete(int? id)
{
    if (id == null || _context.Rol == null)
    {
        return NotFound();
    }

    var rol = await _context.Rol
        .FirstOrDefaultAsync(m => m.IdRol == id);
    if (rol == null)
    {
        return NotFound();
    }

    return View(rol);
}

[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    if (_context.Rol == null)
    {
        return Problem("Entity set 'dbMatissaNETContext.Rol'  is null.");
    }

    var rol = await _context.Rol.FindAsync(id);
    if (rol != null)
    {
        _context.Rol.Remove(rol);
        await _context.SaveChangesAsync();

        // Almacenar el mensaje de éxito en TempData
        TempData["SuccessMessage"] = "El rol se elimino con exito.";
    }

    return RedirectToAction(nameof(Index));
}


        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null || _context.Rol == null)
            {
                return NotFound();
            }

            var rol = await _context.Rol
                //.Include(c => Navigation)
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rol == null)
            {
                return NotFound();
            }
            if (rol != null)
            {
                rol.Estado = (byte?)((rol.Estado == 1) ? 0 : 1);
                _context.Update(rol);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
          return (_context.Rol?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
