using Microsoft.AspNetCore.Mvc;
using IPC2_Proyecto2_202303088.Estructuras;
using System.Diagnostics;
using System.IO;

namespace IPC2_Proyecto2_202303088.Controllers
{
    public class DronController : Controller
    {
        public IActionResult Index()
        {
            var servicio = HomeController.servicio;
            var listaDrones = servicio.ListaDronesGlobal;

            if (listaDrones != null && listaDrones.ObtenerCabeza() != null)
            {
                GenerarGraficaDrones(listaDrones);
                ViewBag.RutaGrafica = "/img/grafica_drones.png";
            }

            return View(listaDrones);
        }

        private void GenerarGraficaDrones(ListaDrones lista)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string dotPath = Path.Combine(folderPath, "grafica_drones.dot");
            string imgPath = Path.Combine(folderPath, "grafica_drones.png");
            string dotContent = "digraph G {\n";
            dotContent += "  node [shape=box, style=filled, fillcolor=lightskyblue, fontname=\"Arial\"];\n";
            dotContent += "  label=\"Estructura de Drones del Sistema\";\n";
            dotContent += "  labelloc=\"t\";\n";

            var actual = lista.ObtenerCabeza();
            int contador = 1;

            while (actual != null)
            {
                var dron = (IPC2_Proyecto2_202303088.Modelos.Dron)actual.Dato;
                dotContent += $"  dron{contador} [label=\"{contador}. {dron.Nombre}\"];\n";

                if (actual.Siguiente != null)
                {
                    dotContent += $"  dron{contador} -> dron{contador + 1};\n";
                }

                actual = actual.Siguiente;
                contador++;
            }

            dotContent += "}";
            System.IO.File.WriteAllText(dotPath, dotContent);

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("dot")
                {
                    Arguments = $"-Tpng \"{dotPath}\" -o \"{imgPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al generar Graphviz: " + ex.Message);
            }
        }
    }
}