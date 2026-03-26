using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proyecto2_202303088.Servicios;

namespace IPC2_Proyecto2_202303088.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile archivo { get; set; }

        public string Mensaje { get; set; }

        public static XmlEntrada servicio = new XmlEntrada();

        public void OnPost()
        {
            if (archivo != null)
            {
                string ruta = Path.Combine(Directory.GetCurrentDirectory(), "temp.xml");

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                servicio.CargarXML(ruta);

                Mensaje = "Archivo cargado correctamente ";
            }
            else
            {
                Mensaje = "Error, debe seleccionar un archivo";
            }
        }
    }
}