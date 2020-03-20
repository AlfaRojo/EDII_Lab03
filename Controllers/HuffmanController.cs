using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using EDII_Lab03.Models;
using System.Threading.Tasks;
using EDII_Lab03.ArbolHuff;
using Microsoft.AspNetCore.Http;

namespace EDII_Lab03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HuffmanController : ControllerBase
    {
        [HttpPost]
        public void Post([FromForm(Name = "file")] IFormFile file)
        {
            CompressHuffman HuffmanCompress = new CompressHuffman();
            using (FileStream thisFile = new FileStream(file.FileName, FileMode.OpenOrCreate))
            {
                HuffmanCompress.CompresionHuffmanImportar(thisFile);
            }
        }
    }
}