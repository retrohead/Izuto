using CommunityToolkit.HighPerformance;
using Izuto.Extensions.BinaryTools;
using System.IO;
using System.Security.Cryptography;
using static Izuto.CROTools.KeySet;

namespace Izuto.CROTools
{
    public static class CROTools
    {
        public static List<ActionResult> CROResults = new List<ActionResult>();

        public static ActionResult validate_crr(string crr_filename, string crodir, out byte[]? hashtable_hash)
        {
            CROResults = new List<ActionResult>();
            hashtable_hash = null;
            var nin_rsa_mod = Keys.GetKey(Keys.KeyType.NinRsaModulus);
            if (nin_rsa_mod == null)
                return new ActionResult(false, "RSA keys not loaded.");

            if(!File.Exists(crr_filename))
                return new ActionResult(false, "File does not exist.");

            byte[] crr;
            RSAKey2048 key = new RSAKey2048();
            int fSize;
            int signedAreaSize;
            int numCros;

            try
            {
                using (var fs = File.OpenRead(crr_filename))
                using (var br = new BinaryReader(fs))
                {
                    fSize = (int)fs.Length;
                    crr = br.ReadBytes(fSize);
                }

                // verify nintendo signature
                CTR.ctr_rsa_init_key_pubmodulus(ref key, nin_rsa_mod);

                // hash: SHA-256 over crr[0x20..0x20+0x120)
                var nintendoHashInput = new byte[0x120];
                Buffer.BlockCopy(crr, 0x20, nintendoHashInput, 0, 0x120);
                var hash = Sha256(nintendoHashInput);

                // signature at crr[0x140..0x140+0x100)
                var nintendoSignature = crr.AsSpan(0x140, 0x100).ToArray();
                var nintendoOk = CTR.ctr_rsa_verify_hash(nintendoSignature, hash, key);

                if (nintendoOk.Success)
                    CROResults.Add(new ActionResult(true, "Nintendo CRR RSA signature valid!"));
                else
                    CROResults.Add(new ActionResult(false, "Nintendo CRR RSA signature invalid! (RSA patch needed)"));

                // verify local signature
                // modulus at crr + 0x40 (0x100 bytes)
                var localModulus = crr.AsSpan(0x40, 0x100).ToArray();
                CTR.ctr_rsa_init_key_pubmodulus(ref key, localModulus);

                signedAreaSize = GetLe32(crr, 0x358) - 0x340;


                var localHashInput = new byte[signedAreaSize];
                Buffer.BlockCopy(crr, 0x340, localHashInput, 0, signedAreaSize);
                hash = Sha256(localHashInput);

                // local signature at crr + 0x240
                var localSignature = crr.AsSpan(0x240, 0x100).ToArray();
                var localOk = CTR.ctr_rsa_verify_hash(localSignature, hash, key);

                if (localOk.Success)
                    CROResults.Add(new ActionResult(true, "Local CRR RSA signature valid!"));
                else
                    CROResults.Add(new ActionResult(false, "Local CRR RSA signature invalid!"));


                numCros = GetLe32(crr, 0x354);

                // collect .cro filenames from current directory
                var croFiles = Directory
                    .EnumerateFiles(crodir, "*.cro", SearchOption.TopDirectoryOnly)
                    .Select(Path.GetFileName)
                    .Where(name => name != null)
                    .ToList()!;

                if (croFiles.Count > numCros)
                    return new ActionResult(false, $"Invalid number of CROs! Expected {numCros}, got {croFiles.Count}");

                if (croFiles.Count != numCros)
                    return new ActionResult(false, $"Number of CROs in directory does not match number in CRR! Expected {numCros}, got {croFiles.Count}");

                // build hashtable

                var hashtableHash = new byte[0x20];

                // base address of hashtable in CRR
                int hashtableOffset = GetLe32(crr, 0x350);

                for (int i = 0; i < numCros; i++)
                {
                    string croName = croFiles[i]!;

                    // fills hashtableHash
                    VerifyCro(Path.Combine(crodir, croName), hashtableHash);

                    int hashTableLength = numCros * 0x20;
                    var crrHashtable = crr.AsSpan(hashtableOffset, hashTableLength).ToArray();
                    bool tableOk = CroHashtableVerify(crrHashtable, numCros, hashtableHash);

                    if (tableOk)
                    {
                        CROResults.Add(new ActionResult(true, $"{croName} hashtable valid!"));
                    }
                    else
                        CROResults.Add(new ActionResult(false, $"{croName} hashtable invalid!"));
                }
            }
            catch (Exception ex)
            { 
                return new ActionResult(false, "Error reading file: " + ex.Message);
            }
            return new ActionResult(true, "CRR file is valid.");
        }

