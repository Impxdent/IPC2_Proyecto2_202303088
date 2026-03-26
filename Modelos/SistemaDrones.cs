using IPC2_Proyecto2_202303088.Estructuras;

namespace IPC2_Proyecto2_202303088.Modelos
{
    public class SistemaDrones
    {
        public string Nombre { get; set; }
        public int AlturaMaxima { get; set; }

        public ListaDrones Drones { get; set; }
        public ListaAlturas Alturas { get; set; }

        public SistemaDrones(string nombre, int alturaMaxima)
        {
            Nombre = nombre;
            AlturaMaxima = alturaMaxima;
            Drones = new ListaDrones();
            Alturas = new ListaAlturas();
        }
    }
}