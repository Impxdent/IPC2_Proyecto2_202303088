using IPC2_Proyecto2_202303088.Modelos;
namespace IPC2_Proyecto2_202303088.Estructuras
{
    public class ListaDrones : ListaSimple
    {
        public void InsertarDron(Dron dron)
        {
            Insertar(dron);
        }
        public Dron ObtenerPorIndice(int indice)
        {
            Nodo actual=ObtenerCabeza();
            int contador=0;
            while (actual != null)
            {
                if (contador == indice)
                {
                    return(Dron)actual.Dato;
                }
                contador++;
                actual=actual.Siguiente;
            }
            return null;
        }
    }
}