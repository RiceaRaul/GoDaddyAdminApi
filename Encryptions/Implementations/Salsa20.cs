using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Encryptions.Implementations
{
    public sealed class Salsa20 : SymmetricAlgorithm
    {
        int m_rounds;

        public Salsa20()
        {
            // set legal values
            LegalBlockSizesValue = new[] { new KeySizes(512, 512, 0) };
            LegalKeySizesValue = new[] { new KeySizes(128, 256, 128) };

            // set default values
            BlockSizeValue = 512;
            KeySizeValue = 256;
            m_rounds = 20;
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            return CreateEncryptor(rgbKey, rgbIV);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            if (rgbKey == null)
                throw new ArgumentNullException("rgbKey");
            if (!ValidKeySize(rgbKey.Length * 8))
                throw new CryptographicException("Invalid key size; it must be 128 or 256 bits.");
            CheckValidIV(rgbIV, "rgbIV");

            return new Salsa20CryptoTransform(rgbKey, rgbIV, m_rounds);
        }

        public override void GenerateIV()
        {
            IVValue = GetRandomBytes(8);
        }

        public override void GenerateKey()
        {
            KeyValue = GetRandomBytes(KeySize / 8);
        }

        public override byte[] IV
        {
            get
            {
                return base.IV;
            }
            set
            {
                CheckValidIV(value, "value");
                IVValue = (byte[])value.Clone();
            }
        }

        public int Rounds
        {
            get
            {
                return m_rounds;
            }
            set
            {
                if (value != 8 && value != 12 && value != 20)
                    throw new ArgumentOutOfRangeException("value", "The number of rounds must be 8, 12, or 20.");
                m_rounds = value;
            }
        }

        private static void CheckValidIV(byte[] iv, string paramName)
        {
            if (iv == null)
                throw new ArgumentNullException(paramName);
            if (iv.Length != 8)
                throw new CryptographicException("Invalid IV size; it must be 8 bytes.");
        }

        private static byte[] GetRandomBytes(int byteCount)
        {
            byte[] bytes = new byte[byteCount];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        private sealed class Salsa20CryptoTransform : ICryptoTransform
        {

            public Salsa20CryptoTransform(byte[] key, byte[] iv, int rounds)
            {
                Debug.Assert(key.Length == 16 || key.Length == 32, "abyKey.Length == 16 || abyKey.Length == 32", "Invalid key size.");
                Debug.Assert(iv.Length == 8, "abyIV.Length == 8", "Invalid IV size.");
                Debug.Assert(rounds == 8 || rounds == 12 || rounds == 20, "rounds == 8 || rounds == 12 || rounds == 20", "Invalid number of rounds.");

                Initialize(key, iv);
                m_rounds = rounds;
            }

            public bool CanReuseTransform
            {
                get { return false; }
            }

            public bool CanTransformMultipleBlocks
            {
                get { return true; }
            }

            public int InputBlockSize
            {
                get { return 64; }
            }

            public int OutputBlockSize
            {
                get { return 64; }
            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                // check arguments
                if (inputBuffer == null)
                    throw new ArgumentNullException("inputBuffer");
                if (inputOffset < 0 || inputOffset >= inputBuffer.Length)
                    throw new ArgumentOutOfRangeException("inputOffset");
                if (inputCount < 0 || inputOffset + inputCount > inputBuffer.Length)
                    throw new ArgumentOutOfRangeException("inputCount");
                if (outputBuffer == null)
                    throw new ArgumentNullException("outputBuffer");
                if (outputOffset < 0 || outputOffset + inputCount > outputBuffer.Length)
                    throw new ArgumentOutOfRangeException("outputOffset");
                if (m_state == null)
                    throw new ObjectDisposedException(GetType().Name);

                byte[] output = new byte[64];
                int bytesTransformed = 0;

                while (inputCount > 0)
                {
                    Hash(output, m_state);
                    m_state[8] = AddOne(m_state[8]);
                    if (m_state[8] == 0)
                    {
                        // NOTE: stopping at 2^70 bytes per nonce is user's responsibility
                        m_state[9] = AddOne(m_state[9]);
                    }

                    int blockSize = Math.Min(64, inputCount);
                    for (int i = 0; i < blockSize; i++)
                        outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ output[i]);
                    bytesTransformed += blockSize;

                    inputCount -= 64;
                    outputOffset += 64;
                    inputOffset += 64;
                }

                return bytesTransformed;
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                if (inputCount < 0)
                    throw new ArgumentOutOfRangeException("inputCount");

                byte[] output = new byte[inputCount];
                TransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
                return output;
            }

            public void Dispose()
            {
                if (m_state != null)
                    Array.Clear(m_state, 0, m_state.Length);
                m_state = null;
            }

            private static uint Rotate(uint v, int c)
            {
                return (v << c) | (v >> (32 - c));
            }

            private static uint Add(uint v, uint w)
            {
                return unchecked(v + w);
            }

            private static uint AddOne(uint v)
            {
                return unchecked(v + 1);
            }

            private void Hash(byte[] output, uint[] input)
            {
                uint[] state = (uint[])input.Clone();

                for (int round = m_rounds; round > 0; round -= 2)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        state[i] ^= Rotate(Add(NextState(state, i), state[i]), 7);
                    }
                }

                for (int index = 0; index < 16; index++)
                {
                    ToBytes(Add(state[index], input[index]), output, 4 * index);
                }
            }

            private uint NextState(uint[] state, int index)
            {
                int nextIndex = (index + 1) % 16;
                return Add(state[nextIndex], state[index]);
            }

            private void Initialize(byte[] key, byte[] iv)
            {
                m_state = new uint[16];
                int keyIndex = key.Length - 16;
                byte[] constants = key.Length == 32 ? c_sigma : c_tau;

                for (int i = 0; i < 4; i++)
                {
                    m_state[i + 1] = ToUInt32(key, i * 4);
                    m_state[i + 11] = ToUInt32(key, keyIndex + (i * 4));
                    m_state[i] = ToUInt32(constants, i * 4);
                }

                for (int i = 0; i < 2; i++)
                {
                    m_state[i + 6] = ToUInt32(iv, i * 4);
                }

                m_state[8] = 0;
                m_state[9] = 0;
            }


            private static uint ToUInt32(byte[] input, int inputOffset)
            {
                return unchecked((uint)(((input[inputOffset] | (input[inputOffset + 1] << 8)) | (input[inputOffset + 2] << 16)) | (input[inputOffset + 3] << 24)));
            }

            private static void ToBytes(uint input, byte[] output, int outputOffset)
            {
                unchecked
                {
                    output[outputOffset] = (byte)input;
                    output[outputOffset + 1] = (byte)(input >> 8);
                    output[outputOffset + 2] = (byte)(input >> 16);
                    output[outputOffset + 3] = (byte)(input >> 24);
                }
            }

            static readonly byte[] c_sigma = Encoding.ASCII.GetBytes("expand 32-byte k");
            static readonly byte[] c_tau = Encoding.ASCII.GetBytes("expand 16-byte k");

            uint[] m_state;
            readonly int m_rounds;
        }
    }
}
