using InmobiliariaOrtega.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaOrtega.Controllers
{
    public class PropietarioController : Controller
    {
        RepositorioPropietario repositorio;

      public PropietarioController()
        { 
            repositorio = new RepositorioPropietario();
        }

        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                int res = repositorio.Alta(p);
                if(res> 0)
                return RedirectToAction(nameof(Index));
                else 
                    return View();

            }
            catch (Exception ex) 
            {
                return View();
            }
        }

        
        public ActionResult Edit(int id)
        {
            return View();
        }

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

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PropietarioController/Delete/5
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
