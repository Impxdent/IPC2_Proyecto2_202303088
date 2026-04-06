using Microsoft.AspNetCore.Mvc;
using IPC2_Proyecto2_202303088.Estructuras;
using IPC2_Proyecto2_202303088.Modelos;
using System.Diagnostics;
using System.IO;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class SistemaController : Controller
    {
        public IActionResult Index()
        {
            var servicio = HomeController.servicio;
            ListaSimple listaDeSistemas = servicio.ListaSistemas; 

            var actual = listaDeSistemas.ObtenerCabeza();
            while (actual != null)
            {
                SistemaDrones sistema = (SistemaDrones)actual.Dato;
                GenerarGraficaSistema(sistema);
                actual = actual.Siguiente;
            }

            return View(listaDeSistemas);
        }

        private void GenerarGraficaSistema(SistemaDrones sistema)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string dotPath = Path.Combine(folderPath, $"sistema_{sistema.Nombre}.dot");
            string imgPath = Path.Combine(folderPath, $"sistema_{sistema.Nombre}.png");

            string dotContent = "digraph G {\n";
            dotContent += "  node [shape=none, fontname=\"Arial\"];\n";
            dotContent += "  tabla [label=<<table border='1' cellborder='1' cellspacing='0' cellpadding='4'>\n";
            dotContent += $"    <tr><td colspan='{ContarDrones(sistema) + 1}'><b>Sistema {sistema.Nombre}</b></td></tr>\n";
            dotContent += "    <tr><td><b>Altura</b></td>";

            var hDron = sistema.Drones.ObtenerCabeza();
            while (hDron != null) {
                var d = (Dron)hDron.Dato;
                dotContent += $"<td><b>{d.Nombre}</b></td>";
                hDron = hDron.Siguiente;
            }
            dotContent += "</tr>\n";

            for (int z = 1; z <= sistema.AlturaMaxima; z++) {
                dotContent += $"    <tr><td>{z}</td>";
                var dCol = sistema.Drones.ObtenerCabeza();
                while (dCol != null) {
                    var dron = (Dron)dCol.Dato;
                    string letraMostrada = "-"; 
                    var nAlt = sistema.Alturas.ObtenerCabeza();
                    while (nAlt != null) {
                        var da = (IPC2_Proyecto2_202303088.Modelos.Altura)nAlt.Dato;
                        if (da.Nivel == z && da.NombreDron == dron.Nombre) { 
                            letraMostrada = da.Letra.ToString();
                            break;
                        } 
                        nAlt = nAlt.Siguiente;
                    }
                    dotContent += $"<td>{letraMostrada}</td>";
                    dCol = dCol.Siguiente;
                }
                dotContent += "</tr>\n";
            }
            dotContent += "  </table>>];\n}";

            System.IO.File.WriteAllText(dotPath, dotContent);
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo("dot") {
                    Arguments = $"-Tpng \"{dotPath}\" -o \"{imgPath}\"",
                    UseShellExecute = false, CreateNoWindow = true
                };
                using (Process process = Process.Start(startInfo)) { process.WaitForExit(); }
            } catch { }
        }

        private int ContarDrones(SistemaDrones s) {
            int i = 0;
            var a = s.Drones.ObtenerCabeza();
            while(a != null){ i++; a = a.Siguiente; }
            return i;
        }
    }
}