using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Matissa.Models;

namespace Matissa.Controllers
{
    public class DetalleCitumsController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public DetalleCitumsController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: DetalleCitums
        public async Task<IActionResult> Index()
        {
            var dbMatissaNETContext = _context.DetalleCita.Include(d => d.IdCitaNavigation).Include(d => d.IdEmpleadoNavigation).Include(d => d.IdServicioNavigation);
            return View(await dbMatissaNETContext.ToListAsync());
        }

        // GET: DetalleCitums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetalleCita == null)
            {
                return NotFound();
            }

            var detalleCitum = await _context.DetalleCita
                .Include(d => d.IdCitaNavigation)
                .Include(d => d.IdEmpleadoNavigation)
                .Include(d => d.IdServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleCita == id);
            if (detalleCitum == null)
            {
                return NotFound();
            }

            return View(detalleCitum);
        }

        // GET: DetalleCitums/Create
        public IActionResult Create()
        {
            ViewData["IdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita");
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado");
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio");
            return View();
        }

        // POST: DetalleCitums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleCita,IdCita,IdEmpleado,IdServicio,FechaCita,HoraInicio,HoraFin,DuraciónServicio,Descuento,CostoServicio,Estado")] DetalleCitum detalleCitum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleCitum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", detalleCitum.IdCita);
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", detalleCitum.IdEmpleado);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", detalleCitum.IdServicio);
            return View(detalleCitum);
        }

        // GET: DetalleCitums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetalleCita == null)
            {
                return NotFound();
            }

            var detalleCitum = await _context.DetalleCita.FindAsync(id);
            if (detalleCitum == null)
            {
                return NotFound();
            }
            ViewData["IdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", detalleCitum.IdCita);
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", detalleCitum.IdEmpleado);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", detalleCitum.IdServicio);
            return View(detalleCitum);
        }

        // POST: DetalleCitums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleCita,IdCita,IdEmpleado,IdServicio,FechaCita,HoraInicio,HoraFin,DuraciónServicio,Descuento,CostoServicio,Estado")] DetalleCitum detalleCitum)
        {
            if (id != detalleCitum.IdDetalleCita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleCitum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleCitumExists(detalleCitum.IdDetalleCita))
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
            ViewData["IdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", detalleCitum.IdCita);
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", detalleCitum.IdEmpleado);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", detalleCitum.IdServicio);
            return View(detalleCitum);
        }

        // GET: DetalleCitums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetalleCita == null)
            {
                return NotFound();
            }

            var detalleCitum = await _context.DetalleCita
                .Include(d => d.IdCitaNavigation)
                .Include(d => d.IdEmpleadoNavigation)
                .Include(d => d.IdServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleCita == id);
            if (detalleCitum == null)
            {
                return NotFound();
            }

            return View(detalleCitum);
        }

        // POST: DetalleCitums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetalleCita == null)
            {
                return Problem("Entity set 'dbMatissaNETContext.DetalleCita'  is null.");
            }
            var detalleCitum = await _context.DetalleCita.FindAsync(id);
            if (detalleCitum != null)
            {
                _context.DetalleCita.Remove(detalleCitum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleCitumExists(int id)
        {
          return (_context.DetalleCita?.Any(e => e.IdDetalleCita == id)).GetValueOrDefault();
        }
    }
}
