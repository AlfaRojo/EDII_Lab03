using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_Lab03.Models
{
    public class MiArchivo
    {
        public string NombreArchivoOriginal { get; set; }
        public double TamanoArchivoDescomprimido { get; set; }
        public double TamanoArchivoComprimido { get; set; }
        public double RazonCompresion { get; set; }
        public double FactorCompresion { get; set; }
        public string PorcentajeReduccion { get; set; }
        public string FormatoCompresion { get; set; }
    }
}
