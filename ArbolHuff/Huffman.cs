using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using EDII_Lab03.Models;
using EDII_Lab03.Helpers;

namespace EDII_Lab03.ArbolHuff
{
	public class Huffman
	{
		List<Nodo> frecuencias = new List<Nodo>();
		//

		public int Leer(string direccion)
		{
			var noCaracteres = 0;
			// metodo para leer archivo, que incremente el contador y llame al metodo "ConteoDeFrecuencia"
			return noCaracteres;
		}

		public void ConteoDeFrecuencia(byte elemento)
		{
			int posicionLista;
			if (frecuencias.Exists(x => x.caracter == elemento))
			{
				posicionLista = frecuencias.FindIndex(x => x.caracter == elemento);

				Nodo prueba = new Nodo();
				prueba = frecuencias.Find(x => x.caracter == elemento);
				frecuencias.RemoveAt(posicionLista);
				frecuencias.Add(new Nodo()
				{
					caracter = elemento,
					probabilidad = prueba.probabilidad + 1
				});
			}
			else
			{
				frecuencias.Add(new Nodo()
				{
					caracter = elemento,
					probabilidad = 1
				});
			}
		}

		public void CrearArbol()
		{
			List<Nodo> frecuenciasORDEN = new List<Nodo>();
			frecuenciasORDEN = frecuencias.OrderBy(x => x.probabilidad).ToList();
			Arbol MiArbol = new Arbol();
			MiArbol.EtiquetarNodo(MiArbol.ConstruirArbol(frecuenciasORDEN));
			Datos.Instance.ListaCod = MiArbol.ListaCodigos;
		}

		public byte[] CrearEncabezado(int noCaracteres)
		{
			double noElementos = Datos.Instance.ListaCod.LongCount();
			string codigo = noCaracteres + "," + noElementos;
			foreach (var cosa in Datos.Instance.ListaCod)
			{
				string p = Convert.ToString(cosa.caracter, 2);
				codigo = codigo + "," + p;
				codigo = codigo + "," + cosa.codigo;
			}
			byte[] encabezadoBytes = Encoding.ASCII.GetBytes(codigo + ",");
			return encabezadoBytes;
		}

	}
}
