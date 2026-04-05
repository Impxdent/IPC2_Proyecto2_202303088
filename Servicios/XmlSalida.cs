using System.Text;
using IPC2_Proyecto2_202303088.Estructuras;
using IPC2_Proyecto2_202303088.Modelos;

namespace IPC2_Proyecto2_202303088.Servicios
{
    public class XmlSalida
    {
        public string GenerarXml(ListaSimple listaMensajes, ListaSimple listaSistemas)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<respuesta>");
            sb.AppendLine("  <listaMensajes>");

            Simulador simulador = new Simulador();
            Nodo actualMensaje = listaMensajes.ObtenerCabeza();

            while (actualMensaje != null)
            {
                Mensaje msj = (Mensaje)actualMensaje.Dato;
                SistemaDrones sistema = null;
                
                Nodo actualSistema = listaSistemas.ObtenerCabeza();
                while (actualSistema != null)
                {
                    SistemaDrones sis = (SistemaDrones)actualSistema.Dato;
                    if (sis.Nombre == msj.Sistema) { sistema = sis; break; }
                    actualSistema = actualSistema.Siguiente;
                }

                if (sistema != null)
                {
                    ResultadoSimulacion resultado = simulador.SimularCompleto(msj, sistema);

                    sb.AppendLine($"    <mensaje nombre=\"{msj.Nombre}\">");
                    sb.AppendLine($"      <sistemaDrones>{msj.Sistema}</sistemaDrones>");
                    sb.AppendLine($"      <tiempoOptimo>{resultado.TiempoOptimo}</tiempoOptimo>");
                    sb.AppendLine($"      <mensajeRecibido>{resultado.MensajeDescifrado}</mensajeRecibido>");
                    sb.AppendLine("      <instrucciones>");

                    for (int segundo = 1; segundo <= resultado.TiempoOptimo; segundo++)
                    {
                        sb.AppendLine($"        <tiempo valor=\"{segundo}\">");
                        sb.AppendLine("          <acciones>");

                        Nodo actualHistorial = resultado.HistorialesDrones.ObtenerCabeza();
                        while (actualHistorial != null)
                        {
                            HistorialDron hist = (HistorialDron)actualHistorial.Dato;
                            string accionDron = ObtenerAccionEnSegundo(hist.AccionesPorSegundo, segundo - 1);
                            
                            sb.AppendLine($"            <dron nombre=\"{hist.NombreDron}\">{accionDron}</dron>");
                            actualHistorial = actualHistorial.Siguiente;
                        }

                        sb.AppendLine("          </acciones>");
                        sb.AppendLine("        </tiempo>");
                    }

                    sb.AppendLine("      </instrucciones>");
                    sb.AppendLine("    </mensaje>");
                }
                actualMensaje = actualMensaje.Siguiente;
            }

            sb.AppendLine("  </listaMensajes>");
            sb.AppendLine("</respuesta>");

            return sb.ToString();
        }

        private string ObtenerAccionEnSegundo(ListaSimple acciones, int indice)
        {
            Nodo actual = acciones.ObtenerCabeza();
            int contador = 0;
            while (actual != null)
            {
                if (contador == indice) return (string)actual.Dato;
                contador++;
                actual = actual.Siguiente;
            }
            return "Esperar"; 
        }
    }
}