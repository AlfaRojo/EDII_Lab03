using System.IO;
using Microsoft.AspNetCore.Mvc;
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
            CompressHuffman HuffmanCompress = new CompressHuffman();
            using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
            {
                HuffmanCompress.CompresionHuffmanImportar(thisFile);
            }
        }
    }
}