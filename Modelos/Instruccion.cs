namespace IPC2_Proyecto2_202303088.Modelos
{
    public class Instruccion
    {
        public string NombreDron { get; set; }
        public int Altura { get; set; }

        public Instruccion(string dron, int altura)
        {
            NombreDron = dron;
            Altura = altura;
        }
    }
}