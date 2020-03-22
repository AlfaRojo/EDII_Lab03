using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EDII_Lab03.Models;
using EDII_Lab03.LZW;

namespace EDII_Lab03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LZWController : ControllerBase
    {

        [HttpPost]
        public ActionResult Post([FromForm(Name = "file")] IFormFile File)
        {
            try
            {
                CompressLZW compressLZW = new CompressLZW();
                var extensionTipo = Path.GetExtension(File.FileName);
                if (extensionTipo == ".txt")
                {
                    using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
                    {
                        compressLZW.CompresionLZWImportar(thisFile);
                    }
                }
                else if (extensionTipo == ".lzw")
                {
                    using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
                    {
                        compressLZW.CompresionLZWExportar(thisFile);
                    }
                }
                else { return NotFound(); }
                return Ok();
            }
            catch (System.NullReferenceException)//No se envia nada
            {
                return NotFound();
            }
        }
    }
}