
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Matissa.Models;

using X.PagedList;
using Microsoft.CodeAnalysis.Operations;

namespace Matissa.Controllers
{
    public class DetallePedidosController : Controller
    {
        private readonly dbMatissaNETContext _context;
        private static List<PedidoModel> pedidoModels = new List<PedidoModel>();

        public DetallePedidosController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: DetallePedidos
        public async Task<IActionResult> Index()
        {
            var dbMatissaNETContext = _context.DetallePedidos.Include(p => p.IdPedidoNavigation).Include(c => c.IdProductoNavigation);
            return View(await dbMatissaNETContext.ToListAsync());
        }

        // GET: DetallePedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos
                .Include(d => d.IdPedidoNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetallePedido == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        [HttpGet]
        public IActionResult AgregarProducto(PedidoModel model)
        {
            pedidoModels.Add(model);

            // Puedes realizar otras operaciones con el detalle si es necesario
            Console.WriteLine($"Detalle recibido:{model.IdProducto} {model.Nombre} {model.Cantidad} {model.Subtotal}");
            foreach (var detall in pedidoModels)
            {
                Console.WriteLine($"Detalle almacenado:{detall.IdProducto} {detall.Nombre} {detall.Cantidad} {detall.Subtotal}");
            }
            return Json(new { success = true, message = "Detalle guardado" });
        }

        [HttpGet]
        public IActionResult RemoveProducto(int idProducto)
        {
            PedidoModel productoRemove = pedidoModels.FirstOrDefault(d => d.IdProducto == idProducto);
            if (productoRemove != null)
            {
                pedidoModels.Remove(productoRemove);
                Console.WriteLine($"Detalle con ID {productoRemove.IdProducto} eliminado.");
            }
            else
            {
                Console.WriteLine($"Detalle no encontrado.");
            }
            Console.WriteLine("Lista después de la eliminación:");
            foreach (var detalle in pedidoModels)
            {
                Console.WriteLine($"ID: {detalle.IdProducto}");
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

        // GET: DetallePedidos/Create
        public IActionResult Create()
        {
            foreach (var detall in pedidoModels)
            {
                Console.WriteLine($"Detalles almacenados:{detall.IdProducto} {detall.Nombre} {detall.Cantidad} {detall.Subtotal}");
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            return View();
        }

        // POST: DetallePedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetallePedido,IdProducto,IdPedido,CantidadProducto,PrecioUnitario")] DetallePedido detallePedido)
        {
            if (detallePedido != null)
            {
                var idPedido = ObtenerPedidoID();
                var totalPedido = 0;
                var pedidoRemove = false;
                foreach (var item in pedidoModels)
                {
                    var producto = _context.Productos.Find(item.IdProducto);
                    Console.WriteLine(producto.SaldoInventario);
                    Console.WriteLine("###############################################################");
                    if (item.Cantidad <= producto.SaldoInventario)
                    {
                        var idDetallePedido = ObtenerDetallePedidoID();
                        detallePedido.IdDetallePedido = idDetallePedido;
                        detallePedido.IdPedido = idPedido;

                        detallePedido.IdProducto = item.IdProducto;
                        detallePedido.CantidadProducto = item.Cantidad;
                        detallePedido.PrecioUnitario = item.Subtotal;

                        _context.DetallePedidos.Add(detallePedido);
                        await _context.SaveChangesAsync();

                        totalPedido += item.Subtotal;


                        producto.SaldoInventario -= item.Cantidad;
                        _context.Productos.Update(producto);
                    }
                    else
                    {
                        var idLastPedido = ObtenerPedidoID();
                        var dataPedido = _context.Pedidos.Find(idLastPedido);
                        if (dataPedido != null)
                        {
                            _context.Pedidos.Remove(dataPedido);
                            _context.SaveChanges();
                            pedidoModels.Clear();
                            pedidoRemove = true;
                        }

                        TempData["Mensaje"] = "No hay suficientes productos para crear el pedido";
                        return RedirectToAction("Index", "Pedidos");
                    }
                }
                if (!pedidoRemove)
                {
                    var pedido = _context.Pedidos.Find(idPedido);
                    if (pedido != null)
                    {

                        pedido.PrecioTotalPedido = totalPedido;
                        _context.Pedidos.Update(pedido);
                        await _context.SaveChangesAsync();
                        pedidoModels.Clear();
                    }
                }
                pedidoModels.Clear();
                return RedirectToAction("Index", "Pedidos");
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto", detallePedido.IdProducto);
            return NotFound(detallePedido);

        }
        public int ObtenerPedidoID()
        {
            var idPedido = _context.Pedidos.Max(p => (int?)p.IdPedido) ?? 1;
            return idPedido;
        }
        public int ObtenerDetallePedidoID()
        {
            var idDetallePedido = _context.DetallePedidos.Max(p => (int?)p.IdDetallePedido) ?? 0;
            return idDetallePedido + 1;
        }

        // GET: DetallePedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos.FindAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "IdPedido", detallePedido.IdPedido);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // POST: DetallePedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetallePedido,IdProducto,IdPedido,CantidadProducto,PrecioUnitario")] DetallePedido detallePedido)
        {
            if (id != detallePedido.IdDetallePedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoExists(detallePedido.IdDetallePedido))
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
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "IdPedido", detallePedido.IdPedido);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // GET: DetallePedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos
                .Include(d => d.IdPedidoNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetallePedido == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // POST: DetallePedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetallePedidos == null)
            {
                return Problem("Entity set 'dbMatissaNETContext.DetallePedidos'  is null.");
            }
            var detallePedido = await _context.DetallePedidos.AnyAsync(c => c.IdPedido == id);
            if (detallePedido == false)
            {
                var dataDetalle = await _context.DetallePedidos.FindAsync(id);
                _context.DetallePedidos.Remove(dataDetalle);
            }
            else
            {
                TempData["Mensaje"] = "No se puede eliminar el pedido";
                return RedirectToAction("Index", "Pedidos");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetallePedidoExists(int id)
        {
            return (_context.DetallePedidos?.Any(e => e.IdDetallePedido == id)).GetValueOrDefault();
        }

        public IActionResult DeletePedido()
        {
            var idLastPedido = ObtenerUltimoPedido();
            var dataPedido = _context.Pedidos.Find(idLastPedido);
            _context.Pedidos.Remove(dataPedido);
            _context.SaveChanges();
            pedidoModels.Clear();
            return RedirectToAction("Index", "Pedidos");
        }

        public int ObtenerUltimoPedido()
        {
            var idLastPedido = _context.Pedidos.Max(c => c.IdPedido);
            return idLastPedido;
        }
    }
}
