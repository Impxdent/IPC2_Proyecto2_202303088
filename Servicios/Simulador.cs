using System;
using IPC2_Proyecto2_202303088.Modelos;
using IPC2_Proyecto2_202303088.Estructuras;

namespace IPC2_Proyecto2_202303088.Servicios
{
    public class ResultadoSimulacion
    {
        public int TiempoOptimo { get; set; }
        public string MensajeDescifrado { get; set; }
        public ListaSimple HistorialesDrones { get; set; } 
    }

    public class HistorialDron
    {
        public string NombreDron { get; set; }
        public int AlturaActual { get; set; }
        public int SegundoActual { get; set; }
        public ListaSimple AccionesPorSegundo { get; set; }

        public HistorialDron(string nombre)
        {
            NombreDron = nombre;
            AlturaActual = 0;
            SegundoActual = 0;
            AccionesPorSegundo = new ListaSimple();
        }

        public void AgregarAccion(string accion)
        {
            AccionesPorSegundo.Insertar(accion);
            SegundoActual++;
        }
    }

    public class Simulador
    {
        private class EstadoDron
        {
            public string NombreDron { get; set; }
            public int AlturaActual { get; set; }
            public int TiempoDisponible { get; set; }
            public EstadoDron(string nombre) { NombreDron = nombre; AlturaActual = 0; TiempoDisponible = 0; }
        }

        public int CalcularTiempoOptimo(Mensaje mensaje)
        {
            int tiempoGlobal = 0;
            ListaSimple listaEstados = new ListaSimple(); 
            Nodo nodoActual = mensaje.Instrucciones.ObtenerCabeza(); 

            while (nodoActual != null)
            {
                Instruccion inst = (Instruccion)nodoActual.Dato; 
                EstadoDron estadoActual = ObtenerOCrearEstado(listaEstados, inst.Dron);
                int tiempoViaje = Math.Abs(inst.Altura - estadoActual.AlturaActual);
                int tiempoLlegada = estadoActual.TiempoDisponible + tiempoViaje;
                int inicioEmision = Math.Max(tiempoGlobal, tiempoLlegada);
                int finEmision = inicioEmision + 1;
                estadoActual.AlturaActual = inst.Altura;
                estadoActual.TiempoDisponible = finEmision;
                tiempoGlobal = finEmision; 
                nodoActual = nodoActual.Siguiente; 
            }
            return tiempoGlobal;
        }

        private EstadoDron ObtenerOCrearEstado(ListaSimple listaEstados, string nombreDron)
        {
            Nodo actual = listaEstados.ObtenerCabeza(); 
            while (actual != null)
            {
                EstadoDron estado = (EstadoDron)actual.Dato;
                if (estado.NombreDron == nombreDron) return estado; 
                actual = actual.Siguiente;
            }
            EstadoDron nuevoEstado = new EstadoDron(nombreDron);
            listaEstados.Insertar(nuevoEstado); 
            return nuevoEstado;
        }

        public ResultadoSimulacion SimularCompleto(Mensaje mensaje, SistemaDrones sistemaAsociado)
        {
            ResultadoSimulacion resultado = new ResultadoSimulacion();
            resultado.HistorialesDrones = new ListaSimple();
            resultado.MensajeDescifrado = "";

            Nodo actualDronSis = sistemaAsociado.Drones.ObtenerCabeza();
            while (actualDronSis != null)
            {
                Dron dron = (Dron)actualDronSis.Dato;
                resultado.HistorialesDrones.Insertar(new HistorialDron(dron.Nombre));
                actualDronSis = actualDronSis.Siguiente;
            }

            int tiempoGlobal = 0;
            Nodo nodoInstruccion = mensaje.Instrucciones.ObtenerCabeza();

            while (nodoInstruccion != null)
            {
                Instruccion inst = (Instruccion)nodoInstruccion.Dato;

                Nodo actualAltura = sistemaAsociado.Alturas.ObtenerCabeza();
                while (actualAltura != null)
                {
                    Altura alt = (Altura)actualAltura.Dato;
                    if (alt.Nivel == inst.Altura)
                    {
                        resultado.MensajeDescifrado += alt.Letra;
                        break;
                    }
                    actualAltura = actualAltura.Siguiente;
                }

                HistorialDron historial = ObtenerHistorial(resultado.HistorialesDrones, inst.Dron);

                int tiempoViaje = Math.Abs(inst.Altura - historial.AlturaActual);
                int tiempoLlegada = historial.SegundoActual + tiempoViaje;
                int inicioEmision = Math.Max(tiempoGlobal, tiempoLlegada);

                for (int i = 0; i < tiempoViaje; i++)
                {
                    if (inst.Altura > historial.AlturaActual) historial.AgregarAccion("Subir");
                    else historial.AgregarAccion("Bajar");
                }

                int tiempoEspera = inicioEmision - tiempoLlegada;
                for (int i = 0; i < tiempoEspera; i++)
                {
                    historial.AgregarAccion("Esperar");
                }

                historial.AgregarAccion("Emitir luz");

                historial.AlturaActual = inst.Altura;
                tiempoGlobal = inicioEmision + 1;

                nodoInstruccion = nodoInstruccion.Siguiente;
            }

            resultado.TiempoOptimo = tiempoGlobal;

            Nodo nodoHistorial = resultado.HistorialesDrones.ObtenerCabeza();
            while (nodoHistorial != null)
            {
                HistorialDron hist = (HistorialDron)nodoHistorial.Dato;
                while (hist.SegundoActual < tiempoGlobal)
                {
                    hist.AgregarAccion("Esperar");
                }
                nodoHistorial = nodoHistorial.Siguiente;
            }

            return resultado;
        }

        private HistorialDron ObtenerHistorial(ListaSimple historiales, string nombreDron)
        {
            Nodo actual = historiales.ObtenerCabeza();
            while (actual != null)
            {
                HistorialDron hist = (HistorialDron)actual.Dato;
                if (hist.NombreDron == nombreDron) return hist;
                actual = actual.Siguiente;
            }
            return null;
        }
    }
}