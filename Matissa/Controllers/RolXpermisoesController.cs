using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Matissa.Model;
using Matissa.Models;
using Microsoft.AspNetCore.Authorization;

namespace Matissa.Controllers
{
    public class RolXpermisoesController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public RolXpermisoesController(dbMatissaNETContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador")]
        // GET: RolXpermisoes
        public async Task<IActionResult> Index()
        {
            var dbMatissaNETContext = _context.RolXpermiso.Include(r => r.IdPermisoNavigation).Include(r => r.IdRolNavigation);
            return View(await dbMatissaNETContext.ToListAsync());
        }

        // GET: RolXpermisoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RolXpermiso == null)
            {
                return NotFound();
            }

            var rolXpermiso = await _context.RolXpermiso
                .Include(r => r.IdPermisoNavigation)
                .Include(r => r.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdRolXpermiso == id);
            if (rolXpermiso == null)
            {
                return NotFound();
            }

            return View(rolXpermiso);
        }

        // GET: RolXpermisoes/Create
        public IActionResult Create()
        {
            ViewData["IdPermiso"] = new SelectList(_context.Permiso, "IdPermiso", "IdPermiso");
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol");
            return View();
        }

        // POST: RolXpermisoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRolXpermiso,IdRol,IdPermiso")] RolXpermiso rolXpermiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rolXpermiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPermiso"] = new SelectList(_context.Permiso, "IdPermiso", "IdPermiso", rolXpermiso.IdPermiso);
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", rolXpermiso.IdRol);
            return View(rolXpermiso);
        }

        // GET: RolXpermisoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RolXpermiso == null)
            {
                return NotFound();
            }

            var rolXpermiso = await _context.RolXpermiso.FindAsync(id);
            if (rolXpermiso == null)
            {
                return NotFound();
            }
            ViewData["IdPermiso"] = new SelectList(_context.Permiso, "IdPermiso", "IdPermiso", rolXpermiso.IdPermiso);
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", rolXpermiso.IdRol);
            return View(rolXpermiso);
        }

        // POST: RolXpermisoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRolXpermiso,IdRol,IdPermiso")] RolXpermiso rolXpermiso)
        {
            if (id != rolXpermiso.IdRolXpermiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rolXpermiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolXpermisoExists(rolXpermiso.IdRolXpermiso))
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
            ViewData["IdPermiso"] = new SelectList(_context.Permiso, "IdPermiso", "IdPermiso", rolXpermiso.IdPermiso);
            ViewData["IdRol"] = new SelectList(_context.Rol, "IdRol", "IdRol", rolXpermiso.IdRol);
            return View(rolXpermiso);
        }

        // GET: RolXpermisoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RolXpermiso == null)
            {
                return NotFound();
            }

            var rolXpermiso = await _context.RolXpermiso
                .Include(r => r.IdPermisoNavigation)
                .Include(r => r.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdRolXpermiso == id);
            if (rolXpermiso == null)
            {
                return NotFound();
            }

            return View(rolXpermiso);
        }

        // POST: RolXpermisoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RolXpermiso == null)
            {
                return Problem("Entity set 'dbMatissaNETContext.RolXpermiso'  is null.");
            }
            var rolXpermiso = await _context.RolXpermiso.FindAsync(id);
            if (rolXpermiso != null)
            {
                _context.RolXpermiso.Remove(rolXpermiso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolXpermisoExists(int id)
        {
          return (_context.RolXpermiso?.Any(e => e.IdRolXpermiso == id)).GetValueOrDefault();
        }
    }
}
