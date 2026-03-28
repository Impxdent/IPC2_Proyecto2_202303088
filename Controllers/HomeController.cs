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

        [HttpPost]
        public IActionResult Cargar(IFormFile archivo)
        {
            if (archivo != null)
            {
                string ruta = Path.Combine(Directory.GetCurrentDirectory(), "temp.xml");

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                servicio.CargarXML(ruta);

                ViewBag.Mensaje = "Archivo cargado correctamente";
            }
            else
            {
                ViewBag.Mensaje = "Error, debe seleccionar un archivo";
            }

            return View("Index");
        }
    }
}