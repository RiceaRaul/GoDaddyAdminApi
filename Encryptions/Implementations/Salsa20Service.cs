using Encryptions.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Encryptions.Implementations
{
    public class Salsa20Service : HMACServiceBase, ISalsa20Service
    {
        private readonly SymmetricAlgorithm _salsa20;
        private readonly string key;
        private readonly string iv;
        private readonly int rounds;

        public Salsa20Service(string key, string iv, string hmac, int rounds = 20)
        {
            _salsa20 = new Salsa20 { Rounds = rounds };
            this.key = key;
            this.iv = iv;
            this.rounds = rounds;
            SetKey(ToBytes(hmac));
/*            GenerateKey();*/
        }

        public string Encrypt(string target)
        {
            var encryptor = _salsa20.CreateEncryptor(ToBytes(key), ToBytes(iv));
            byte[] bytes = Encoding.ASCII.GetBytes(target);
            var encryptedData = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            var hmac = ComputeHMAC(encryptedData);
            var encryptedResult = Convert.ToHexString(hmac) + Convert.ToHexString(encryptedData);

            return encryptedResult;
        }

        public string Decrypt(string target)
        {
            var receivedHMAC = ToBytes(target.Substring(0, 64));
            var receivedData = ToBytes(target.Substring(64));

            var computedHMAC = ComputeHMAC(receivedData);

            if (!HMACsMatch(receivedHMAC, computedHMAC))
            {
                throw new InvalidOperationException("HMAC verification failed. The data may have been tampered with.");
            }

            var decryptor = _salsa20.CreateDecryptor(ToBytes(key), ToBytes(iv));
            var decryptedData = decryptor.TransformFinalBlock(receivedData, 0, receivedData.Length);
            var decryptedResult = Encoding.ASCII.GetString(decryptedData);

            return decryptedResult;
        }

        private static byte[] ToBytes(string hex)
        {
            byte[] output = new byte[hex.Length / 2];
            for (int nChar = 0; nChar < hex.Length; nChar += 2)
                output[nChar / 2] = ToByte(hex, nChar);
            return output;
        }

        private static byte ToByte(string hex, int offset)
        {
            return (byte)((ToNibble(hex[offset]) << 4) | ToNibble(hex[offset + 1]));
        }

        private static int ToNibble(char hex)
        {
            return hex > '9' ? hex - ('A' - 10) : hex - '0';
        }
    }
}
