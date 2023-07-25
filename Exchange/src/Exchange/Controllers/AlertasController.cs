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
using Exchange.Models.AlertaViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Exchange.Controllers
{
    public class AlertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            //only the purchased made by the connected user will be shown
            var applicationDbContext = _context.Alerta
                .Include(p => p.Cliente)
                .Where(p => p.Cliente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alerta = await _context.Alerta
                .Include(p => p.Cliente)
                .Include(p => p.MonedaAlertar).ThenInclude(p => p.Criptomoneda)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alerta == null)
            {
                return NotFound();
            }

            return View(alerta);
        }

        // GET: Purchases/Create
        public IActionResult Create(SeleccionadasCriptomonedasParaAlertaViewModel seleccionadasCriptomonedas)
        {
            AlertaCreateViewModel alerta = new();
            alerta.MonedaAlertar = new List<AlertaItemViewModel>();

            if (seleccionadasCriptomonedas.IdsToAdd == null)
            {
                ModelState.AddModelError("Criptomoneda no seleccionada", "Por favor, debes de seleccionar al menos una criptomoneda para crear la alerta");
            }
            else
                alerta.MonedaAlertar = _context.Criptomoneda.Include(criptomoneda => criptomoneda.Red)
                    .Select(criptomoneda => new AlertaItemViewModel()
                    {
                        ID = criptomoneda.ID,
                        NombreRed = criptomoneda.Red.nombre,
                        Precio = criptomoneda.Precio,
                        Nombre = criptomoneda.Nombre
                    })
                    .Where(criptomoneda => seleccionadasCriptomonedas.IdsToAdd.Contains(criptomoneda.ID.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            alerta.Nombre = Cliente.Nombre;
            alerta.PrimerApellido = Cliente.PrimerApellido;
            alerta.SegundoApellido = Cliente.SegundoApellido;

            return View(alerta);
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(AlertaCreateViewModel alertaViewModel)
        {
            Criptomoneda criptomoneda; MonedaAlerta monedaAlerta;
            Cliente cliente;
            Alerta alerta = new();
            
            alerta.MonedaAlertar = new List<MonedaAlerta>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            
            if (ModelState.IsValid)
            {
                foreach (AlertaItemViewModel item in alertaViewModel.MonedaAlertar)
                {
                    criptomoneda = await _context.Criptomoneda.FirstOrDefaultAsync<Criptomoneda>(m => m.ID == item.ID);

                }
            }

            

            if (ModelState.ErrorCount > 0)
            {
                alertaViewModel.Nombre = cliente.Nombre;
                alertaViewModel.PrimerApellido = cliente.PrimerApellido;
                alertaViewModel.SegundoApellido = cliente.SegundoApellido;
                return View(alertaViewModel);
            }

            
            alerta.Cliente = cliente;
            alerta.FechaAlerta = DateTime.Now;
            /*
            if (purchaseViewModel.PaymentMethod == "PayPal")
                purchase.PaymentMethod = new PayPal()
                {
                    Email = purchaseViewModel.Email,
                    Prefix = purchaseViewModel.Prefix,
                    Phone = purchaseViewModel.Phone
                };
            else
                purchase.PaymentMethod = new CreditCard()
                {
                    CreditCardNumber = purchaseViewModel.CreditCardNumber,
                    CCV = purchaseViewModel.CCV,
                    ExpirationDate = (DateTime)purchaseViewModel.ExpirationDate
                };
            */
            alerta.FechaExpira = alertaViewModel.FechaExpira;

            
            _context.Add(alerta);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { Id = alerta.Id });

            


        }

        // GET: Purchases/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alerta = await _context.Alerta.FindAsync(id);
            if (alerta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", alerta.Cliente);
            return View(alerta);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaAlerta,FechaExpira,ClienteId")] Alerta alerta)
        {
            if (id != alerta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alerta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(alerta.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", alerta.Cliente);
            return View(alerta);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alerta = await _context.Alerta
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alerta == null)
            {
                return NotFound();
            }

            return View(alerta);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alerta = await _context.Alerta.FindAsync(id);
            _context.Alerta.Remove(alerta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Alerta.Any(e => e.Id == id);
        }
    }
}
