
namespace Izuto.CROTools
{
    public class KeySet
    {
        public enum KeyStatus
        {
            KEY_ERR_LEN_MISMATCH,
            KEY_ERR_INVALID_NODE,
            KEY_OK
        }
        public enum RSAKeyType
        {
            RSAKEY_INVALID,
            RSAKEY_PRIV,
            RSAKEY_PUB
        }
        public class RSAKey2048
        {
            public byte[] n = new byte[256];
            public byte[] e = new byte[3];
            public byte[] d = new byte[256];
            public byte[] p = new byte[128];
            public byte[] q = new byte[128];
            public byte[] dp = new byte[128];
            public byte[] dq = new byte[128];
            public byte[] qp = new byte[128];
            public RSAKeyType keytype;
        }
        public class key128
        {
            public char[] data = new char[16];
            public bool valid;
        }
    }
}
