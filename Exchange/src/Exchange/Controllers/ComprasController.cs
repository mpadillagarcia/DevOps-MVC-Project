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
using Exchange.Models.CompraViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Exchange.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Compras
        public async Task<IActionResult> Index()
        {
            //only the comprad made by the connected user will be shown
            var applicationDbContext = _context.Compra
                .Include(p => p.Cliente)
                .Where(p => p.Cliente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .Include(p => p.Cliente)
                .Include(p => p.CompraItems).ThenInclude(p => p.Criptomoneda)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create(SeleccionadasCriptomonedasParaCompraViewModel seleccionadasCriptomonedas)
        {
            CompraCreateViewModel compra = new();
            compra.CompraItems = new List<CompraItemViewModel>();

            if (seleccionadasCriptomonedas.IdsToAdd == null)
            {
                ModelState.AddModelError("Criptomoneda no seleccionada", "Debes seleccionar al menos una criptomoneda");
            }
            else
                compra.CompraItems = _context.Criptomoneda.Include(Criptomoneda => Criptomoneda.Red)
                    .Select(criptomoneda => new CompraItemViewModel()
                    {
                        ID = criptomoneda.ID,
                        Red = criptomoneda.Red.nombre,
                        Precio = criptomoneda.Precio,
                        Nombre = criptomoneda.Nombre
                    })
                    .Where(criptomoneda => seleccionadasCriptomonedas.IdsToAdd.Contains(criptomoneda.ID.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            compra.Nombre = Cliente.Nombre;
            compra.PrimerApellido = Cliente.PrimerApellido;
            compra.SegundoApellido = Cliente.SegundoApellido;

            return View(compra);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CompraCreateViewModel compraViewModel)
        {
            Criptomoneda criptomoneda; CompraItem compraItem;
            Cliente Cliente;
            Compra compra = new();
            compra.PrecioTotal = 0;
            compra.CompraItems = new List<CompraItem>();
            Cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));


            if (ModelState.IsValid)
            {
                foreach (CompraItemViewModel item in compraViewModel.CompraItems)
                {
                    criptomoneda = await _context.Criptomoneda.FirstOrDefaultAsync<Criptomoneda>(m => m.ID == item.ID);
                    if (criptomoneda.CantidadAComprar < item.Cantidad)
                    {
                        ModelState.AddModelError("", $"No hay suficientes Criptomonedas con el nombre {criptomoneda.Nombre}, porfavor selecciona menos o igual a {criptomoneda.CantidadAComprar}");
                    }
                    else
                    {
                        if (item.Cantidad > 0)
                        {
                            criptomoneda.CantidadAComprar -= item.Cantidad;
                            compraItem = new CompraItem
                            {
                                Criptomoneda = criptomoneda,
                                Compra = compra,
                                Cantidad = item.Cantidad
                            };
                            compra.PrecioTotal += item.Cantidad * criptomoneda.Precio;
                            compra.CompraItems.Add(compraItem);
                        }
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                compraViewModel.Nombre = Cliente.Nombre;
                compraViewModel.PrimerApellido = Cliente.PrimerApellido;
                compraViewModel.SegundoApellido = Cliente.SegundoApellido;
                return View(compraViewModel);
            }


            compra.Cliente = Cliente;
            compra.CompraFecha = DateTime.Now;
            if (compraViewModel.MetodoPago == "PayPal")
                compra.MetodoPago = new PayPal()
                {
                    Email = compraViewModel.Email,
                    Prefijo = compraViewModel.Prefijo,
                    Tlf = compraViewModel.Tlf
                };
            else
                compra.MetodoPago = new TarjetaCredito()
                {
                    NumeroTarjeta = compraViewModel.NumeroTarjeta,
                    CVV = compraViewModel.CVV,
                    FechaCaducidad = (DateTime)compraViewModel.FechaExpiracion
                };
            _context.Add(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = compra.CompraId });
        }

        // GET: Compras/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", compra.ClienteId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("CompraId,PrecioTotal,CompraFecha,ClienteId")] Compra compra)
        {
            if (id != compra.CompraId)
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
                    if (!CompraExists(compra.CompraId))
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
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", compra.ClienteId);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.CompraId == id);
        }
    }
}
