using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;

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



	}
}
