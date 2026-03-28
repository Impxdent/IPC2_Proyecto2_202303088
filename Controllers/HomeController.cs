using Microsoft.AspNetCore.Mvc;
using IPC2_Proyecto2_202303088.Servicios;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class HomeController : Controller
    {
        public static XmlEntrada servicio = new XmlEntrada();

        public IActionResult Index()
        {
            return View();
        }
    }
}