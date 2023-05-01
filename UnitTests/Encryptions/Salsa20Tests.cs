using Encryptions.Implementations;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace UnitTests.Encryptions
{
    public class Salsa20Tests
    {
        [Fact]
        public void BlockSize()
        {
            using (SymmetricAlgorithm salsa20 = new Salsa20())
            {
                salsa20.BlockSize.Should().Be(512);

                KeySizes[] sizes = salsa20.LegalBlockSizes;
                sizes.Length.Should().Be(1);
                sizes[0].MinSize.Should().Be(512);
                sizes[0].MaxSize.Should().Be(512);
                sizes[0].SkipSize.Should().Be(0);

                salsa20.Invoking(s => s.BlockSize = 128).Should().Throw<CryptographicException>();
            }
        }

        [Fact]
        public void Encrypt()
        {
            var key = "0053A6F94C9FF24598EB3E91E4378ADD3083D6297CCF2275C81B6EC11467BA0D";
            var iv = "0D74DB42A91077DE";
            using (SymmetricAlgorithm salsa20 = new Salsa20() { Rounds = 8 })
            {
                var encrypt = salsa20.CreateEncryptor(ToBytes(key),ToBytes(iv));
                var text = "Data Source=91.92.136.222;Initial Catalog=CST;User ID=sa;Password=Relisys123;MultipleActiveResultSets=True";
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                var output = encrypt.TransformFinalBlock(bytes, 0, bytes.Length);
                string result = Convert.ToHexString(output);


                byte[] resultBytes = ToBytes(result);
                var decryptor = salsa20.CreateDecryptor(ToBytes(key),ToBytes(iv));
                var decrypt = decryptor.TransformFinalBlock(resultBytes, 0, resultBytes.Length);
                var decryptResult = Encoding.ASCII.GetString(decrypt);
            }
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
