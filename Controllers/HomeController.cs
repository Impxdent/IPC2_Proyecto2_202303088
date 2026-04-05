using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using IPC2_Proyecto2_202303088.Servicios;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class HomeController : Controller
    {
        // Tu variable global original
        public static XmlEntrada servicio = new XmlEntrada();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cargar()
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
                
                ViewBag.Mensaje = "El archivo fue cargado exitosamente";
            }
            else
            {
                ViewBag.Mensaje = "Error, seleccione un archivo valido";
            }

            return View("Index");
        }

        [HttpGet]
        public IActionResult DescargarSalidaXml()
        {
            if (servicio.ListaMensajes == null || servicio.ListaMensajes.Contar() == 0)
            {
                ViewBag.Mensaje = "No hay datos para generar el archivo. Carga un XML primero.";
                return View("Index");
            }

            // Llamamos al generador
            XmlSalida generador = new XmlSalida();
            string contenidoXml = generador.GenerarXml(servicio.ListaMensajes, servicio.ListaSistemas);

            // Devolvemos el archivo para descargar
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contenidoXml);
            return File(bytes, "application/xml", "salida.xml");
        }
    }
}