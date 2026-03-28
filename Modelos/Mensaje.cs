using IPC2_Proyecto2_202303088.Estructuras;

namespace IPC2_Proyecto2_202303088.Modelos
{
    public class Mensaje
    {
        public string Nombre { get; set; }
        public string Sistema { get; set; }

        public ListaInstrucciones Instrucciones { get; set; }

        public Mensaje(string nombre, string sistema)
        {
            Nombre = nombre;
            Sistema = sistema;
            Instrucciones = new ListaInstrucciones();
        }
    }
}