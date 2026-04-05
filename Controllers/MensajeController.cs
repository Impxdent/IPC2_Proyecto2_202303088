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
                    string mensajeDescifrado = "";
                    SistemaDrones sistemaAsociado = null;

                    Nodo actualSistema = servicio.ListaSistemas.ObtenerCabeza();
                    while (actualSistema != null)
                    {
                        SistemaDrones sis = (SistemaDrones)actualSistema.Dato;
                        if (sis.Nombre == mensaje.Sistema)
                        {
                            sistemaAsociado = sis;
                            break;
                        }
                        actualSistema = actualSistema.Siguiente;
                    }

                    if (sistemaAsociado != null)
                    {
                        Nodo actualInstruccion = mensaje.Instrucciones.ObtenerCabeza();
                        while (actualInstruccion != null)
                        {
                            Instruccion inst = (Instruccion)actualInstruccion.Dato;
                            
                            Nodo actualAltura = sistemaAsociado.Alturas.ObtenerCabeza();
                            
                            while (actualAltura != null)
                            {
                                Altura alt = (Altura)actualAltura.Dato;
                                
                                if (alt.Nivel == inst.Altura) 
                                {
                                    mensajeDescifrado += alt.Letra;
                                    break;
                                }
                                actualAltura = actualAltura.Siguiente;
                            }
                            actualInstruccion = actualInstruccion.Siguiente;
                        }
                    }

                    IPC2_Proyecto2_202303088.Servicios.Simulador simulador = new IPC2_Proyecto2_202303088.Servicios.Simulador();
                    int tiempoOptimo = simulador.CalcularTiempoOptimo(mensaje);

                    IPC2_Proyecto2_202303088.Graphviz.Graficador graficador = new IPC2_Proyecto2_202303088.Graphviz.Graficador();
                    string rutaImagen = graficador.GraficarInstrucciones(mensaje);
                    ViewBag.RutaImagen = rutaImagen;

                    ViewBag.MensajeDescifrado = mensajeDescifrado;
                    ViewBag.TiempoOptimo = tiempoOptimo;

                    return View(mensaje);
                }

                actual = actual.Siguiente;
            }

            return Content("Mensaje no encontrado");
        }
    }
}