using Microsoft.AspNetCore.Mvc;
using IPC2_Proyecto2_202303088.Estructuras;
using IPC2_Proyecto2_202303088.Modelos;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class SistemaController : Controller
    {
        public IActionResult Index()
        {
            var servicio = HomeController.servicio;

            return View(servicio.ListaSistemas);
        }
        public IActionResult Detalle(string nombre)
        {
            var servicio = HomeController.servicio;

            Nodo actual = servicio.ListaSistemas.ObtenerCabeza();

            while (actual != null)
            {
                SistemaDrones sistema = (SistemaDrones)actual.Dato;

                if (sistema.Nombre == nombre)
                {
                    return View(sistema);
                }

                actual = actual.Siguiente;
            }

            return Content("Sistema no encontrado");
        }
    }
}