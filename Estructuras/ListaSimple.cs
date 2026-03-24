namespace IPC2_Proyecto2_202303088.Estructuras
{
    public class ListaSimple
    {
        private Nodo cabeza;
        public ListaSimple()
        {
            cabeza=null;
        }
        public void Insertar(object dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (cabeza == null)
            {
                cabeza=nuevo;
                return;
            }
            Nodo actual=cabeza;
            while (actual.Siguiente != null)
            {
                actual=actual.Siguiente;
            }
            actual.Siguiente=nuevo;
        }
        public Nodo ObtenerCabeza()
        {
            return cabeza;
        }
        public int Contar()
        {
            int contador=0;
            Nodo actual = cabeza;
            while (actual != null)
            {
                contador++;
                actual=actual.Siguiente;
            }
            return contador;
        }
    }
}