using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using EDII_Lab03.Models;
using System;
using System.Linq;
using System.Web;
using System.Net;

namespace EDII_Lab03.LZW
{
    public class CompressLZW
    {
        public int BufferSize { get; private set; }
        const int bufferLength = 100;
        public List<Caracteres> caracteresExistentes = new List<Caracteres>();
        public int totalCaracteres;
        public Dictionary<string, byte> DiccionarioIndices = new Dictionary<string, byte>();
        public string TextoEnBinario = "";
        public string nombreArchivo;

        public void CompresionLZWImportar(FileStream ArchivoImportado)
        {
            Dictionary<string, int> LZWdiccionario = new Dictionary<string, int>();
            Directory.CreateDirectory("~/LZW/Importados/");
            Directory.CreateDirectory("~/LZW/Compresiones/");
            var extension = Path.GetExtension(ArchivoImportado.Name);
            var nombre = Path.GetFileName(ArchivoImportado.Name);
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
                using (FileStream writeStream = new FileStream((@"~/LZW/Compresiones/" + nombreArchivo + ".lzw"), FileMode.OpenOrCreate))
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
                                using (FileStream archivoFinal = new FileStream((@"~/LZW/Compresiones/" + nombreArchivo + ".lzw"), FileMode.OpenOrCreate))
                                {
                                    archivoFinal.WriteByte(Convert.ToByte(Convert.ToInt32(item)));
                                }
                                //Escribir los bytes en el archivo
                            }
                        }
                        PropiedadesArchivoActual.TamanoArchivoComprimido = writeStream.Length;
                        PropiedadesArchivoActual.FactorCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido);
                        PropiedadesArchivoActual.RazonCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido);
                        PropiedadesArchivoActual.PorcentajeReduccion = (Convert.ToDouble(1) - PropiedadesArchivoActual.FactorCompresion).ToString();
                        PropiedadesArchivoActual.FormatoCompresion = ".lzw";
                        GuaradarCompresiones(PropiedadesArchivoActual);
                    }
                }
            }
            var FileVirtualPath = @"~/LZW/Compresiones/" + nombreArchivo + ".lzw";
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
        void GuaradarCompresiones(MiArchivo Archivo)
        {
            string archivoLeer = string.Empty;
            string ArchivoMapeo = "~/LZW/";
            archivoLeer = ArchivoMapeo + Path.GetFileName("ListaCompresiones");
            using (var writer = new StreamWriter(archivoLeer, true))
            {
                if (!(Archivo.TamanoArchivoComprimido <= 0 && Archivo.TamanoArchivoDescomprimido <= 0))
                {
                    writer.WriteLine(Archivo.NombreArchivoOriginal + "|" + Archivo.TamanoArchivoDescomprimido + "|" + Archivo.TamanoArchivoComprimido + "|" + Archivo.FactorCompresion + "|" + Archivo.RazonCompresion + "|" + Archivo.PorcentajeReduccion + "|" + Archivo.FormatoCompresion);
                }
            }
        }
        #endregion //Métodos ajenos a compresiones, solo auxiliares
    }
}
