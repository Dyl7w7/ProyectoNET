
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Matissa.Models;

using X.PagedList;

namespace Matissa.Controllers
{
    public class PedidosController : Controller
    {
        private readonly dbMatissaNETContext _context;

        public PedidosController(dbMatissaNETContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index(string? buscar, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var dataPedido = from pedido in _context.Pedidos select pedido;
            var pedidoContext = _context.Pedidos.Include(d => d.IdClienteNavigation);

            if (!string.IsNullOrEmpty(buscar))
            {
                dataPedido = pedidoContext.Where(x => x.IdClienteNavigation.NombreCliente.Contains(buscar));
                var pagePedidos = await dataPedido.ToPagedListAsync(pageNumber, pageSize);
                return View(pagePedidos);
            }
            else
            {
                var pagePedidos = await pedidoContext.ToPagedListAsync(pageNumber, pageSize);
                return View(pagePedidos);
            }

        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.DetallePedidos.Where(d => d.IdPedido == id)
                .Include(p => p.IdProductoNavigation).Include(c => c.IdPedidoNavigation).Include(a => a.IdPedidoNavigation.IdClienteNavigation)
                .ToListAsync();
            if (pedido == null)
            {
                TempData["PedidoNull"] = "No existe el pedido";
                return RedirectToAction("Index", "Pedidos");
            }
            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,IdCliente,FechaPedido,PrecioTotalPedido,Estado")] Pedido pedido)
        {
            var idPedido = ObtenerPedidoID();
            pedido.IdPedido = idPedido;
            pedido.PrecioTotalPedido = 0;
            if (ModelState.IsValid)
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "DetallePedidos");
            }
            TempData["Mensaje"] = "Error";
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", pedido.IdCliente);
            return View(pedido);
        }
        public int ObtenerPedidoID()
        {
            var idPedido = _context.Pedidos.Max(p => (int?)p.IdPedido) ?? 0;
            return idPedido + 1;
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", pedido.IdCliente);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,IdCliente,FechaPedido,PrecioTotalPedido,Estado")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", pedido.IdCliente);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return await DeleteConfirmed(id);
        }

        public IActionResult DeletePedido()
        {
            var idLastPedido = ObtenerUltimoPedido();
            var dataPedido = _context.Pedidos.Find(idLastPedido);
            _context.Pedidos.Remove(dataPedido);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public int ObtenerUltimoPedido()
        {
            var idLastPedido = _context.Pedidos.Max(c => c.IdPedido);
            return idLastPedido;
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Pedidos == null)
            {
                return Problem("Entity set 'dbMatissaNETContext.Pedidos'  is null.");
            }
            var pedido = await _context.DetallePedidos.AnyAsync(c => c.IdPedido == id);
            if (pedido == false)
            {
                var dataPedido = await _context.Pedidos.FindAsync(id);
                _context.Pedidos.Remove(dataPedido);
            }
            else
            {
                TempData["Mensaje"] = "No se puede eliminar el pedido";
                return RedirectToAction("Index");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return (_context.Pedidos?.Any(e => e.IdPedido == id)).GetValueOrDefault();
        }
    }
}
