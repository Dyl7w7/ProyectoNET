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
    public class VentaServiciosController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public VentaServiciosController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: VentaServicios
        public async Task<IActionResult> Index(string searchString)
        {
            var ventas = from v in _context.VentaServicios select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                ventas = ventas.Where(v =>
                    v.IdCita.ToString().Contains(searchString) ||
                    v.ValorVenta.ToString().Contains(searchString) ||
                    v.FechaVenta.ToString().Contains(searchString) ||
                    v.FormaPago.Contains(searchString) 
                );
            }

            return View(await ventas.ToListAsync());
        }




        // GET: VentaServicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VentaServicios == null)
            {
                return NotFound();
            }

            var ventaServicio = await _context.VentaServicios
                .FirstOrDefaultAsync(m => m.IdVentaServicio == id);
            if (ventaServicio == null)
            {
                return NotFound();
            }

            return View(ventaServicio);
        }

        // GET: VentaServicios/Create
        public IActionResult Create()
        {
            var citas = _context.Cita.ToList();

            if (citas != null && citas.Any())
            {
                ViewBag.Citas = new SelectList(citas, "IdCita", "IdCita"); // Utiliza solo la propiedad IdCita para el SelectList
            }
            else
            {
                ViewBag.Citas = new SelectList(new List<Citum>(), "IdCita", "IdCita");
            }

            return View();
        }




        // POST: VentaServicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVentaServicio,IdCita,ValorVenta,FechaVenta,FormaPago,Estado")] VentaServicio ventaServicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Agregar mensajes de registro para verificar los datos recibidos
                    Console.WriteLine($"IdCita: {ventaServicio.IdCita}");
                    Console.WriteLine($"ValorVenta: {ventaServicio.ValorVenta}");
                    Console.WriteLine($"FechaVenta: {ventaServicio.FechaVenta}");
                    Console.WriteLine($"FormaPago: {ventaServicio.FormaPago}");
                    Console.WriteLine($"Estado: {ventaServicio.Estado}");

                    _context.Add(ventaServicio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Puedes agregar un registro de la excepción para depurar
                    Console.WriteLine(ex.Message);
                    return RedirectToAction(nameof(Create));
                }
            }
            // Si llegas aquí, significa que ModelState no es válido
            // Puedes añadir un mensaje de error o log para depuración
            return View(ventaServicio);
        }


        // GET: VentaServicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VentaServicios == null)
            {
                return NotFound();
            }

            var ventaServicio = await _context.VentaServicios.FindAsync(id);
            if (ventaServicio == null)
            {
                return NotFound();
            }
            return View(ventaServicio);
        }

        // POST: VentaServicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVentaServicio,IdCita,ValorVenta,FechaVenta,FormaPago,Estado")] VentaServicio ventaServicio)
        {
            if (id != ventaServicio.IdVentaServicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ventaServicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaServicioExists(ventaServicio.IdVentaServicio))
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
            return View(ventaServicio);
        }

        // GET: VentaServicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VentaServicios == null)
            {
                return NotFound();
            }

            var ventaServicio = await _context.VentaServicios
                .FirstOrDefaultAsync(m => m.IdVentaServicio == id);
            if (ventaServicio == null)
            {
                return NotFound();
            }

            return View(ventaServicio);
        }

        // POST: VentaServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VentaServicios == null)
            {
                return Problem("Entity set 'dbMatissaNETContext.VentaServicios'  is null.");
            }
            var ventaServicio = await _context.VentaServicios.FindAsync(id);
            if (ventaServicio != null)
            {
                _context.VentaServicios.Remove(ventaServicio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaServicioExists(int id)
        {
          return (_context.VentaServicios?.Any(e => e.IdVentaServicio == id)).GetValueOrDefault();
        }
    }
}
