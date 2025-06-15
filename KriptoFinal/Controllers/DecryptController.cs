using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptoFinal.Controllers
{
    public class DecryptController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string privateKeyBase64, string encryptedTextBase64)
        {
            if (string.IsNullOrWhiteSpace(privateKeyBase64))
            {
                ModelState.AddModelError("", "Private Key Giriniz!");
            }

            if (string.IsNullOrWhiteSpace(encryptedTextBase64))
            {
                ModelState.AddModelError("", "Şifrelenmiş Metni Giriniz!");
            }

            string decryptedText = null;

            if (ModelState.IsValid)
            {
                try
                {
                    byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedTextBase64);

                    using var rsa = RSA.Create();
                    rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

                    var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                    decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                }
                catch
                {
                    ModelState.AddModelError("", "Şifre Çözme Başarısız!");
                }
            }

            ViewBag.PrivateKey = privateKeyBase64;
            ViewBag.EncryptedText = encryptedTextBase64;
            ViewBag.DecryptedText = decryptedText;

            return View();
        }
    }
}