        public static ActionResult resign_crr(string crr_filename, string crodir)
        {
            CROResults = new List<ActionResult>();
            long fSize;
            int nu_cros;
            byte[] crr;
            if (!File.Exists(crr_filename))
                return new ActionResult(false, "File does not exist.");

            try
            {
                using (var fs = new FileStream(crr_filename, FileMode.Open, FileAccess.ReadWrite))
                using (BinaryReaderX br = new BinaryReaderX(fs))
                {
                    fSize = fs.Length;
                    crr = br.ReadBytes((int)fSize); 
                }

                int numCros = GetLe32(crr, 0x354);

                List<string> fnames = new List<string>(numCros);
                int i = -1;
                // Equivalent of opendir("./") + readdir
                var files = Directory.EnumerateFiles(crodir, "*.cro", SearchOption.TopDirectoryOnly);

                foreach (var path in files)
                {
                    string name = Path.GetFileName(path);
                    if (name.Length > 4 && name.EndsWith(".cro", StringComparison.OrdinalIgnoreCase))
                    {
                        i++;
                        if (i >= numCros)
                        {
                            return new ActionResult(false, $"Invalid number of CROs! Expected {numCros}, got {i + 1}+");
                        }
                        fnames.Add(name);
                    }
                }
                if (i + 1 != numCros)
                {
                    return new ActionResult(false, $"Invalid number of CROs! Expected {numCros}, got {i + 1}");
                }
                // crr_hashtable = crr + getle32(crr + 0x350)
                int tableOffset = GetLe32(crr, 0x350);

                // For each CRO, rewrite its hash into the CRR hashtable
                for (i = 0; i < numCros; i++)
                {
                    RehashCro(
                        Path.Combine(crodir, fnames[i]),
                        crr.AsSpan(tableOffset + (0x20 * i), 0x20)   // destination slot
                    );
                }
                CROResults.Add(new ActionResult(true, "CRR rehashed!"));

                // rsa_sign_crr(crr + 0x240, crr + 0x40, crr + 0x340, getle32(crr + 0x358) - 0x340);
                {
                    int sigOffset = 0x240;
                    int modOffset = 0x40;
                    int dataOffset = 0x340;
                    int dataLength = GetLe32(crr, 0x358) - 0x340;

                    RsaSignCrr(
                        crr.AsSpan(sigOffset, 0x100),     // signature output
                        crr.AsSpan(modOffset, 0x100),     // public modulus
                        crr.AsSpan(dataOffset, dataLength)// signed area
                    );
                }
                CROResults.Add(new ActionResult(true, "CRR resigned!"));

                // Write patched CRR back to disk
                using (var fs = new FileStream(crr_filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Position = 0;
                    fs.Write(crr, 0, (int)fSize);
                }
            }
            catch (Exception ex)
            {
                return new ActionResult(false, "Error reading file: " + ex.Message);
            }
            return new ActionResult(true, "CRR file resigned.");
        }

        private static byte[] Sha256(byte[] input)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(input);
        }

        private static bool CroHashtableVerify(byte[] crrHashtable, int numCros, byte[] croHash)
        {
            for (int i = 0; i < numCros; i++)
            {
                // Compare 0x20 bytes at offset i * 0x20
                if (crrHashtable.AsSpan(i * 0x20, 0x20).SequenceEqual(croHash))
                    return true;
            }
            return false;
        }

        private static int GetLe32(byte[] buf, int offset)
        {
            return
                (buf[offset + 0] << 0) |
                (buf[offset + 1] << 8) |
                (buf[offset + 2] << 16) |
                (buf[offset + 3] << 24);
        }

        private static void VerifyCro(string filename, byte[] hashtableHash)
        {
            byte[] cro = File.ReadAllBytes(filename);
            int fSize = cro.Length;

            // Hash the hash table (first 0x80 bytes)
            var tableHash = SHA256.HashData(cro.AsSpan(0, 0x80));
            Buffer.BlockCopy(tableHash, 0, hashtableHash, 0, 0x20);

            // Hash and verify the header
            {
                var headerHash = SHA256.HashData(cro.AsSpan(0x80, 0x100));
                bool ok = CtrSha256Verify(
                    cro.AsSpan(0x80, 0x100).ToArray(),
                    headerHash,
                    cro.AsSpan(0, 0x20).ToArray()
                );

                CROResults.Add(new ActionResult(ok, ok
                    ? $"{Path.GetFileName(filename)} header valid!"
                    : $"{Path.GetFileName(filename)} header invalid!"));
            }

            // Section 0
            {
                int offset = GetLe32(cro, 0xB0);
                int len = GetLe32(cro, 0xB4);

                var secHash = SHA256.HashData(cro.AsSpan(offset, len));
                bool ok = CtrSha256Verify(
                    cro.AsSpan(offset, len).ToArray(),
                    secHash,
                    cro.AsSpan(0x20, 0x20).ToArray()
                );

                CROResults.Add(new ActionResult(ok, ok
                    ? $"{Path.GetFileName(filename)} section 0 valid!"
                    : $"{Path.GetFileName(filename)} section 0 invalid!"));
            }

            // Section 1
            {
                int offset = GetLe32(cro, 0xC0);
                int len = GetLe32(cro, 0xB8) - GetLe32(cro, 0xC0);

                var secHash = SHA256.HashData(cro.AsSpan(offset, len));
                bool ok = CtrSha256Verify(
                    cro.AsSpan(offset, len).ToArray(),
                    secHash,
                    cro.AsSpan(0x40, 0x20).ToArray()
                );

                CROResults.Add(new ActionResult(ok, ok
                    ? $"{Path.GetFileName(filename)} section 1 valid!"
                    : $"{Path.GetFileName(filename)} section 1 invalid!"));
            }

            // Section 2
            {
                int offset = GetLe32(cro, 0xB8);
                int len = GetLe32(cro, 0xBC);

                var secHash = SHA256.HashData(cro.AsSpan(offset, len));
                bool ok = CtrSha256Verify(
                    cro.AsSpan(offset, len).ToArray(),
                    secHash,
                    cro.AsSpan(0x60, 0x20).ToArray()
                );

                CROResults.Add(new ActionResult(ok, ok
                    ? $"{Path.GetFileName(filename)} section 2 valid!"
                    : $"{Path.GetFileName(filename)} section 2 invalid!"));
            }
        }

