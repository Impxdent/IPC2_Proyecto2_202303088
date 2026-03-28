using Microsoft.AspNetCore.Mvc;
using IPC2_Proyecto2_202303088.Estructuras;
using IPC2_Proyecto2_202303088.Modelos;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class MensajeController : Controller
    {
        public IActionResult Index()
        {
            var servicio = HomeController.servicio;

            return View(servicio.ListaMensajes);
        }

        public IActionResult Detalle(string nombre)
        {
            var servicio = HomeController.servicio;

            Nodo actual = servicio.ListaMensajes.ObtenerCabeza();

            while (actual != null)
            {
                Mensaje mensaje = (Mensaje)actual.Dato;

                if (mensaje.Nombre == nombre)
                {
                    return View(mensaje);
                }

                actual = actual.Siguiente;
            }

            return Content("Mensaje no encontrado");
        }
    }
}