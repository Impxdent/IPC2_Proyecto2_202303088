namespace IPC2_Proyecto2_202303088.Estructuras
{
    public class Nodo
    {
        public object Dato;
        public Nodo Siguiente;
        public Nodo(object dato)
        {
            Dato=dato;
            Siguiente=null;
        }
    }
}