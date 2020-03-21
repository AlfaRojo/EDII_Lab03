using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_Lab03.Models
{
    public class Factor
    {
        public void GuaradarCompresiones(MiArchivo Archivo, string tipo)
        {
            string archivoLeer = string.Empty;
            string ArchivoMapeo = "TusArchivos/" + tipo;
            archivoLeer = ArchivoMapeo + Path.GetFileName("ListaCompresiones");
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
