using IPC2_Proyecto2_202303088.Modelos;
namespace IPC2_Proyecto2_202303088.Estructuras
{
    public class ListaInstrucciones : ListaSimple
    {
        public void InsertarInstruccion(Instruccion instruccion)
        {
            Insertar(instruccion);
        }
        public Instruccion ObtenerPorIndice(int indice)
        {
            Nodo actual = ObtenerCabeza();
            int contador = 0;
            while (actual != null)
            {
                if (contador == indice)
                {
                    return (Instruccion)actual.Dato;
                }
                contador++;
                actual = actual.Siguiente;
            }
            return null;
        }
        public void MostrarInstrucciones()
        {
            Nodo actual = ObtenerCabeza();
            while (actual != null)
            {
                Instruccion inst = (Instruccion)actual.Dato;
                Console.WriteLine("Dron: " + inst.Dron + 
                                  " - Altura: " + inst.Altura);
                actual = actual.Siguiente;
            }
        }
    }
}