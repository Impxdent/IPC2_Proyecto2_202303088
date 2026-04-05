using System;
using System.Diagnostics;
using System.IO;
using IPC2_Proyecto2_202303088.Modelos;
using IPC2_Proyecto2_202303088.Estructuras;

namespace IPC2_Proyecto2_202303088.Graphviz
{
    public class Graficador
    {
        private string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");

        public Graficador()
        {
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }
        }

        public string GraficarSistema(SistemaDrones sistema)
        {
            string nombreArchivo = $"sistema_{sistema.Nombre}";
            string rutaDot = Path.Combine(rutaCarpeta, $"{nombreArchivo}.dot");
            string rutaPng = Path.Combine(rutaCarpeta, $"{nombreArchivo}.png");

            using (StreamWriter sw = new StreamWriter(rutaDot))
            {
                sw.WriteLine("digraph G {");
                sw.WriteLine("  node [shape=box, style=filled, color=lightblue];");
                sw.WriteLine($"  S [label=\"Sistema: {sistema.Nombre}\"];");

                Nodo actual = sistema.Drones.ObtenerCabeza();
                int i = 0;
                while (actual != null)
                {
                    Dron d = (Dron)actual.Dato;
                    sw.WriteLine($"  D{i} [label=\"Dron: {d.Nombre}\"];");
                    sw.WriteLine($"  S -> D{i};");
                    actual = actual.Siguiente;
                    i++;
                }
                sw.WriteLine("}");
            }

            GenerarImagen(rutaDot, rutaPng);
            return $"/img/{nombreArchivo}.png";
        }

        public string GraficarInstrucciones(Mensaje mensaje)
        {
            string nombreArchivo = $"mensaje_{mensaje.Nombre}";
            string rutaDot = Path.Combine(rutaCarpeta, $"{nombreArchivo}.dot");
            string rutaPng = Path.Combine(rutaCarpeta, $"{nombreArchivo}.png");

            using (StreamWriter sw = new StreamWriter(rutaDot))
            {
                sw.WriteLine("digraph G {");
                sw.WriteLine("  rankdir=LR;"); 
                sw.WriteLine("  node [shape=box, style=filled, color=lightgreen];");
                sw.WriteLine($"  M [label=\"Mensaje: {mensaje.Nombre}\"];");

                Nodo actual = mensaje.Instrucciones.ObtenerCabeza();
                int i = 0;
                while (actual != null)
                {
                    Instruccion inst = (Instruccion)actual.Dato;
                    sw.WriteLine($"  I{i} [label=\"{inst.Dron}\\nAlt: {inst.Altura}\"];");
                    
                    if (i == 0) sw.WriteLine($"  M -> I{i};");
                    else sw.WriteLine($"  I{i - 1} -> I{i};"); 
                    
                    actual = actual.Siguiente;
                    i++;
                }
                sw.WriteLine("}");
            }

            GenerarImagen(rutaDot, rutaPng);
            return $"/img/{nombreArchivo}.png";
        }

        private void GenerarImagen(string rutaDot, string rutaPng)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaPng}\"",
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
                Console.WriteLine("Error al ejecutar graphviz" + ex.Message);
            }
        }
    }
}