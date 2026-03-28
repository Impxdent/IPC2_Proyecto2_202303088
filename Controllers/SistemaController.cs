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
    }
}