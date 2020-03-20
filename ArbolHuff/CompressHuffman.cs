using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using EDII_Lab03.Helpers;
using System.Text;

namespace EDII_Lab03.ArbolHuff
{
    public class CompressHuffman
    {

        public void CompresionHuffmanImportar(FileStream PostFile)
        {
            Directory.CreateDirectory("~/Huffman/Compresiones/");
            Directory.CreateDirectory("~/Huffman/Descompresiones/");
            string nombreArchivo = Path.GetFileNameWithoutExtension(PostFile.Name);
            string extension = Path.GetExtension(PostFile.Name);
            var huffman = new Huffman();
            var fileVirtualPath = "";
            PostFile.Close();
            if (extension == ".huff")
            {
                using (FileStream archivo = new FileStream("~/Huffman/Descompresiones/" + nombreArchivo + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    int contador = 0;
                    int contadorCarac = 0;
                    int CantCaracteres = 0;
                    int CaracteresDif = 0;
                    string texto = string.Empty;
                    string acumula = "";
                    byte auxiliar = 0;
                    int bufferLength = 80;
                    var buffer = new byte[bufferLength];
                    string textoCifrado = string.Empty;
                    using (var file = new FileStream(PostFile.Name, FileMode.Open))
                    {
                        using (var reader = new BinaryReader(file))
                        {
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                buffer = reader.ReadBytes(bufferLength);
                                foreach (var item in buffer)
                                {

                                    if (contador == ((CaracteresDif * 2) + 2) && contadorCarac < CantCaracteres)
                                    {
                                        texto = Convert.ToString(item, 2);
                                        if (texto.Length < 8)
                                        {
                                            texto = texto.PadLeft(8, '0');
                                        }
                                        acumula = acumula + texto;
                                        int cont = 0;
                                        int canteliminar = 0;
                                        string validacion = "";
                                        foreach (var item2 in acumula)
                                        {
                                            validacion = validacion + item2;
                                            cont++;
                                            if (Datos.Instance.DicCarcacteres.ContainsKey(validacion))
                                            {
                                                archivo.WriteByte(Datos.Instance.DicCarcacteres[validacion]);
                                                acumula = acumula.Substring(cont);
                                                cont = 0;
                                                contadorCarac++;
                                                canteliminar = cont;
                                                validacion = "";
                                            }
                                        }
                                    }
                                    if (item != 44)
                                    {
                                        byte[] byteCarac = { item };
                                        texto = texto + Encoding.ASCII.GetString(byteCarac);
                                    }
                                    if (item == 44 && contador > 1 && contador < ((CaracteresDif * 2) + 2))
                                    {
                                        if (item == 44 && contador % 2 == 0)
                                        {
                                            auxiliar = Convert.ToByte(texto, 2);
                                            texto = string.Empty;
                                            contador++;
                                        }
                                        else if (contador % 2 != 0 && item == 44)
                                        {
                                            Datos.Instance.DicCarcacteres.Add(texto, auxiliar);
                                            texto = string.Empty;
                                            contador++;
                                        }
                                    }
                                    else
                                    {
                                        if (item == 44 && contador == 0)
                                        {
                                            CantCaracteres = int.Parse(texto);
                                            texto = string.Empty;
                                            contador++;
                                        }
                                        else if (item == 44 && contador == 1)
                                        {
                                            CaracteresDif = int.Parse(texto);
                                            texto = string.Empty;
                                            contador++;
                                        }
                                    }
                                }
                            }
                            reader.ReadBytes(bufferLength);
                        } 
                    }
                };
                Datos.Instance.DicCarcacteres.Clear();
                fileVirtualPath = @"~/Huffman/Descompresiones/" + nombreArchivo + ".txt";
            }
            else
            {
                int cantidadCaracteres = huffman.Leer(PostFile);
                huffman.CrearArbol();
                byte[] encabezado = huffman.CrearEncabezado(cantidadCaracteres);
                using (FileStream archivo = new FileStream("~/Huffman/Compresiones/" + nombreArchivo + ".huff", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    foreach (var item in encabezado)
                    {
                        archivo.WriteByte(item);
                    }
                    int bufferLength = 80;
                    var buffer = new byte[bufferLength];
                    string textoCifrado = string.Empty;
                    using (var file = new FileStream(PostFile.Name, FileMode.Open))
                    {
                        using (var reader = new BinaryReader(file))
                        {
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                buffer = reader.ReadBytes(bufferLength);
                                foreach (var item in buffer)
                                {
                                    int posiList;
                                    posiList = Datos.Instance.ListaCod.FindIndex(x => x.caracter == item);
                                    textoCifrado = textoCifrado + Datos.Instance.ListaCod.ElementAt(posiList).codigo;
                                    if ((textoCifrado.Length / 8) > 0)
                                    {
                                        string escribirByte = textoCifrado.Substring(0, 8);
                                        byte byteEscribir = Convert.ToByte(escribirByte, 2);
                                        archivo.WriteByte(byteEscribir);
                                        textoCifrado = textoCifrado.Substring(8);
                                    }
                                }
                            }
                            reader.ReadBytes(bufferLength);
                        }
                    }
                    if (textoCifrado.Length > 0 && (textoCifrado.Length % 8) == 0)
                    {
                        byte byteEsc = Convert.ToByte(textoCifrado, 2);
                    }
                    else if (textoCifrado.Length > 0)
                    {
                        textoCifrado = textoCifrado.PadRight(8, '0');
                        byte byteEsc = Convert.ToByte(textoCifrado, 2);
                    }
                };
                fileVirtualPath = @"~/Huffman/Compresiones/" + nombreArchivo + ".huff";
            }
        }
    }
}
