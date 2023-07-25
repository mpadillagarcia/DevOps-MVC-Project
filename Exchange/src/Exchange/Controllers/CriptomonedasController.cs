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
using Microsoft.AspNetCore.Authorization;

namespace Exchange.Controllers
{

    [Authorize]

    public class CriptomonedasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CriptomonedasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Criptomonedas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Criptomoneda.ToListAsync());
        }

        // GET: Criptomonedas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criptomoneda = await _context.Criptomoneda
                .FirstOrDefaultAsync(m => m.ID == id);
            if (criptomoneda == null)
            {
                return NotFound();
            }

            return View(criptomoneda);
        }

        // GET: Criptomonedas/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Criptomonedas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Precio,PorcentajeVariacion,Capitalizacion,NombreRed,CantidadAComprar,CantidadAVender")] Criptomoneda criptomoneda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(criptomoneda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(criptomoneda);
        }

        // GET: Criptomonedas/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criptomoneda = await _context.Criptomoneda.FindAsync(id);
            if (criptomoneda == null)
            {
                return NotFound();
            }
            return View(criptomoneda);
        }

        // POST: Criptomonedas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Precio,PorcentajeVariacion,Capitalizacion,NombreRed,CantidadAComprar,CantidadAVender")] Criptomoneda criptomoneda)
        {
            if (id != criptomoneda.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(criptomoneda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CriptomonedaExists(criptomoneda.ID))
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
            return View(criptomoneda);
        }

        // GET: Criptomonedas/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criptomoneda = await _context.Criptomoneda
                .FirstOrDefaultAsync(m => m.ID == id);
            if (criptomoneda == null)
            {
                return NotFound();
            }

            return View(criptomoneda);
        }

        // POST: Criptomonedas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var criptomoneda = await _context.Criptomoneda.FindAsync(id);
            _context.Criptomoneda.Remove(criptomoneda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CriptomonedaExists(int id)
        {
            return _context.Criptomoneda.Any(e => e.ID == id);
        }

        // GET: Movies/SelectMoviesForPurchase
        [HttpGet]
        public IActionResult SeleccionCriptomonedasParaCompra(string criptomonedaNombre, string criptomonedaRedSeleccionada, int Precio, float PorcentajeVariacion)
        {
            SeleccionCriptomonedasParaCompraViewModel seleccionCriptomonedas = new SeleccionCriptomonedasParaCompraViewModel();
            seleccionCriptomonedas.Red = new SelectList(_context.Red.Select(g => g.nombre).ToList());

            seleccionCriptomonedas.Criptomonedas = _context.Criptomoneda.Where(m => m.CantidadAComprar > 0).Include(m => m.Red);
            // seleccionCriptomonedas.Criptomonedas = _context.Criptomoneda.Include(m => m.Red);

            if (criptomonedaNombre != null)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.Nombre.Contains(criptomonedaNombre));
            }

            if (criptomonedaRedSeleccionada != null)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.Red.nombre.Contains(criptomonedaRedSeleccionada));
            }

            if (PorcentajeVariacion != 0)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.PorcentajeVariacion.Equals(PorcentajeVariacion));
            }

            if (Precio != 0)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.Precio.Equals(Precio));
            }

            seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.ToList();
            return View(seleccionCriptomonedas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionCriptomonedasParaCompra(SeleccionadasCriptomonedasParaCompraViewModel seleccionadasCriptomonedas)
        {
            if (seleccionadasCriptomonedas.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Compras", seleccionadasCriptomonedas);
            }
            //a message error will be shown to the customer in case no movies are selected
            ModelState.AddModelError(string.Empty, "Necesitas seleccionar al menos una criptomoneda");

            //the View SelectMoviesForPurchase will be shown again
            return SeleccionCriptomonedasParaCompra(seleccionadasCriptomonedas.criptomonedaNombre, seleccionadasCriptomonedas.criptomonedaRedSeleccionada, seleccionadasCriptomonedas.Precio, seleccionadasCriptomonedas.PorcentajeVariacion);


        }


        [HttpGet]
        public IActionResult SeleccionMonedasParaVender(string NombreMoneda, string RedMonedaSeleccionada, int Capitalizacion, float PorcentajeVariacion)
        {
            CriptomonedasParaVenderViewModel seleccionMonedas = new CriptomonedasParaVenderViewModel();

            //Lista de Redes existentes en la Base de Datos
            seleccionMonedas.Redes = new SelectList(_context.Red.Select(g => g.nombre).ToList());

            seleccionMonedas.Criptomonedas = _context.Criptomoneda.Where(m => m.CantidadAVender > 0).Include(m => m.Red);
            

            if (NombreMoneda != null)
            {
                seleccionMonedas.Criptomonedas = seleccionMonedas.Criptomonedas.Where(m => m.Nombre.Contains(NombreMoneda));
            }

            if (RedMonedaSeleccionada != null)
            {
                seleccionMonedas.Criptomonedas = seleccionMonedas.Criptomonedas.Where(m => m.Red.nombre.Contains(RedMonedaSeleccionada));
            }

            if (Capitalizacion != 0)
            {
                seleccionMonedas.Criptomonedas = seleccionMonedas.Criptomonedas.Where(m => m.Capitalizacion.Equals(Capitalizacion));
            }

            if (PorcentajeVariacion != 0)
            {
                seleccionMonedas.Criptomonedas = seleccionMonedas.Criptomonedas.Where(m => m.PorcentajeVariacion.Equals(PorcentajeVariacion));
            }

            seleccionMonedas.Criptomonedas = seleccionMonedas.Criptomonedas.ToList();
            return View(seleccionMonedas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionMonedasParaVender(MonedasSeleccionadasParaVenderViewModel MonedasSeleccionadas)
        {
            if (MonedasSeleccionadas.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Ventas", MonedasSeleccionadas);
            }

            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos una Criptomoneda");


            return SeleccionMonedasParaVender(MonedasSeleccionadas.NombreMoneda, MonedasSeleccionadas.RedMonedaSeleccionada, MonedasSeleccionadas.Capitalizacion, MonedasSeleccionadas.PorcentajeVariacion);


        }


        public IActionResult SeleccionCriptomonedasParaAlerta(string criptomonedaNombre,
            string criptomonedaRedSeleccionada, int Capitalizacion, float PorcentajeVariacion)
        {
            SeleccionCriptomonedasParaAlertaViewModel seleccionCriptomonedas = new SeleccionCriptomonedasParaAlertaViewModel();

            seleccionCriptomonedas.Red = new SelectList(_context.Red.Select(g => g.nombre).ToList());

            seleccionCriptomonedas.Criptomonedas = _context.Criptomoneda.Include(m => m.Red);

            if (criptomonedaNombre != null)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.Nombre.Contains(criptomonedaNombre));
            }

            if (criptomonedaRedSeleccionada != null)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.Red.nombre.Contains(criptomonedaRedSeleccionada));
            }

            if (Capitalizacion != 0)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.Capitalizacion.Equals(Capitalizacion));
            }

            if (PorcentajeVariacion != 0)
            {
                seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.Where(m => m.PorcentajeVariacion.Equals(PorcentajeVariacion));
            }

            seleccionCriptomonedas.Criptomonedas = seleccionCriptomonedas.Criptomonedas.ToList();

            return View(seleccionCriptomonedas);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionCriptomonedasParaAlerta(SeleccionadasCriptomonedasParaAlertaViewModel seleccionadasCriptomonedas)
        {
            // Si el usuario ha seleccionado alguna película, entonces crearemos la compra.
            // Para ello llamaremos al método de acción Create (GET) de Purchase.
            if (seleccionadasCriptomonedas.IdsToAdd != null)
            {
                return RedirectToAction("Create", "Alertas", seleccionadasCriptomonedas);
            }
            // Si el usuario no ha seleccionado ninguna película, le informaremos y
            // se vuelve a generar el ViewModel
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos una criptomoneda");
            //the View SelectMoviesForPurchase will be shown again
            return SeleccionCriptomonedasParaAlerta(seleccionadasCriptomonedas.criptomonedaNombre, seleccionadasCriptomonedas.criptomonedaRedSeleccionada, seleccionadasCriptomonedas.capitalizacion, seleccionadasCriptomonedas.porcentajeVariacion);
        }

        
        [HttpGet]
        public IActionResult SelectCriptomonedaForPrestamo(string NombreMoneda, string RedMonedaSeleccionada, int Precio, int Capitalizacion, float PorcentajeVariacion)
        {
            SelectCriptomonedasForPrestamoViewModel selectCriptomonedas = new SelectCriptomonedasForPrestamoViewModel();
            selectCriptomonedas.Redes = new SelectList(_context.Red.Select(g => g.nombre).ToList());

            /*selectCriptomonedas.Criptomonedas = _context.Criptomoneda.Include(m => m.Red)
                .Where(Criptomoneda => Criptomoneda.Nombre.Contains(NombreMoneda) || NombreMoneda == null
                    && (Criptomoneda.Red.nombre.Contains(RedMonedaSeleccionada) || RedMonedaSeleccionada == null)); */


            selectCriptomonedas.Criptomonedas = _context.Criptomoneda.Include(m => m.Red);

            if (NombreMoneda is not null)
            {
                selectCriptomonedas.Criptomonedas = selectCriptomonedas.Criptomonedas.Where(Criptomoneda => Criptomoneda.Nombre.Contains(NombreMoneda));
            }

            if (RedMonedaSeleccionada is not null)
            {
                selectCriptomonedas.Criptomonedas = selectCriptomonedas.Criptomonedas.Where(Criptomoneda => Criptomoneda.Red.nombre.Contains(RedMonedaSeleccionada));
            }

            if (Precio != 0)
            {
                selectCriptomonedas.Criptomonedas = selectCriptomonedas.Criptomonedas.Where(Criptomoneda => Criptomoneda.Precio.Equals(Precio));
            }

            if (Capitalizacion != 0)
            {
                selectCriptomonedas.Criptomonedas = selectCriptomonedas.Criptomonedas.Where(Criptomoneda => Criptomoneda.Capitalizacion.Equals(Capitalizacion));
            }

            if (PorcentajeVariacion != 0)
            {
                selectCriptomonedas.Criptomonedas = selectCriptomonedas.Criptomonedas.Where(Criptomoneda => Criptomoneda.PorcentajeVariacion.Equals(PorcentajeVariacion));
            }


            selectCriptomonedas.Criptomonedas = selectCriptomonedas.Criptomonedas.ToList();
            return View(selectCriptomonedas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectCriptomonedaForPrestamo(SelectedCriptomonedaForPrestamoViewModel selectedCriptomonedas)
        {
            if (selectedCriptomonedas.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Prestamos", selectedCriptomonedas);
            }

            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos una Criptomoneda");


            return SelectCriptomonedaForPrestamo(selectedCriptomonedas.NombreMoneda, selectedCriptomonedas.RedMonedaSeleccionada, selectedCriptomonedas.Precio,
                selectedCriptomonedas.Capitalizacion, selectedCriptomonedas.PorcentajeVariacion);


        }


    }
}