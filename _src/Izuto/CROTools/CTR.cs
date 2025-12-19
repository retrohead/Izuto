using plugin_nintendo.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Izuto.CROTools.KeySet;

namespace Izuto.CROTools
{
    public static class CTR
    {
        public static void ctr_rsa_init_key_pubmodulus(ref RSAKey2048 key, byte[] modulus)
        {
            byte[] exponent = { 0x01, 0x00, 0x01 };

            ctr_rsa_init_key_pub(ref key, modulus, exponent);
        }
        public static void ctr_rsa_init_key_pub(ref RSAKey2048 key, byte[] modulus, byte[] exponent)
        {
            key.keytype = RSAKeyType.RSAKEY_PUB;
            Buffer.BlockCopy(modulus, 0, key.n, 0, 0x100);
            Buffer.BlockCopy(exponent, 0, key.e, 0, 3);
        }

        public static ActionResult ctr_rsa_verify_hash(byte[] signature, byte[] hash, RSAKey2048 key)
        {
            if (key.keytype == RSAKeyType.RSAKEY_INVALID)
                return new ActionResult(false, "RSA Key is invalid");

            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = key.n,
                Exponent = key.e
            });
            bool ok = rsa.VerifyHash(
                    hash,
                    signature,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );
            if(!ok)
                return new ActionResult(false, "RSA signature verification failed.");

            return new ActionResult(true, "RSA signature verification succeeded.");
        }
    }
}
