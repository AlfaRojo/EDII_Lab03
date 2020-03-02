using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EDII_Lab03.ArbolHuff
{
    public class Arbol
    {
        Nodo raiz;
        public List<CaracterCodigo> ListaCodigos = new List<CaracterCodigo>();
        public Nodo NodoPadre(Nodo der, Nodo izq)
        {
            Nodo Padre = new Nodo(0, (der.probabilidad + izq.probabilidad));
            Padre.derecho = der;
            Padre.izquierdo = izq;
            return Padre;
        }
        public Nodo ConstruirArbol(List<Nodo> ListaProbabilidades)
        {
            List<Nodo> lPrinicipal = ListaProbabilidades;
            List<Nodo> lSecundaria = new List<Nodo>();
            while (lPrinicipal.Count > 2)
            {
                lSecundaria = lPrinicipal;
                Nodo nuevoNodo = NodoPadre(lSecundaria[0], lSecundaria[1]);
                lSecundaria.RemoveRange(0, 2);
                lSecundaria.Add(nuevoNodo);
                lPrinicipal = lSecundaria.OrderBy(o => o.probabilidad).ToList();
            }
            return raiz = NodoPadre(lPrinicipal[0], lPrinicipal[1]);
        }
        public void EtiquetarNodo(Nodo Raiz)
        {
            string Etiquette = Raiz.etiqueta;
            if (Raiz.izquierdo != null)
            {
                Raiz.izquierdo.etiqueta = Etiquette + "0";
                EtiquetarNodo(Raiz.izquierdo);
            }
            if (Raiz.derecho != null)
            {
                //Ó un 1 cuando es hijo derecho
                Raiz.derecho.etiqueta = Etiquette + "1";
                EtiquetarNodo(Raiz.derecho);
            }
            if (Raiz.derecho == null && Raiz.izquierdo == null)
            {
                CaracterCodigo nuevoCaracter = new CaracterCodigo(Raiz.caracter, Raiz.etiqueta);
                ListaCodigos.Add(nuevoCaracter);
            }
        }
    }
    public class Nodo
    {
        public string etiqueta = "";
        public double probabilidad;
        public byte caracter;
        public Nodo izquierdo;
        public Nodo derecho;
        public Nodo()
        {

        }
        public Nodo(byte car, double prob)
        {
            probabilidad = prob;
            caracter = car;
        }
    }
    public class CaracterCodigo
    {
        public string codigo;
        public byte caracter;

        public CaracterCodigo()
        {

        }
        public CaracterCodigo(byte car, string cod)
        {
            caracter = car;
            codigo = cod;
        }
    }
}

