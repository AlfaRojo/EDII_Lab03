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
        public List<Caracteres> ListaCaracteresExistentes = new List<Caracteres>();
        public int TotalDeCaracteres;
        public Dictionary<string, byte> DiccionarioIndices = new Dictionary<string, byte>();
        public string TextoEnBinario = "";
        public string nombreArchivo;

        public ActionResult CompresionLZWImportar(FileStream ArchivoImportado)
        {
            Dictionary<string, int> DiccionarioLZWCompresion = new Dictionary<string, int>();
            Directory.CreateDirectory("~/App_Data/ArchivosImportados/");
            Directory.CreateDirectory("~/App_Data/Compresiones/");
            var DireccionArchivo = string.Empty;
            var ArchivoMapeo = "~/App_Data/ArchivosImportados/";
            DireccionArchivo = ArchivoMapeo + Path.GetFileName(ArchivoImportado.Name);
            var extension = Path.GetExtension(ArchivoImportado.Name);
            var PropiedadesArchivoActual = new MiArchivo();
            FileInfo ArchivoAnalizado = new FileInfo(DireccionArchivo);
            PropiedadesArchivoActual.TamanoArchivoDescomprimido = ArchivoAnalizado.Length;
            PropiedadesArchivoActual.NombreArchivoOriginal = ArchivoAnalizado.Name;
            nombreArchivo = ArchivoAnalizado.Name.Split('.')[0];
            var listaCaracteresExistentes = new List<byte>();
            var listaCaracteresEscribir = new List<int>();
            var listaCaracteresBinario = new List<string>();
            using (var Lectura = new BinaryReader(ArchivoImportado))
            {
                using (var writeStream = new FileStream((@"~/App_Data/Compresiones/" + nombreArchivo + ".lzw"), FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(writeStream))
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
                            DiccionarioLZWCompresion.Add(caractreres.ToString(), DiccionarioLZWCompresion.Count + 1);
                        }
                        var TamanoDiccionario = Convert.ToString(DiccionarioLZWCompresion.LongCount()) + ".";
                        writer.Write(TamanoDiccionario.ToCharArray());
                        Lectura.BaseStream.Position = 0;
                        var CaracterActual = string.Empty;
                        var Output = string.Empty;
                        while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                        {
                            byteBuffer = Lectura.ReadBytes(bufferLength);
                            foreach (byte item in byteBuffer)
                            {
                                var CadenaAnalizada = CaracterActual + Convert.ToChar(item);
                                if (DiccionarioLZWCompresion.ContainsKey(CadenaAnalizada))
                                {
                                    CaracterActual = CadenaAnalizada;
                                }
                                else
                                {
                                    listaCaracteresEscribir.Add(DiccionarioLZWCompresion[CaracterActual]);
                                    DiccionarioLZWCompresion.Add(CadenaAnalizada, DiccionarioLZWCompresion.Count + 1);
                                    CaracterActual = Convert.ToChar(item).ToString();
                                }
                            }
                        }
                        listaCaracteresEscribir.Add(DiccionarioLZWCompresion[CaracterActual]);
                        var TamanoTexto = Convert.ToString(DiccionarioLZWCompresion.LongCount()) + ".";
                        writer.Write(TamanoTexto.ToCharArray());
                        foreach (var item in listaCaracteresExistentes)
                        {
                            var Indice = Convert.ToByte(item);
                            writer.Write(Indice);
                        }
                        writer.Write(Environment.NewLine);
                        var mayorIndice = listaCaracteresEscribir.Max();
                        var bitsMayorIndice = (Convert.ToString(mayorIndice, 2)).Count();
                        writer.Write(bitsMayorIndice.ToString().ToCharArray());
                        writer.Write(extension.ToCharArray());
                        writer.Write(Environment.NewLine);
                        if (mayorIndice > 255)
                        {
                            foreach (var item in listaCaracteresEscribir)
                            {
                                var indiceBinario = Convert.ToString(item, 2);
                                while (indiceBinario.Count() < bitsMayorIndice)
                                {
                                    indiceBinario = "0" + indiceBinario;
                                }
                                listaCaracteresBinario.Add(indiceBinario);
                            }
                            var cadenaBits = string.Empty;
                            foreach (var item in listaCaracteresBinario)
                            {
                                for (int i = 0; i < item.Length; i++)
                                {
                                    if (cadenaBits.Count() < 8)
                                    {
                                        cadenaBits += item[i];
                                    }
                                    else
                                    {
                                        var cadenaDecimal = Convert.ToInt64(cadenaBits, 2);
                                        var cadenaEnByte = Convert.ToByte(cadenaDecimal);
                                        writer.Write((cadenaEnByte));
                                        cadenaBits = string.Empty;
                                        cadenaBits += item[i];
                                    }
                                }
                            }
                            if (cadenaBits.Length > 0)
                            {
                                var cadenaRestante = Convert.ToInt64(cadenaBits, 2);
                                writer.Write(Convert.ToByte(cadenaRestante));
                            }
                        }
                        else
                        {
                            foreach (var item in listaCaracteresEscribir)
                            {
                                writer.Write(Convert.ToByte(Convert.ToInt32(item)));
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
            var FileVirtualPath = @"~/App_Data/Compresiones/" + nombreArchivo + ".lzw";
            return new FileStreamResult(FileVirtualPath, "application/x-rar-compressed")//Revisar archivo retornado
            {
                FileDownloadName = "file.lzw"
            };

        }
        #region Utilizables
        void GuaradarCompresiones(MiArchivo Archivo)
        {
            string archivoLeer = string.Empty;
            string ArchivoMapeo = "~/App_Data/";
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
