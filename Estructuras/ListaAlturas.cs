using IPC2_Proyecto2_202303088.Modelos;
namespace IPC2_Proyecto2_202303088.Estructuras
{
    public class ListaAlturas : ListaSimple
    {
        public void InsertarAltura(Altura altura)
        {
            Insertar(altura);
        }
        public Altura BuscarPorNivel(int nivel)
        {
            Nodo actual = ObtenerCabeza();
            while (actual != null)
            {
                Altura a = (Altura)actual.Dato;
                if (a.Nivel == nivel)
                {
                    return a;
                }
                actual = actual.Siguiente;
            }
            return null;
        }
    }
}