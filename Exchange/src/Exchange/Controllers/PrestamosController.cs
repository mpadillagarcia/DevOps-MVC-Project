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
using Exchange.Models.PrestamoViewModels;
using Microsoft.AspNetCore.Authorization;


namespace Exchange.Controllers
{
    [Authorize]
    public class PrestamosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrestamosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Prestamo
                .Include(p => p.Cliente)
                .Where(p => p.Cliente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo
                .Include(p => p.Cliente)
                .Include(p => p.MonedasPrestadas).ThenInclude(p => p.Criptomoneda).ThenInclude(p => p.Red)
                .FirstOrDefaultAsync(m => m.PrestamoID == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public IActionResult Create(SelectedCriptomonedaForPrestamoViewModel selectedCriptomonedas)
        {
            PrestamoCreateViewModel prestamo = new();
            prestamo.PrestamoItems = new List<PrestamoItemViewModel>();

            if (selectedCriptomonedas.IdsToAdd == null)
            {
                ModelState.AddModelError("CriptomonedaNoSelected", "You should select at least a Criptomoneda to be lent, please");
            }
            else
                prestamo.PrestamoItems = _context.Criptomoneda.Include(criptomoneda => criptomoneda.Red)
                    .Select(criptomoneda => new PrestamoItemViewModel()
                    {
                        CriptomonedaId = criptomoneda.ID,
                        Red = criptomoneda.Red.nombre,
                        Nombre = criptomoneda.Nombre,
                        Precio = criptomoneda.Precio,
                        Capitalizacion = criptomoneda.Capitalizacion,
                        PorcentajeVariacion = criptomoneda.PorcentajeVariacion
                    })
                    .Where(criptomoneda => selectedCriptomonedas.IdsToAdd.Contains(criptomoneda.CriptomonedaId.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            prestamo.Nombre = Cliente.Nombre;
            prestamo.PrimerApellido = Cliente.PrimerApellido;
            prestamo.SegundoApellido = Cliente.SegundoApellido;

            return View(prestamo);
        }

        // POST: Prestamos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PrestamoCreateViewModel prestamoViewModel)
        {
            Criptomoneda criptomoneda; MonedaPrestada prestamoItem;
            Cliente cliente;
            Prestamo prestamo = new();
            //purchase.TotalPrice = 0;
            prestamo.MonedasPrestadas = new List<MonedaPrestada>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));


            if (ModelState.IsValid)
            {
                foreach (PrestamoItemViewModel item in prestamoViewModel.PrestamoItems)
                {
                    criptomoneda = await _context.Criptomoneda.FirstOrDefaultAsync<Criptomoneda>(m => m.ID == item.CriptomonedaId);

                    if (item.Cantidad > 0)
                    {
                        //movie.QuantityForPurchase -= item.Quantity;
                        prestamoItem = new MonedaPrestada
                        {
                            Criptomoneda = criptomoneda,
                            Prestamo = prestamo,
                            Cantidad = item.Cantidad
                        };
                        //purchase.TotalPrice += item.Quantity * movie.PriceForPurchase;
                        prestamo.MonedasPrestadas.Add(prestamoItem);
                    }
                    else {
                        ModelState.AddModelError("", "Please select at least a Criptomoneda to be lent or cancel your prestamo");
                    }

                }
            }

            if (ModelState.ErrorCount > 0)
            {
                prestamoViewModel.Nombre = cliente.Nombre;
                prestamoViewModel.PrimerApellido = cliente.PrimerApellido;
                prestamoViewModel.SegundoApellido = cliente.SegundoApellido;
                return View(prestamoViewModel);
            }


            prestamo.Cliente = cliente;
            prestamo.FechaPrestamo = DateTime.Now;
            if (prestamoViewModel.MetodoPago == "PayPal")
                prestamo.MetodoPago = new PayPal()
                {
                    Email = prestamoViewModel.Email,
                    Prefijo = prestamoViewModel.Prefijo,
                    Tlf = prestamoViewModel.Tlf
                };
            else
                prestamo.MetodoPago = new TarjetaCredito()
                {
                    NumeroTarjeta = prestamoViewModel.NumeroTarjeta,
                    CVV = prestamoViewModel.CVV,
                    FechaCaducidad = (DateTime)prestamoViewModel.FechaCaducidad
                };
            _context.Add(prestamo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = prestamo.PrestamoID });
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", prestamo.ClienteId);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrestamoID,FechaPrestamo,TasaInteres,ClienteId")] Prestamo prestamo)
        {
            if (id != prestamo.PrestamoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.PrestamoID))
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
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", prestamo.ClienteId);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamo
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.PrestamoID == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamo.FindAsync(id);
            _context.Prestamo.Remove(prestamo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamo.Any(e => e.PrestamoID == id);
        }
    }
}
