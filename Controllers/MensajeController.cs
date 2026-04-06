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
                                if (alt.Nivel == inst.Altura && alt.NombreDron == inst.Dron) 
                                {
                                    mensajeDescifrado += alt.Letra;
                                    break;
                                }
                                actualAltura = actualAltura.Siguiente;
                            }
                            actualInstruccion = actualInstruccion.Siguiente;
                        }

                        ViewBag.DronesSistema = sistemaAsociado.Drones;
                        ViewBag.TablaSimulacion = GenerarTablaSimulacion(mensaje, sistemaAsociado);
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

        private ListaSimple GenerarTablaSimulacion(Mensaje m, SistemaDrones s)
        {
            ListaSimple simulacion = new ListaSimple();
            int segundo = 1;
            bool terminado = false;

            while (!terminado)
            {
                terminado = true;
                ListaSimple accionesDelSegundo = new ListaSimple();
                Nodo nDron = s.Drones.ObtenerCabeza();

                while (nDron != null)
                {
                    Dron d = (Dron)nDron.Dato;
                    string accion = CalcularAccionDron(m, d.Nombre, segundo, out bool aunTrabaja);
                    
                    if (aunTrabaja) terminado = false;
                    
                    accionesDelSegundo.Insertar(new { Dron = d.Nombre, Accion = accion });
                    nDron = nDron.Siguiente;
                }

                if (!terminado)
                {
                    simulacion.Insertar(new { Segundo = segundo, Acciones = accionesDelSegundo });
                    segundo++;
                }
                if (segundo > 500) break;
            }
            return simulacion;
        }

        private string CalcularAccionDron(Mensaje m, string nombreDron, int segObjetivo, out bool trabajando)
        {
            int altActual = 0;
            int segCronometro = 0;
            Nodo n = m.Instrucciones.ObtenerCabeza();

            while (n != null)
            {
                Instruccion i = (Instruccion)n.Dato;
                if (i.Dron == nombreDron)
                {
                    while (altActual != i.Altura)
                    {
                        segCronometro++;
                        if (segCronometro == segObjetivo) { trabajando = true; return altActual < i.Altura ? "Subir" : "Bajar"; }
                        if (altActual < i.Altura) altActual++; else altActual--;
                    }
                    segCronometro++;
                    if (segCronometro == segObjetivo) { trabajando = true; return "Emitir Luz"; }
                }
                n = n.Siguiente;
            }
            trabajando = false;
            return "Esperar";
        }
    }
}