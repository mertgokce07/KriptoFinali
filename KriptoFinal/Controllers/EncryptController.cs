using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptoFinal.Controllers
{
    public class EncryptController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string publicKeyBase64, string plainText)
        {
            if (string.IsNullOrWhiteSpace(publicKeyBase64))
            {
                ModelState.AddModelError("", "Public Key Giriniz.");
            }

            if (string.IsNullOrWhiteSpace(plainText))
            {
                ModelState.AddModelError("", "Şifrelenecek Metni Giriniz.");
            }

            string encryptedText = null;

            if (ModelState.IsValid)
            {
                try
                {
                    byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);

                    using var rsa = RSA.Create();
                    rsa.ImportRSAPublicKey(publicKeyBytes, out _);

                    var data = Encoding.UTF8.GetBytes(plainText);
                    var encryptedBytes = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);

                    encryptedText = Convert.ToBase64String(encryptedBytes);
                }
                catch
                {
                    ModelState.AddModelError("", "Şifreleme Başarısız!");
                }
            }

            ViewBag.PublicKey = publicKeyBase64;
            ViewBag.PlainText = plainText;
            ViewBag.EncryptedText = encryptedText;

            return View();
        }
    }
}
