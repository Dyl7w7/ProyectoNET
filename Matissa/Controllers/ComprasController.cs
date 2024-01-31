using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using matissa.Models;
using X.PagedList;
using Matissa.Models;

namespace matissa.Controllers
{
    public class ComprasController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public ComprasController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: Compras
        public IActionResult Index(int? page, int busqueda)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            if (busqueda == 0)
            {
              return _context.Compras != null ?
              View(_context.Compras.ToPagedList(pageNumber, pageSize)) :
              Problem("Entity set 'dbMatissaContext.Productos'  is null.");
            }
            else
            {
                var compra = from compraBusca in _context.Compras select compraBusca;
                compra = compra.Where(c => c.IdCompra == busqueda);
                return View(compra.ToPagedList(pageNumber, pageSize));

            }
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,Descripción,FechaCompra,CostoTotalCompra,Factura,Estado")] Compra compra)
        {
            var idCompra = ObtenerCompraId();
            compra.IdCompra = idCompra;

            DateTime fechaHoy = DateTime.Now;
            compra.FechaCompra = fechaHoy;

            double costoTotal = 0;
            compra.CostoTotalCompra = costoTotal;

            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "DetalleCompras");
            }
            return View(compra);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,Descripción,FechaCompra,CostoTotalCompra,Factura,Estado")] Compra compra)
        {
            if (id != compra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.IdCompra))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        public IActionResult DeleteCompra()
        {
            var idCompra = ObtenerUltimaCompra();
            var dateCompra = _context.Compras.Find(idCompra);
            _context.Compras.Remove(dateCompra);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public int ObtenerUltimaCompra()
        {
            var idCompra = _context.Compras.Max(e => (int?)e.IdCompra) ?? 0;

            return idCompra;
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'dbMatissaContext.Compras'  is null.");
            }
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Anular(int id)
        {
            var compra = _context.Compras.Find(id);
            compra.Estado = 0;
            _context.Compras.Update(compra);
            _context.SaveChanges();
            return RedirectToAction("Index", "Compras");
        }

        private bool CompraExists(int id)
        {
          return (_context.Compras?.Any(e => e.IdCompra == id)).GetValueOrDefault();
        }

        public int ObtenerCompraId()
        {
            var idCompra = _context.Compras.Max(e => (int?)e.IdCompra) ?? 0;

            return idCompra + 1;
        }
    }
}
