using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace KriptoFinal.Controllers
{
    public class SHA256Controller : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string inputType, string? inputText, IFormFile? inputFile)
        {
            string hashResult = null;

            if (inputType == "text")
            {
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    ModelState.AddModelError("", "Metin Giriniz.");
                }
            }
            else if (inputType == "file")
            {
                if (inputFile == null || inputFile.Length == 0)
                {
                    ModelState.AddModelError("inputFile", "Bir Dosya Seçiniz.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Giriş Türünü Seçiniz.");
            }

            if (ModelState.IsValid)
            {
                if (inputType == "text" && !string.IsNullOrWhiteSpace(inputText))
                {
                    hashResult = ComputeSHA256Hash(Encoding.UTF8.GetBytes(inputText));
                    ViewBag.InputText = inputText;
                }
                else if (inputType == "file" && inputFile != null && inputFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await inputFile.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    hashResult = ComputeSHA256Hash(fileBytes);
                }
            }

            ViewBag.HashResult = hashResult;
            return View();
        }


        private string ComputeSHA256Hash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(data);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
