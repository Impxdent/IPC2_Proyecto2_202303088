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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar XML: " + ex.Message);
            }
        }

        private void LeerDrones(XmlNode nodoDrones)
        {
            if (nodoDrones == null) return;

            foreach (XmlNode dronNode in nodoDrones.SelectNodes("dron"))
            {
                string nombre = dronNode.InnerText.Trim();
                ListaDronesGlobal.InsertarDron(new Dron(nombre));
            }
        }

        private void LeerSistemas(XmlNode nodoSistemas)
        {
            if (nodoSistemas == null) return;

            foreach (XmlNode sistemaNode in nodoSistemas.SelectNodes("sistemaDrones"))
            {
                string nombreSistema = sistemaNode.Attributes["nombre"]?.Value;
                int alturaMax = int.Parse(sistemaNode.SelectSingleNode("alturaMaxima")?.InnerText ?? "0");
                SistemaDrones nuevoSistema = new SistemaDrones(nombreSistema, alturaMax);
                foreach (XmlNode contenidoNode in sistemaNode.SelectNodes("contenido"))
                {
                    string nombreDron = contenidoNode.SelectSingleNode("dron")?.InnerText.Trim();
                    
                    if (!string.IsNullOrEmpty(nombreDron))
                    {
                        nuevoSistema.Drones.InsertarDron(new Dron(nombreDron));
                        XmlNodeList listaAlturas = contenidoNode.SelectNodes("alturas/altura");
                        foreach (XmlNode alturaNode in listaAlturas)
                        {
                            int valor = int.Parse(alturaNode.Attributes["valor"].Value);
                            char letra = alturaNode.InnerText.Trim()[0];
                            Altura nuevaAltura = new Altura(valor, letra, nombreDron);
                            nuevoSistema.Alturas.InsertarAltura(nuevaAltura);
                        }
                    }
                }

                ListaSistemas.Insertar(nuevoSistema);
            }
        }

        private void LeerMensajes(XmlNode nodoMensajes)
        {
            if (nodoMensajes == null) return;

            foreach (XmlNode mensajeNode in nodoMensajes.SelectNodes("Mensaje"))
            {
                string nombre = mensajeNode.Attributes["nombre"]?.Value;
                string sistemaRef = mensajeNode.SelectSingleNode("sistemaDrones")?.InnerText;

                Mensaje mensaje = new Mensaje(nombre, sistemaRef);

                foreach (XmlNode instNode in mensajeNode.SelectNodes("instrucciones/instruccion"))
                {
                    string dronRef = instNode.Attributes["dron"]?.Value;
                    int nivelRef = int.Parse(instNode.InnerText);

                    mensaje.Instrucciones.InsertarInstruccion(new Instruccion(dronRef, nivelRef));
                }

                ListaMensajes.Insertar(mensaje);
            }
        }
    }
}