        private static bool CtrSha256Verify(byte[] data, byte[] expectedHash, byte[] storedHash)
        {
            var hash = SHA256.HashData(data);
            return hash.SequenceEqual(storedHash);
        }

        private static void RehashCro(string filename, Span<byte> hashtableHash)
        {
            // fopen(fname, "rb+") → open existing file for read/write
            byte[] cro;
            int fSize;
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                using (var br = new BinaryReader(fs))
                {
                    fSize = (int)fs.Length;
                    cro = br.ReadBytes(fSize);
                }
            }
            catch (Exception ex)
            {
                CROResults.Add(new ActionResult(false, $"Error opening {Path.GetFileName(filename)}: {ex.Message}"));
                return;
            }

            // Hash sections, write those to filebuf
            // ctr_sha_256(cro + 0x80, 0x100, cro + 0x0);
            {
                var hash = SHA256.HashData(cro.AsSpan(0x80, 0x100));
                hash.CopyTo(cro.AsSpan(0x00, 0x20));
            }

            // Section 0
            {
                int offset = GetLe32(cro, 0xB0);
                int len = GetLe32(cro, 0xB4);

                var hash = SHA256.HashData(cro.AsSpan(offset, len));
                hash.CopyTo(cro.AsSpan(0x20, 0x20));
            }

            // Section 1
            {
                int offset = GetLe32(cro, 0xC0);
                int len = GetLe32(cro, 0xB8) - GetLe32(cro, 0xC0);

                var hash = SHA256.HashData(cro.AsSpan(offset, len));
                hash.CopyTo(cro.AsSpan(0x40, 0x20));
            }

            // Section 2
            {
                int offset = GetLe32(cro, 0xB8);
                int len = GetLe32(cro, 0xBC);

                var hash = SHA256.HashData(cro.AsSpan(offset, len));
                hash.CopyTo(cro.AsSpan(0x60, 0x20));
            }

            // get hashtable hash for return
            {
                var hash = SHA256.HashData(cro.AsSpan(0, 0x80));
                hash.CopyTo(hashtableHash);
            }

            // Write updated CRO back to disk
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.Position = 0;
                fs.Write(cro, 0, fSize);
            }

            CROResults.Add(new ActionResult(true, $"{Path.GetFileName(filename)} rehashed!"));
        }

        private static void RsaSignCrr(Span<byte> signatureOut, Span<byte> publicModulus, ReadOnlySpan<byte> dataToSign)
        {
            // Compute SHA-256 hash of the signed area
            byte[] hash = SHA256.HashData(dataToSign);


            RSAParameters rsaParams = new RSAParameters
            {
                Modulus = Keys.GetKey(Keys.KeyType.RsaModulus),
                Exponent = Keys.GetKey(Keys.KeyType.RsaExponent),

                D = Keys.GetKey(Keys.KeyType.RsaPrivateExponent),
                P = Keys.GetKey(Keys.KeyType.RsaPrivatePrime1),
                Q = Keys.GetKey(Keys.KeyType.RsaPrivatePrime2),
                DP = Keys.GetKey(Keys.KeyType.RsaPrivateDP),
                DQ = Keys.GetKey(Keys.KeyType.RsaPrivateDQ),
                InverseQ = Keys.GetKey(Keys.KeyType.RsaPrivateQP),
            };

            using RSA rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);

            // Sign the hash (PKCS#1 v1.5 + SHA-256)
            byte[] sig = rsa.SignHash(
                hash,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            if (sig.Length != 0x100)
                throw new InvalidOperationException($"Unexpected RSA signature length: {sig.Length}");

            // Copy signature into caller’s buffer
            sig.CopyTo(signatureOut);                    // writes directly into crr[0x240..0x33F]
            rsaParams.Modulus.CopyTo(publicModulus);     // writes directly into crr[0x40..0x13F]
        }
    }
}
