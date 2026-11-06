using System;
using System.IO;
using System.Text;

namespace Crypto.Algorithms
{
    public class XXTEA
    {
        private static readonly UTF8Encoding Utf8 = new();
        private const uint Delta = 0x9E3779B9;

        private static uint MX(uint sum, uint y, uint z, int p, uint e, uint[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }

        public static byte[] Encrypt(byte[] data, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Encrypt(data, Utf8.GetBytes(key));
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length == 0) throw new ArgumentException("Key cannot be empty", nameof(key));

            if (data.Length == 0)
            {
                return data;
            }

            return ToByteArray(
                Encrypt(
                    ToUInt32Array(data, includeLength: true),
                    ToUInt32Array(FixKey(key), includeLength: false)
                ),
                includeLength: false
            );
        }

        public static byte[] Decrypt(byte[] data, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Decrypt(data, Utf8.GetBytes(key));
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length == 0) throw new ArgumentException("Key cannot be empty", nameof(key));

            if (data.Length == 0)
            {
                return data;
            }

            return ToByteArray(
                Decrypt(
                    ToUInt32Array(data, includeLength: false),
                    ToUInt32Array(FixKey(key), includeLength: false)
                ),
                includeLength: true
            );
        }

        private static uint[] Encrypt(uint[] v, uint[] k)
        {
            int n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }

            uint z = v[n], y, sum = 0, e;
            int p, q = 6 + 52 / (n + 1);

            unchecked
            {
                while (q-- > 0)
                {
                    sum += Delta;
                    e = sum >> 2 & 3;

                    for (p = 0; p < n; p++)
                    {
                        y = v[p + 1];
                        z = v[p] += MX(sum, y, z, p, e, k);
                    }

                    y = v[0];
                    z = v[n] += MX(sum, y, z, p, e, k);
                }
            }

            return v;
        }

        private static uint[] Decrypt(uint[] v, uint[] k)
        {
            int n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }

            uint z, y = v[0], sum, e;
            int p, q = 6 + 52 / (n + 1);

            unchecked
            {
                sum = (uint)(q * Delta);

                while (sum != 0)
                {
                    e = sum >> 2 & 3;

                    for (p = n; p > 0; p--)
                    {
                        z = v[p - 1];
                        y = v[p] -= MX(sum, y, z, p, e, k);
                    }

                    z = v[n];
                    y = v[0] -= MX(sum, y, z, p, e, k);
                    sum -= Delta;
                }
            }

            return v;
        }

        private static byte[] FixKey(byte[] key)
        {
            if (key.Length == 16)
                return key;

            byte[] fixedKey = new byte[16];

            if (key.Length < 16)
            {
                key.CopyTo(fixedKey, 0);
            }
            else
            {
                Array.Copy(key, 0, fixedKey, 0, 16);
            }

            return fixedKey;
        }

        private static uint[] ToUInt32Array(byte[] data, bool includeLength)
        {
            int length = data.Length;
            int n = (length & 3) == 0 ? length >> 2 : (length >> 2) + 1;
            uint[] result;

            if (includeLength)
            {
                result = new uint[n + 1];
                result[n] = (uint)length;
            }
            else
            {
                result = new uint[n];
            }

            for (int i = 0; i < length; i++)
            {
                result[i >> 2] |= (uint)data[i] << ((i & 3) << 3);
            }

            return result;
        }

        private static byte[] ToByteArray(uint[] data, bool includeLength)
        {
            int n = data.Length << 2;

            if (includeLength)
            {
                int originalLength = (int)data[data.Length - 1];
                n -= 4;

                if (originalLength < n - 3 || originalLength > n)
                {
                    throw new InvalidDataException(
                        $"Corrupted data: invalid length. Expected length between {n - 3} and {n}, got {originalLength}"
                    );
                }

                n = originalLength;
            }

            byte[] result = new byte[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = (byte)(data[i >> 2] >> ((i & 3) << 3));
            }

            return result;
        }
    }
}
