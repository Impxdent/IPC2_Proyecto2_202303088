using Microsoft.AspNetCore.Mvc;
using IPC2_Proyecto2_202303088.Estructuras;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class DronController : Controller
    {
        public IActionResult Index()
        {
            var servicio = HomeController.servicio;

            return View(servicio.ListaDronesGlobal);
        }
    }
}