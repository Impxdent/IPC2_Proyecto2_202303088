namespace IPC2_Proyecto2_202303088.Modelos
{
    public class Altura
    {
        public int Nivel { get; set; }
        public char Letra { get; set; }
        public string NombreDron { get; set; }

        public Altura(int nivel, char letra, string nombreDron)
        {
            Nivel = nivel;
            Letra = letra;
            NombreDron = nombreDron;
        }
    }
}