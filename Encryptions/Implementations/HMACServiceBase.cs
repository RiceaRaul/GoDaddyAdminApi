using System.Security.Cryptography;

namespace Encryptions.Implementations
{
    public abstract class HMACServiceBase
    {
        private byte[] hmacKey;

        protected void GenerateKey()
        {
            using (var hmac = new HMACSHA256())
            {
                hmacKey = hmac.Key;
            }
        }

        protected void SetKey(byte[] key)
        {
            hmacKey = key;
        }

        protected byte[] ComputeHMAC(byte[] data)
        {
            using (var hmac = new HMACSHA256(hmacKey))
            {
                return hmac.ComputeHash(data);
            }
        }

        protected bool HMACsMatch(byte[] hmac1, byte[] hmac2)
        {
            if (hmac1.Length != hmac2.Length)
            {
                return false;
            }

            for (int i = 0; i < hmac1.Length; i++)
            {
                if (hmac1[i] != hmac2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
