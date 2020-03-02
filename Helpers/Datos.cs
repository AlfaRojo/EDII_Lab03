using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDII_Lab03.ArbolHuff;

namespace EDII_Lab03.Helpers
{
    public class Datos
    {
		private static Datos _instance = null;
		public static Datos Instance
		{
			get
			{
				if (_instance == null) _instance = new Datos();
				{
					return _instance;
				}
			}
		}
		public List<CaracterCodigo> ListaCod = new List<CaracterCodigo>();
		public Dictionary<string, byte> DicCarcacteres = new Dictionary<string, byte>();
	}
}
