using System;

namespace EDII_Lab03.Models
{
    public class Caracteres : IComparable
    {
        public byte CaracterTexto { get; set; }
        public int Frecuencia { get; set; }
        public int indice { get; set; }
        public bool CaracterYaRecorrido = false;
        public bool CaracterAUsar = false;
        public string binarioText { get; set; }
        public int CompareTo(object obj)
        {
            var vComparador = (Caracteres)obj;
            return CaracterTexto.CompareTo(vComparador.CaracterTexto);
        }
    }
}
