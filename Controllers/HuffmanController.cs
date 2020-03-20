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
        public void Post([FromForm(Name = "file")] IFormFile File)
        {
            Huffman HuffmanCompress = new Huffman();
            using (FileStream thisFile = new FileStream(File.FileName, FileMode.OpenOrCreate))
            {
                var direccion = Path.GetFullPath(thisFile.Name);
                HuffmanCompress.Leer(direccion);
            }
        }
    }
}