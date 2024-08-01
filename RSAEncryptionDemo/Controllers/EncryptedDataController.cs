using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace RSAEncryptionDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncryptedDataController : ControllerBase
    {
        private readonly ILogger<EncryptedDataController> _logger;
        private readonly RSAParameters _rsaParameters;

        public EncryptedDataController(ILogger<EncryptedDataController> logger, RSAParameters rsaParameters)
        {
            _logger = logger;
            _rsaParameters = rsaParameters;
        }

        /// <summary>
        /// Returns an encrypted version of the string that is passed in. 
        /// The string will be decrypted using the private key that is in the current RSAParameters.
        /// </summary>
        /// <param name="unencryptedString">string to be encrypted</param>
        /// <returns>encrypted string</returns>
        [HttpGet(nameof(EncryptString), Name = "EncryptString")]
        public string EncryptString(string unencryptedString)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                // initialize the provider with the public key to illustrate how a client will encrypt data
                provider.ImportFromPem(GetPublicRSAKey());

                // convert the string to a byte[], encrypt it, then convert it to a base64 string
                return Convert.ToBase64String(provider.Encrypt(Encoding.Unicode.GetBytes(unencryptedString), true));
            }
        }

        /// <summary>
        /// Decrypts the passed in string.
        /// The string will be decrypted using the private key that is in the current RSAParameters.
        /// If the application has been restarted since encryption occurred, this will fail.
        /// </summary>
        /// <param name="encryptedString">base64 string to be decrypted</param>
        /// <returns>decrypted string</returns>
        [HttpPost(nameof(DecryptString), Name = "DecryptString")]
        public string DecryptString(string encryptedString)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                // initialize the provider with the global RSAParameters (public/private key)
                provider.ImportParameters(_rsaParameters);

                // convert the string from a base64 string, decrypt it, then convert the return byte[] to a string
                return Encoding.Unicode.GetString(provider.Decrypt(Convert.FromBase64String(encryptedString), true));
            }
        }

        /// <summary>
        /// Get the public key from that is in the current RSAParameters.
        /// This can be used by a client to encrypt strings.
        /// </summary>
        /// <returns>The public key PEM encoded</returns>
        [HttpGet(nameof(GetPublicRSAKey), Name = "GetPublicRSAKey")]
        public string GetPublicRSAKey()
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                // initialize the provider with the global RSAParameters (public/private key)
                provider.ImportParameters(_rsaParameters);

                // return the Public Key PEM encoded
                return provider.ExportRSAPublicKeyPem();
            }
        }
    }
}
