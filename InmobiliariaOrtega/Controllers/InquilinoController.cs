﻿using InmobiliariaOrtega.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaOrtega.Controllers
{
    [Authorize(Policy = "Empleado")]
    public class InquilinoController : Controller
    {
        private readonly IConfiguration configuration;
        private RepositorioInquilino repositorioInquilino;

        public InquilinoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioInquilino = new RepositorioInquilino(configuration);
        }

        // GET: TestController
        public ActionResult Index()
        {
            return View(repositorioInquilino.ObtenerTodos<Inquilino>());
        }

        // GET: TestController/Details/5
        public ActionResult Details(int id)
        {
            return View(repositorioInquilino.ObtenerPorId<Inquilino>(id));
        }

        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino e)
        {
            try
            {
                repositorioInquilino.Alta(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                if (ex is SqlException && (ex as SqlException).Number == 2627)
                    msg = "Ya existe un inquilino asociado a ese Email.";
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                TempData["Error"] = msg;
                return View();
            }
        }

        // GET: TestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(repositorioInquilino.ObtenerPorId<Inquilino>(id));
        }

        // POST: TestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino e)
        {
            try
            {
                repositorioInquilino.Editar(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(repositorioInquilino.ObtenerPorId<Inquilino>(id));
            }
        }

        // GET: TestController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            return View(repositorioInquilino.ObtenerPorId<Inquilino>(id));
        }

        // POST: TestController/Delete/5
        [HttpPost]
        [Authorize(Policy = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorioInquilino.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                if (ex is SqlException && (ex as SqlException).Number == 547)
                    msg = "No se puede eliminar el Inquilino porque existe un Contrato a nombre suyo.";
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                TempData["Error"] = msg;
                return View(repositorioInquilino.ObtenerPorId<Inquilino>(id));
            }
        }
    }
}
