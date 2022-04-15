﻿using InmobiliariaOrtega.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace InmobiliariaOrtega.Controllers
{
    [Authorize(Policy = "Empleado")]
    public class ContratoController : Controller
    {
        private readonly IConfiguration configuration;
        private RepositorioContrato repositorioContrato;
        private RepositorioInmueble repositorioInmueble;
        private RepositorioInquilino repositorioInquilino;

        public ContratoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioContrato = new RepositorioContrato(configuration);
            repositorioInmueble = new RepositorioInmueble(configuration);
            repositorioInquilino = new RepositorioInquilino(configuration);
        }

        // GET: ContratoController
        public ActionResult Index(int InmuebleId = 0, int InquilinoId = 0)
        {
            ViewData["Inmuebles"] = repositorioInmueble.ObtenerTodos<Inmueble>();
            ViewData["Inquilinos"] = repositorioInquilino.ObtenerTodos<Inquilino>();

            List<Contrato> contratos;
            if (InmuebleId != 0)
                contratos = repositorioContrato.ObtenerPorInmueble(InmuebleId);
            else if (InquilinoId != 0)
                contratos = repositorioContrato.ObtenerPorInquilino(InquilinoId);
            else
                contratos = repositorioContrato.ObtenerTodos();
            return View(contratos);
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            Contrato c = repositorioContrato.ObtenerPorId_v2(id);
            return View(c);
        }

        public ActionResult Buscar(string data)
        {
            try
            {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
                Dictionary<string, string> filtros = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL

                string condiciones = "";
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                int inmuebleId = Convert.ToInt32(filtros["InmuebleId"]);
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                condiciones += inmuebleId == 0 ? "" : " AND InmuebleId = " + inmuebleId;

                int inquilinoId = Convert.ToInt32(filtros["InquilinoId"]);
                condiciones += inquilinoId == 0 ? "" : " AND InquilinoId = " + inquilinoId;

                int estado = Convert.ToInt32(filtros["Estado"]);
                condiciones += estado == 0 ? "" : $" AND Estado = '{estado}'";

                string fechaDesde = filtros["FechaDesde"].ToString();
                string fechaHasta = filtros["FechaHasta"].ToString();

                DateTime desde = fechaDesde == "" ? DateTime.MinValue : DateTime.Parse(fechaDesde);
                DateTime hasta = fechaHasta == "" ? DateTime.MaxValue : DateTime.Parse(fechaHasta);

                List<Contrato> lista = repositorioContrato.ObtenerPorBusqueda(condiciones, desde, hasta);
                return PartialView("Fila", lista);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            ViewData["Inmuebles"] = repositorioInmueble.ObtenerVisibles();
            ViewData["Inquilinos"] = repositorioInquilino.ObtenerTodos<Inquilino>();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Contrato e = new Contrato()
                {
                    InquilinoId = Convert.ToInt32(collection["InquilinoId"]),
                    InmuebleId = Convert.ToInt32(collection["InmuebleId"]),
                    FechaDesde = DateTime.Parse(collection["FechaDesde"]),
                    Estado = 1
                };
                e.Precio = repositorioInmueble.ObtenerPorId<Inmueble>(e.InmuebleId).Precio;

                // Por FechaHasta solo viene año y mes. Settear el mismo dia que FechaDesde
                e.FechaHasta = DateTime.Parse(collection["FechaHasta"]);
                int mesInput = e.FechaHasta.Month; // Guardar el mes ingresado para ver si el AddDays lo cambia
                e.FechaHasta = e.FechaHasta.AddDays(e.FechaDesde.Day - 1);

                // Comprobar si el AddDays cambio el mes (si el dia inicio no existe en el mes final)
                if (mesInput != e.FechaHasta.Month)
                {
                    switch (e.FechaHasta.Month)
                    {
                        case 3: // Meses siguientes a los meses cortos- porque el AddDays pasa al siguiente mes si lo necesita
                            e.FechaHasta = new DateTime(e.FechaHasta.Year, mesInput, DateTime.IsLeapYear(e.FechaHasta.Year) ? 29 : 28);
                            break;
                        case 5:
                        case 7:
                        case 10:
                        case 12:
                            if (e.FechaDesde.Day > 30)
                                e.FechaHasta = new DateTime(e.FechaHasta.Year, mesInput, 30);
                            break;
                    }
                }

                if (e.FechaDesde > e.FechaHasta || (e.FechaDesde.Month == e.FechaHasta.Month && e.FechaDesde.Year == e.FechaHasta.Year))
                    throw new Exception("La fecha final del contrato no puede ser menor o del mismo mes que la fecha inicial.");
                if (!repositorioInmueble.Disponible(e.InmuebleId, e.FechaDesde, e.FechaHasta))
                    throw new Exception("No se puede crear el contrato porque el inmueble esta ocupado en el periodo ingresado.");

                repositorioContrato.Alta(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                ViewData["Inmuebles"] = repositorioInmueble.ObtenerVisibles();
                ViewData["Inquilinos"] = repositorioInquilino.ObtenerTodos<Inquilino>();
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            List<Inmueble> inmuebles = repositorioInmueble.ObtenerVisibles(); // Mostrar los disponibles + el seleccionado actualmente
            Inmueble actual = repositorioInmueble.ObtenerPorId<Inmueble>(repositorioContrato.ObtenerPorId<Contrato>(id).InmuebleId);
            if (actual.Visible == 0)
                inmuebles.Insert(0, actual);

            ViewData["Inmuebles"] = inmuebles;
            ViewData["Inquilinos"] = repositorioInquilino.ObtenerTodos<Inquilino>();
            return View(repositorioContrato.ObtenerPorId<Contrato>(id));
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Contrato e = new Contrato()
                {
                    Id = id,
                    InquilinoId = Convert.ToInt32(collection["InquilinoId"]),
                    InmuebleId = Convert.ToInt32(collection["InmuebleId"]),
                    FechaDesde = DateTime.Parse(collection["FechaDesde"]),
                    Estado = 1
                };
                // Por FechaHasta solo viene año y mes. Settear el mismo dia que FechaDesde
                e.FechaHasta = DateTime.Parse(collection["FechaHasta"]);
                int mesInput = e.FechaHasta.Month; // Guardar el mes ingresado para ver si el AddDays lo cambia
                e.FechaHasta = e.FechaHasta.AddDays(e.FechaDesde.Day - 1);

                // Comprobar si el AddDays cambio el mes (si el dia inicio no existe en el mes final)
                if (mesInput != e.FechaHasta.Month)
                {
                    switch (e.FechaHasta.Month)
                    {
                        case 3: // Meses siguientes a los meses cortos- porque el AddDays pasa al siguiente mes si lo necesita
                            e.FechaHasta = new DateTime(e.FechaHasta.Year, mesInput, DateTime.IsLeapYear(e.FechaHasta.Year) ? 29 : 28);
                            break;
                        case 5:
                        case 7:
                        case 10:
                        case 12:
                            if (e.FechaDesde.Day > 30)
                                e.FechaHasta = new DateTime(e.FechaHasta.Year, mesInput, 30);
                            break;
                    }
                }

                if (e.FechaDesde > e.FechaHasta || (e.FechaDesde.Month == e.FechaHasta.Month && e.FechaDesde.Year == e.FechaHasta.Year))
                    throw new Exception("La fecha final del contrato no puede ser menor o del mismo mes que la fecha inicial");
                if (!repositorioInmueble.Disponible(e.InmuebleId, e.FechaDesde, e.FechaHasta, e.Id))
                    throw new Exception("El inmueble esta ocupado por otro contrato en el periodo de fechas ingresado.");

                repositorioContrato.Editar(e);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                List<Inmueble> inmuebles = repositorioInmueble.ObtenerVisibles(); // Mostrar los disponibles + el seleccionado actualmente
                Inmueble actual = repositorioInmueble.ObtenerPorId<Inmueble>(repositorioContrato.ObtenerPorId<Contrato>(id).InmuebleId);
                inmuebles.Insert(0, actual);

                TempData["Error"] = ex.Message;
                ViewData["Inmuebles"] = inmuebles;
                ViewData["Inquilinos"] = repositorioInquilino.ObtenerTodos<Inquilino>();
                return View(repositorioContrato.ObtenerPorId<Contrato>(id));
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            return View(repositorioContrato.ObtenerPorId<Contrato>(id));
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [Authorize(Policy = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Contrato e = repositorioContrato.ObtenerPorId<Contrato>(id);

                repositorioContrato.Eliminar(e.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                if (ex is SqlException && (ex as SqlException).Number == 547)
                    msg = "No se puede eliminar este Contrato porque ya se realizaron Pagos sobre él.";
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                TempData["Error"] = msg;
                return View(repositorioContrato.ObtenerPorId<Contrato>(id));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Terminar(IFormCollection collection)
        {
            Contrato e = repositorioContrato.ObtenerPorId_v2(Convert.ToInt32(collection["TerminarContratoId"].ToString()));
            try
            {
                if (e.Estado != 1)
                    throw new Exception("No se puede marcar como terminado porque el contrato ya esta " + e.NombreEstado + ".");
                if (e.ProximoPagoTexto != "N/A")
                    throw new Exception("No se puede marcar como terminado porque existen pagos pendientes.");

                repositorioContrato.CambiarEstado(e.Id, 2);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id = e.Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renovar(IFormCollection collection)
        {
            Contrato anterior = repositorioContrato.ObtenerPorId_v2(Convert.ToInt32(collection["ContratoId"]));
            try
            {
                if (anterior.Inmueble.Visible == 0)
                    throw new Exception("No se puede renovar porque el inmueble está oculto.");
                if (anterior.Estado != 1)
                    throw new Exception("No se puede renovar porque el contrato ya esta " + anterior.NombreEstado + ".");
                if (!(anterior.EstadoPagos == "Al día" || anterior.EstadoPagos == "Finalizados"))
                    throw new Exception("No se puede renovar porque los pagos no están al día.");
                if (anterior.FechaHasta < DateTime.Today)
                    throw new Exception("No se puede renovar el contrato porque ya se superó la fecha de finalización.");
                //if (DateTime.Now < anterior.FechaHasta.AddMonths(-1))
                //throw new Exception("No se puede renovar el contrato porque todavía no se alcanzo el último mes del mismo.");

                /*DateTime fechaRenovacion = DateTime.Parse(collection["RenovarContratoFecha"].ToString()).AddDays(anterior.FechaHasta.Day - 1);

                // Corregir fechas si el contrato empieza en los ultimos días del mes y termina en uno de los meses de pocos días
                if (anterior.FechaHasta.Day > 28)
                {
                    switch (fechaRenovacion.Month)
                    {
                        case 3: // Meses siguientes a los meses deformes- porque el AddDays de arriba^^ los ignora y rompe el calculo de Contrato.TotalMeses
                            fechaRenovacion = new DateTime(fechaRenovacion.Year, 2, DateTime.IsLeapYear(fechaRenovacion.Year) ? 29 : 28);
                            break;
                        case 5: case 7: case 10: case 12:
                            if (anterior.FechaHasta.Day > 30)
                                fechaRenovacion = new DateTime(fechaRenovacion.Year, fechaRenovacion.Month - 1, 30);
                            break;
                    }
                }*/

                // Por FechaHasta solo viene año y mes. Settear el mismo dia que FechaDesde
                DateTime fechaRenovacion = DateTime.Parse(collection["RenovarContratoFecha"]);
                int mesInput = fechaRenovacion.Month; // Guardar el mes ingresado para ver si el AddDays lo cambia
                fechaRenovacion = fechaRenovacion.AddDays(anterior.FechaDesde.Day - 1);

                // Comprobar si el AddDays cambio el mes (si el dia inicio no existe en el mes final)
                if (mesInput != fechaRenovacion.Month)
                {
                    switch (fechaRenovacion.Month)
                    {
                        case 3: // Meses siguientes a los meses cortos- porque el AddDays pasa al siguiente mes si lo necesita
                            fechaRenovacion = new DateTime(fechaRenovacion.Year, mesInput, DateTime.IsLeapYear(fechaRenovacion.Year) ? 29 : 28);
                            break;
                        case 5:
                        case 7:
                        case 10:
                        case 12:
                            if (fechaRenovacion.Day > 30)
                                fechaRenovacion = new DateTime(fechaRenovacion.Year, mesInput, 30);
                            break;
                    }
                }

                if (!repositorioInmueble.Disponible(anterior.InmuebleId, anterior.FechaHasta, fechaRenovacion, anterior.Id))
                    throw new Exception("No se puede renovar porque el inmueble está ocupado por otro contrato en el periodo de fechas ingresado.");

                Contrato renovacion = new Contrato()
                {
                    InquilinoId = anterior.InquilinoId,
                    InmuebleId = anterior.InmuebleId,
                    FechaDesde = anterior.FechaHasta,
                    FechaHasta = fechaRenovacion,
                    Estado = 1
                };

                repositorioContrato.Alta(renovacion);
                //repositorioContrato.CambiarEstado(anterior.Id, 3);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id = anterior.Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Romper(IFormCollection collection)
        {
            Contrato e = repositorioContrato.ObtenerPorId_v2(Convert.ToInt32(collection["RomperContratoId"]));
            try
            {
                if (e.Estado != 1)
                    throw new Exception("No se puede romper porque el contrato ya esta " + e.NombreEstado + ".");
                if (e.ProximoPago <= DateTime.Now)
                    throw new Exception("No se puede romper el contrato porque los pagos no estan al día.");

                e.FechaHasta = e.FechaDesde > DateTime.Now ? e.FechaDesde : (DateTime.Now);
                e.Estado = 3;
                repositorioContrato.Editar(e);
                //repositorioContrato.CambiarEstado(e.Id, 4);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id = e.Id });
            }
        }
    }
}
