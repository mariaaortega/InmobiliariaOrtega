using InmobiliariaOrtega.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaOrtega.Controllers
{
    public class InquilinoController : Controller
    {
        RepositorioInquilino repositorio;

        public InquilinoController()
        {
            repositorio = new RepositorioInquilino();
        }
        // GET: InquilinoController
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                int res = repositorio.Alta(i);
                if (res > 0)
                    return RedirectToAction(nameof(Index));
                else
                    return View();

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
