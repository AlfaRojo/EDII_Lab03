using System.Collections.Generic;
using System.IO;
using EDII_Lab03.Models;
using System;
using System.Linq;

namespace EDII_Lab03.LZW
{
    public class CompressLZW
    {
        #region Variables
        public int BufferSize { get; private set; }
        const int bufferLength = 100;
        public List<Caracteres> caracteresExistentes = new List<Caracteres>();
        public int totalCaracteres;
        public Dictionary<string, byte> DiccionarioIndices = new Dictionary<string, byte>();
        public string TextoEnBinario = "";
        public string nombreArchivo;
        #endregion
        public void CompresionLZWExportar(FileStream ArchivoImportado)
        {
            nombreArchivo = Path.GetFileName(ArchivoImportado.Name).Replace("IMPORTADO_","").Split('.')[0];
            var extension = Path.GetExtension(ArchivoImportado.Name);
            var DiccionarioCar = new Dictionary<int, string>();
            using (var Lectura = new BinaryReader(ArchivoImportado))
            {
                var CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                var CantidadCaracteresDiccionatrio = string.Empty;
                while (CaracterDiccionario != '.')
                {
                    CantidadCaracteresDiccionatrio += CaracterDiccionario;
                    CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                }
                var CantidadTexto = string.Empty;
                CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                while (CaracterDiccionario != '.')
                {
                    CantidadTexto += CaracterDiccionario;
                    CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                }
                CaracterDiccionario = Convert.ToChar(Lectura.PeekChar());
                var byteEscrito = Lectura.ReadByte();
                while (DiccionarioCar.Count != Convert.ToInt32(CantidadCaracteresDiccionatrio))
                {
                    if (!DiccionarioCar.ContainsValue(Convert.ToString(Convert.ToChar(byteEscrito))))
                    {
                        DiccionarioCar.Add(DiccionarioCar.Count + 1, Convert.ToString(Convert.ToChar(byteEscrito)));
                    }
                    byteEscrito = Lectura.ReadByte();
                }
                Lectura.ReadByte();
                Lectura.ReadByte();
                CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                var TamanoBits = string.Empty;
                while (CaracterDiccionario != '.')
                {
                    TamanoBits += CaracterDiccionario;
                    CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                }
                CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                while (CaracterDiccionario != '\u0002')
                {
                    extension += CaracterDiccionario;
                    CaracterDiccionario = Convert.ToChar(Lectura.ReadByte());
                }
                extension = "." + extension;
                var byteActual = string.Empty;
                var listaComprimidos = new List<int>();
                Lectura.ReadByte();
                Lectura.ReadByte();
                while (Lectura.BaseStream.Position != Lectura.BaseStream.Length && listaComprimidos.Count < Convert.ToInt32(CantidadTexto))
                {
                    var byteLeido = Convert.ToString(Lectura.ReadByte(), 2);
                    while (byteLeido.Length < 8)
                    {
                        byteLeido = "0" + byteLeido;
                    }
                    byteActual += byteLeido;
                    if (Convert.ToInt32(TamanoBits) > 8)
                    {
                        if (byteActual.Length >= Convert.ToInt32(TamanoBits))
                        {
                            var thisComprimido = string.Empty;
                            for (int i = 0; i < Convert.ToInt32(TamanoBits); i++)
                            {
                                thisComprimido += byteActual[i];
                            }
                            listaComprimidos.Add(Convert.ToInt32(thisComprimido, 2));
                            byteActual = byteActual.Substring(Convert.ToInt32(TamanoBits));
                        }
                    }
                    else
                    {
                        listaComprimidos.Add(Convert.ToInt32(byteActual, 2));
                        byteActual = string.Empty;
                    }
                }
                if (byteActual.Length > 0)
                {
                    listaComprimidos[listaComprimidos.Count - 1] = listaComprimidos[listaComprimidos.Count - 1] + Convert.ToInt32(byteActual, 2);
                }
                var primerCaracter = DiccionarioCar[listaComprimidos[0]];
                listaComprimidos.RemoveAt(0);
                var decompressed = new System.Text.StringBuilder(primerCaracter);
                ArchivoImportado.Close();
                using (FileStream ArchivoNuevo = new FileStream((@"TusArchivos/EXPORTADO_" + nombreArchivo + ".txt"), FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(ArchivoNuevo))
                    {
                        foreach (var item in listaComprimidos)
                        {
                            var analizarCadena = string.Empty;
                            if (DiccionarioCar.ContainsKey(item))
                            {
                                analizarCadena = DiccionarioCar[item];
                            }
                            else if (item == DiccionarioCar.Count + 1)
                            {
                                analizarCadena = primerCaracter + primerCaracter[0];
                            }
                            decompressed.Append(analizarCadena);
                            DiccionarioCar.Add(DiccionarioCar.Count + 1, primerCaracter + analizarCadena[0]);
                            primerCaracter = analizarCadena;
                        }
                        writer.Write(decompressed.ToString());
                    }
                }
            }
        }
        public void CompresionLZWImportar(FileStream ArchivoImportado)
        {
            Dictionary<string, int> LZWdiccionario = new Dictionary<string, int>();
            var extension = Path.GetExtension(ArchivoImportado.Name);
            var nombre = Path.GetFileName(ArchivoImportado.Name).Replace("EXPORTADO_","");
            var PropiedadesArchivoActual = new MiArchivo();
            FileInfo ArchivoAnalizado = new FileInfo(ArchivoImportado.Name);
            PropiedadesArchivoActual.TamanoArchivoDescomprimido = ArchivoAnalizado.Length;
            PropiedadesArchivoActual.NombreArchivoOriginal = ArchivoAnalizado.Name;
            nombreArchivo = ArchivoAnalizado.Name.Split('.')[0];
            var listaCaracteresExistentes = new List<byte>();
            var listaCaracteresBinario = new List<string>();
            var ASCIIescribir = new List<int>();
            using (var Lectura = new BinaryReader(ArchivoImportado))
            {
                using (FileStream writeStream = new FileStream((@"TusArchivos/IMPORTADO_" + nombreArchivo + ".lzw"), FileMode.OpenOrCreate))
                {
                    using (BinaryWriter writer = new BinaryWriter(writeStream))
                    {
                        var byteBuffer = new byte[bufferLength];
                        while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                        {
                            byteBuffer = Lectura.ReadBytes(bufferLength);
                            foreach (var item in byteBuffer)
                            {
                                if (!listaCaracteresExistentes.Contains(item))
                                {
                                    listaCaracteresExistentes.Add(item);
                                }
                            }
                        }
                        listaCaracteresExistentes.Sort();
                        foreach (var item in listaCaracteresExistentes)
                        {
                            var caractreres = Convert.ToChar(item);
                            LZWdiccionario.Add(caractreres.ToString(), LZWdiccionario.Count + 1);
                        }
                        var diccionarioTam = Convert.ToString(LZWdiccionario.LongCount()) + ".";
                        writer.Write(diccionarioTam.ToCharArray());
                        Lectura.BaseStream.Position = 0;
                        var thisCaracter = string.Empty;
                        var myOutput = string.Empty;
                        while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                        {
                            byteBuffer = Lectura.ReadBytes(bufferLength);
                            foreach (byte item in byteBuffer)
                            {
                                var toAnalizar = thisCaracter + Convert.ToChar(item);
                                if (LZWdiccionario.ContainsKey(toAnalizar))
                                {
                                    thisCaracter = toAnalizar;
                                }
                                else
                                {
                                    ASCIIescribir.Add(LZWdiccionario[thisCaracter]);
                                    LZWdiccionario.Add(toAnalizar, LZWdiccionario.Count + 1);
                                    thisCaracter = Convert.ToChar(item).ToString();
                                }
                            }
                        }
                        ASCIIescribir.Add(LZWdiccionario[thisCaracter]);
                        var textotamano = Convert.ToString(LZWdiccionario.LongCount()) + ".";
                        writer.Write(textotamano.ToCharArray());
                        foreach (var item in listaCaracteresExistentes)
                        {
                            var Indice = Convert.ToByte(item);
                            writer.Write(Indice);
                        }
                        writer.Write(Environment.NewLine);
                        var mayorIndice = ASCIIescribir.Max();
                        var bitsIndiceMayor = (Convert.ToString(mayorIndice, 2)).Count();
                        writer.Write(bitsIndiceMayor.ToString().ToCharArray());
                        writer.Write(extension.ToCharArray());
                        writer.Write(Environment.NewLine);
                        if (mayorIndice > 255)
                        {
                            foreach (var item in ASCIIescribir)
                            {
                                var indiceBinario = Convert.ToString(item, 2);
                                while (indiceBinario.Count() < bitsIndiceMayor)
                                {
                                    indiceBinario = "0" + indiceBinario;
                                }
                                listaCaracteresBinario.Add(indiceBinario);
                            }
                            var allBits = string.Empty;
                            foreach (var item in listaCaracteresBinario)
                            {
                                for (int i = 0; i < item.Length; i++)
                                {
                                    if (allBits.Count() < 8)
                                    {
                                        allBits += item[i];
                                    }
                                    else
                                    {
                                        var allDecimal = Convert.ToInt64(allBits, 2);
                                        var allBytes = Convert.ToByte(allDecimal);
                                        writer.Write((allBytes));
                                        allBits = string.Empty;
                                        allBits += item[i];
                                    }
                                }
                            }
                            if (allBits.Length > 0)
                            {
                                var allResultado = Convert.ToInt64(allBits, 2);
                                writer.Write(Convert.ToByte(allResultado));
                            }
                        }
                        else
                        {
                            foreach (var item in ASCIIescribir)
                            {
                                writer.Write(Convert.ToByte(Convert.ToInt32(item)));
                            }
                        }
                        PropiedadesArchivoActual.TamanoArchivoComprimido = writeStream.Length;
                        PropiedadesArchivoActual.FactorCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido);
                        PropiedadesArchivoActual.RazonCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido);
                        PropiedadesArchivoActual.PorcentajeReduccion = (Convert.ToDouble(1) - PropiedadesArchivoActual.FactorCompresion).ToString();
                        PropiedadesArchivoActual.FormatoCompresion = ".lzw";
                        Factor FactorCompresion = new Factor();
                        FactorCompresion.GuaradarCompresiones(PropiedadesArchivoActual, "LZW");
                    }
                }
            }
        }
        #region Utilizables
        string OtroArchivo(string fileName, string sourcePath, string targetPath)
        {
            string sourceFile = Path.Combine(sourcePath, fileName);
            string destFile = Path.Combine(targetPath, fileName);
            {
                Directory.CreateDirectory(targetPath);
            }
            File.Copy(sourceFile, destFile, true);
            return targetPath;
        }
        #endregion //Métodos ajenos a compresiones, solo auxiliares
    }
}
