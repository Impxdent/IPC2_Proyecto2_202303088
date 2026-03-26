using System.Xml;
using IPC2_Proyecto2_202303088.Modelos;
using IPC2_Proyecto2_202303088.Estructuras;

namespace IPC2_Proyecto2_202303088.Servicios
{
    public class XmlEntrada
    {
        public ListaDrones ListaDronesGlobal;
        public ListaSimple ListaSistemas;
        public ListaSimple ListaMensajes;

        public XmlEntrada()
        {
            ListaDronesGlobal = new ListaDrones();
            ListaSistemas = new ListaSimple();
            ListaMensajes = new ListaSimple();
        }

        public void CargarXML(string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode root = doc.SelectSingleNode("config");

            if (root != null)
            {
                LeerDrones(root.SelectSingleNode("listaDrones"));
                LeerSistemas(root.SelectSingleNode("listaSistemasDrones"));
                LeerMensajes(root.SelectSingleNode("listaMensajes"));
            }
        }

        private void LeerDrones(XmlNode nodoDrones)
        {
            if (nodoDrones == null) return;

            foreach (XmlNode dronNode in nodoDrones.SelectNodes("dron"))
            {
                string nombre = dronNode.InnerText.Trim();
                Dron dron = new Dron(nombre);

                ListaDronesGlobal.InsertarDron(dron);
            }
        }

        private void LeerSistemas(XmlNode nodoSistemas)
        {
            if (nodoSistemas == null) return;

            foreach (XmlNode sistemaNode in nodoSistemas.SelectNodes("sistemaDrones"))
            {
                if (sistemaNode.Attributes["nombre"] == null || sistemaNode["alturaMaxima"] == null)
                    continue;

                string nombre = sistemaNode.Attributes["nombre"].Value;
                int alturaMax = int.Parse(sistemaNode["alturaMaxima"].InnerText);

                SistemaDrones sistema = new SistemaDrones(nombre, alturaMax);

                // Leer drones del sistema
                foreach (XmlNode dronNode in sistemaNode.SelectNodes("contenido/dron"))
                {
                    string nombreDron = dronNode.InnerText.Trim();
                    sistema.Drones.InsertarDron(new Dron(nombreDron));
                }

                // Leer alturas
                foreach (XmlNode alturaNode in sistemaNode.SelectNodes("contenido/alturas/altura"))
                {
                    if (alturaNode.Attributes["valor"] == null) continue;

                    int nivel = int.Parse(alturaNode.Attributes["valor"].Value);
                    char letra = alturaNode.InnerText.Trim()[0];

                    sistema.Alturas.InsertarAltura(new Altura(nivel, letra));
                }

                ListaSistemas.Insertar(sistema);
            }
        }

        private void LeerMensajes(XmlNode nodoMensajes)
        {
            if (nodoMensajes == null) return;

            foreach (XmlNode mensajeNode in nodoMensajes.SelectNodes("Mensaje"))
            {
                if (mensajeNode.Attributes["nombre"] == null || mensajeNode["sistemaDrones"] == null)
                    continue;

                string nombre = mensajeNode.Attributes["nombre"].Value;
                string sistema = mensajeNode["sistemaDrones"].InnerText;

                Mensaje mensaje = new Mensaje(nombre, sistema);

                foreach (XmlNode instNode in mensajeNode.SelectNodes("instrucciones/instruccion"))
                {
                    if (instNode.Attributes["dron"] == null) continue;

                    string dron = instNode.Attributes["dron"].Value;
                    int altura = int.Parse(instNode.InnerText);

                    mensaje.Instrucciones.InsertarInstruccion(
                        new Instruccion(dron, altura)
                    );
                }

                ListaMensajes.Insertar(mensaje);
            }
        }
    }
}