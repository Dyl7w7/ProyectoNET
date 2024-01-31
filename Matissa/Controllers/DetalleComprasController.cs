using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Matissa.Models;
using matissa.Models;

namespace matissa.Controllers
{
    public class DetalleComprasController : Controller
    {
        private readonly dbMatissaNETContext _context;
        private static List<ServicioModel> servicioModels = new List<ServicioModel>();

        public DetalleComprasController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: DetalleCompras
        public async Task<IActionResult> Index(int id, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var queryDetalleCompra = _context.DetalleCompras.Include(d => d.IdCompraNavigation).Include(d => d.IdProductoNavigation).Include(d => d.IdProveedorNavigation);

            var detalleCompra = await queryDetalleCompra
                .Where(c => c.IdCompra == id)
                .ToListAsync();

            return View(detalleCompra.ToPagedList(pageNumber, pageSize));
        }

        // GET: DetalleCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetalleCompras == null)
            {
                return NotFound();
            }

            var detalleCompra = await _context.DetalleCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleCompra == id);
            if (detalleCompra == null)
            {
                return NotFound();
            }

            return View(detalleCompra);
        }

        [HttpGet]
        public IActionResult AgregarProducto(ServicioModel model)
        {
            servicioModels.Add(model);

            // Puedes realizar otras operaciones con el detalle si es necesario
            Console.WriteLine($"Detalle recibido:{model.idProducto} {model.idProveedor} {model.nombreProducto} {model.nombreProveedor} {model.cantidad} {model.precio} {model.costoTotal}");
            foreach (var detall in servicioModels)
            {
                Console.WriteLine($"Detalle almacenado:{model.idProducto} {model.idProveedor} {model.nombreProducto} {model.nombreProveedor} {model.cantidad} {model.precio} {model.costoTotal}");
            }
            return Json(new { success = true, message = "Detalle guardado" });
        }

        [HttpGet]
        public IActionResult RemoveProducto(int idProducto)
        {
            ServicioModel productoRemove = servicioModels.FirstOrDefault(d => d.idProducto == idProducto);
            if (productoRemove != null)
            {
                servicioModels.Remove(productoRemove);
                Console.WriteLine($"Detalle con ID {productoRemove.idProducto} eliminado.");
            }
            else
            {
                Console.WriteLine($"Detalle no encontrado.");
            }
            Console.WriteLine("Lista después de la eliminación:");
            foreach (var detalle in servicioModels)
            {
                Console.WriteLine($"ID: {detalle.idProducto}");
            }
            return Json(new { success = true, message = "Detalle guardado" });
        }

        [HttpGet]
        public IActionResult DataProducto(int idProducto)
        {
            var producto = _context.Productos.Find(idProducto);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpGet]
        public IActionResult DataProveedor(int idProveedor)
        {
            var proveedor = _context.Proveedors.Find(idProveedor);
            if (proveedor == null)
            {
                return NotFound();
            }
            return Ok(proveedor);
        }

        // GET: DetalleCompras/Create
        public IActionResult Create()
        {
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor");
            return View();
        }

        // POST: DetalleCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleCompra,IdCompra,IdProducto,IdProveedor,PrecioUnitario,Cantidad,CostoTotalUnitario")] DetalleCompra detalleCompra)
        {
            if (detalleCompra != null)
            {
                var idCompra = ObtenerCompraID();
                double totalComprado = 0;

                foreach(var item in servicioModels)
                {
                    var producto = _context.Productos.Find(item.idProducto);
                    Console.WriteLine(producto.SaldoInventario);
                    Console.WriteLine("#####################################################################");

                    var idDetalleCompra = ObtenerDetalleCompraID();
                    detalleCompra.IdDetalleCompra = idDetalleCompra;
                    detalleCompra.IdCompra = idCompra;

                    detalleCompra.IdProducto = item.idProducto;
                    detalleCompra.IdProveedor = item.idProveedor;
                    detalleCompra.PrecioUnitario = item.precio;
                    detalleCompra.Cantidad = item.cantidad;
                    detalleCompra.CostoTotalUnitario = item.costoTotal;

                    _context.DetalleCompras.Add(detalleCompra);
                    await _context.SaveChangesAsync();

                    totalComprado += item.costoTotal;

                    producto.SaldoInventario += item.cantidad;
                    _context.Productos.Update(producto);
                }

                var compra = _context.Compras.Find(idCompra);

                if(compra != null)
                {
                    compra.CostoTotalCompra = totalComprado;
                    _context.Compras.Update(compra);
                    await _context.SaveChangesAsync();
                }

                servicioModels.Clear();
                return RedirectToAction("Index", "Compras");
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", detalleCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detalleCompra.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", detalleCompra.IdProveedor);
            return NotFound(detalleCompra);
        }

        public int ObtenerCompraID()
        {
            var idCompra = _context.Compras.Max(c => (int?)c.IdCompra) ?? 1;
            return idCompra;
        }

        public int ObtenerDetalleCompraID()
        {
            var idDetalleCompra = _context.DetalleCompras.Max(c => (int?)c.IdDetalleCompra) ?? 0;
            return idDetalleCompra + 1;
        }

        // GET: DetalleCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetalleCompras == null)
            {
                return NotFound();
            }

            var detalleCompra = await _context.DetalleCompras.FindAsync(id);
            if (detalleCompra == null)
            {
                return NotFound();
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", detalleCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detalleCompra.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", detalleCompra.IdProveedor);
            return View(detalleCompra);
        }

        // POST: DetalleCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleCompra,IdCompra,IdProducto,IdProveedor,PrecioUnitario,Cantidad,CostoTotalUnitario")] DetalleCompra detalleCompra)
        {
            if (id != detalleCompra.IdDetalleCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleCompraExists(detalleCompra.IdDetalleCompra))
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
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", detalleCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detalleCompra.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", detalleCompra.IdProveedor);
            return View(detalleCompra);
        }

        // GET: DetalleCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetalleCompras == null)
            {
                return NotFound();
            }

            var detalleCompra = await _context.DetalleCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleCompra == id);
            if (detalleCompra == null)
            {
                return NotFound();
            }

            return View(detalleCompra);
        }

        // POST: DetalleCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetalleCompras == null)
            {
                return Problem("Entity set 'dbMatissaContext.DetalleCompras'  is null.");
            }
            var detalleCompra = await _context.DetalleCompras.FindAsync(id);
            if (detalleCompra != null)
            {
                _context.DetalleCompras.Remove(detalleCompra);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleCompraExists(int id)
        {
          return (_context.DetalleCompras?.Any(e => e.IdDetalleCompra == id)).GetValueOrDefault();
        }
    }
}
