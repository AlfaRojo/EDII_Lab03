using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using EDII_Lab03.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EDII_Lab03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HuffmanController : ControllerBase
    {
        const int bufferLength = 100;
        public List<Caracteres> ListaCaracteresExistentes = new List<Caracteres>();
        public int totalCaracteres;
        public Dictionary<string, byte> diccionarioIndices = new Dictionary<string, byte>();
        public string txtBin = "";
        public string nombreArchivo;

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }
    }
}