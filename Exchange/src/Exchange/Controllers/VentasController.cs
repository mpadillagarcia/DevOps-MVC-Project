using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exchange.Data;
using Exchange.Models;
using Exchange.Models.CriptomonedaViewModels;
using Exchange.Models.VentaViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Exchange.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            //only the purchased made by the connected user will be shown
            var applicationDbContext = _context.Venta
                .Include(p => p.Cliente)
                .Where(p => p.Cliente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(p => p.Cliente)
                .Include(p => p.MonedasVendidas).ThenInclude(p => p.Criptomoneda)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create(MonedasSeleccionadasParaVenderViewModel monedasSeleccionadas)
        {
            VentaCreateViewModel venta = new();
            venta.MonedasVendidas = new List<VentaItemViewModel>();

            if (monedasSeleccionadas.IdsToAdd == null)
            {
                ModelState.AddModelError("CriptomonedaNoSeleccionada", "Debes seleccionar al menos una moneda para vender");
            }
            else
                venta.MonedasVendidas = _context.Criptomoneda.Include(criptomoneda => criptomoneda.Red)
                    .Select(criptomoneda => new VentaItemViewModel()
                    {
                        ID = criptomoneda.ID,
                        Red = criptomoneda.Red.nombre,
                        Precio = criptomoneda.Precio,
                        Nombre = criptomoneda.Nombre
                    })
                    .Where(criptomoneda => monedasSeleccionadas.IdsToAdd.Contains(criptomoneda.ID.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            venta.Nombre = Cliente.Nombre;
            venta.PrimerApellido = Cliente.PrimerApellido;
            venta.SegundoApellido = Cliente.SegundoApellido;

            return View(venta);
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(VentaCreateViewModel ventaViewModel)
        {
            Criptomoneda criptomoneda; MonedaVendida monedaVendida;
            Cliente cliente;
            Venta venta = new();
            venta.EquivEuros = 0;
            venta.MonedasVendidas = new List<MonedaVendida>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));


            if (ModelState.IsValid)
            {
                foreach (VentaItemViewModel item in ventaViewModel.MonedasVendidas)
                {
                    criptomoneda = await _context.Criptomoneda.FirstOrDefaultAsync<Criptomoneda>(m => m.ID == item.ID);
                    if (criptomoneda.CantidadAVender < item.CantidadAVender)
                    {
                        ModelState.AddModelError("", $"No hay tanto {criptomoneda.Nombre}, selecciona menos o igual a {criptomoneda.CantidadAVender}");
                    }
                    else
                    {
                        if (item.CantidadAVender > 0)
                        {
                            criptomoneda.CantidadAVender -= item.CantidadAVender;
                            monedaVendida = new MonedaVendida
                            {
                                Criptomoneda = criptomoneda,
                                Venta = venta,
                                CantidadVenta = item.CantidadAVender

                            };
                            venta.EquivEuros += item.CantidadAVender * criptomoneda.Precio;
                            //venta.EquivEuros ++;
                            venta.MonedasVendidas.Add(monedaVendida);
                        }
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                ventaViewModel.Nombre = cliente.Nombre;
                ventaViewModel.PrimerApellido = cliente.PrimerApellido;
                ventaViewModel.SegundoApellido = cliente.SegundoApellido;
                return View(ventaViewModel);
            }


            venta.Cliente = cliente;
            venta.FechaVenta = DateTime.Now;
            if (ventaViewModel.MetodoPago == "PayPal")
                venta.MetodoPago = new PayPal()
                {
                    Email = ventaViewModel.Email,
                    Prefijo = ventaViewModel.Prefijo,
                    Tlf = ventaViewModel.tlf
                };
            else
                venta.MetodoPago = new TarjetaCredito()
                {
                    NumeroTarjeta = ventaViewModel.NumeroTarjeta,
                    CVV = ventaViewModel.CVV,
                    FechaCaducidad = (DateTime)ventaViewModel.FechaCaducidad
                };

            _context.Add(venta);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = venta.VentaId });
        }

        // GET: Ventas/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", venta.ClienteId);
            return View(venta);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("VentaId,EquivEuros,FechaVenta,ClienteId")] Venta venta)
        {
            if (id != venta.VentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
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
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", venta.ClienteId);
            return View(venta);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            _context.Venta.Remove(venta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.VentaId == id);
        }
    }
}