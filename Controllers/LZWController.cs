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
        CompressLZW compressLZW = new CompressLZW();

        [HttpPost]
        public void Post([FromForm(Name = "file")] IFormFile File)
        {
            using (FileStream thisFile = new FileStream(File.FileName, FileMode.OpenOrCreate))
            {
                compressLZW.CompresionLZWImportar(thisFile);
            }
        }
    }
}