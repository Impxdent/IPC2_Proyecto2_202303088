namespace IPC2_Proyecto2_202303088.Modelos
{
    public class Instruccion
    {
        public string Dron { get; set; }
        public int Altura { get; set; }

        public Instruccion(string dron, int altura)
        {
            Dron = dron;
            Altura = altura;
        }
    }
}