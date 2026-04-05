using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
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

        public IActionResult CargarArchivo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcesarArchivo()
        {
            var archivoXml = Request.Form.Files.FirstOrDefault();

            if (archivoXml != null && archivoXml.Length > 0)
            {
                string rutaTemporal = Path.GetTempFileName();
                using (var stream = new FileStream(rutaTemporal, FileMode.Create))
                {
                    archivoXml.CopyTo(stream);
                }
                
                servicio.CargarXML(rutaTemporal); 
                ViewBag.Mensaje = "¡Archivo cargado correctamente!";
            }
            else
            {
                ViewBag.Mensaje = "Error: Seleccione un archivo.";
            }

            return View("CargarArchivo"); 
        }

        [HttpGet]
        public IActionResult DescargarSalidaXml()
        {
            if (servicio.ListaMensajes == null || servicio.ListaMensajes.Contar() == 0)
            {
                ViewBag.Mensaje = "No hay datos. Carga un XML primero.";
                return View("CargarArchivo");
            }

            XmlSalida generador = new XmlSalida();
            string contenidoXml = generador.GenerarXml(servicio.ListaMensajes, servicio.ListaSistemas);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contenidoXml);
            return File(bytes, "application/xml", "salida.xml");
        }

        public IActionResult Ayuda()
        {
            return View();
        }
    }
}