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
        public ActionResult Post([FromForm(Name = "file")] IFormFile File)
        {
            var extensionTipo = Path.GetExtension(File.FileName);
            CompressHuffman HuffmanCompress = new CompressHuffman();
            if (extensionTipo == ".txt")
            {
                using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
                {
                    HuffmanCompress.CompresionHuffmanImportar(thisFile);
                }
            }
            else if (extensionTipo == ".huff")
            {
                using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
                {
                    HuffmanCompress.CompresionHuffmanExportar(thisFile);
                }
            }
            else { return NotFound(); }
            return Ok();
        }
    }
}