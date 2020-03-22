using System.IO;

namespace EDII_Lab03.Models
{
    public class Factor
    {
        public void GuaradarCompresiones(MiArchivo Archivo, string tipo)
        {
            string ArchivoMapeo = "TusArchivos/" + tipo;
            string archivoLeer = ArchivoMapeo + Path.GetFileName("ListaCompresiones");
            using (var writer = new StreamWriter(archivoLeer, true))
            {
                if (!(Archivo.TamanoArchivoComprimido <= 0 && Archivo.TamanoArchivoDescomprimido <= 0))
                {
                    writer.WriteLine(Archivo.NombreArchivoOriginal + "|" + Archivo.TamanoArchivoDescomprimido + "|" + Archivo.TamanoArchivoComprimido + "|" + Archivo.FactorCompresion + "|" + Archivo.RazonCompresion + "|" + Archivo.PorcentajeReduccion + "|" + Archivo.FormatoCompresion);
                }
            }
        }
    }
}
