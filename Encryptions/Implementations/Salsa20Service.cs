using Encryptions.Interfaces;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace Encryptions.Implementations
{
    public class Salsa20Service : ISalsa20Service
    {
        private readonly SymmetricAlgorithm _salsa20;
        private readonly string Key;
        private readonly string IV;
        private readonly int rounds;

        public Salsa20Service(string key,string iv,int rounds = 8)
        {
            _salsa20 = new Salsa20() { Rounds = rounds };
            Key = key;
            IV = iv;
            this.rounds = rounds;
        }

        public string Encrypt(string target)
        {
            var encrypt = _salsa20.CreateEncryptor(ToBytes(Key), ToBytes(IV));
            byte[] bytes = Encoding.ASCII.GetBytes(target);
            var transform = encrypt.TransformFinalBlock(bytes, 0, bytes.Length);
            string result = Convert.ToHexString(transform);

            return result;           
        }

        public string Decrypt(string target)
        {
            var decryptor = _salsa20.CreateDecryptor(ToBytes(Key),ToBytes(IV));
            byte[] bytes = ToBytes(target);
            var decrypt = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            var decryptResult = Encoding.ASCII.GetString(decrypt);

            return decryptResult;
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
            return (byte)(ToNibble(hex[offset]) * 16 + ToNibble(hex[offset + 1]));
        }

        private static int ToNibble(char hex)
        {
            return hex > '9' ? hex - ('A' - 10) : hex - '0';
        }
    }
}
