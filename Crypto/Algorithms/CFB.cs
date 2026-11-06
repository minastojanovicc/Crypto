using System;

namespace Crypto.Algorithms
{
    public class CFB
    {
        private const int BlockSize = 16; // 16B = 128b

        public static byte[] Encrypt(byte[] inputBytes, byte[] key, byte[] iv)
        {
            if (inputBytes == null) throw new ArgumentNullException(nameof(inputBytes));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (iv == null) throw new ArgumentNullException(nameof(iv));
            if (iv.Length != BlockSize)
                throw new ArgumentException($"IV must be exactly {BlockSize} bytes", nameof(iv));

            if (inputBytes.Length == 0)
                return Array.Empty<byte>();

            byte[] result = new byte[inputBytes.Length];
            byte[] feedback = new byte[BlockSize];
            Array.Copy(iv, feedback, BlockSize);

            for (int i = 0; i < inputBytes.Length; i += BlockSize)
            {
                // Enkriptuj feedback buffer (uvek ceo blok!)
                byte[] encryptedFeedback = XXTEA.Encrypt(feedback, key);

                // VAŽNO: XXTEA može vratiti više od BlockSize bajtova zbog padding-a
                // Koristimo samo prvih BlockSize bajtova
                if (encryptedFeedback.Length < BlockSize)
                {
                    throw new InvalidOperationException(
                        $"XXTEA encryption returned less than {BlockSize} bytes"
                    );
                }

                int bytesToProcess = Math.Min(BlockSize, inputBytes.Length - i);

                // XOR plaintext sa enkriptovanim feedback-om
                for (int j = 0; j < bytesToProcess; j++)
                {
                    result[i + j] = (byte)(inputBytes[i + j] ^ encryptedFeedback[j]);
                }

                // Pripremi feedback za sledeću iteraciju
                if (bytesToProcess == BlockSize)
                {
                    // Pun blok - kopiraj ceo ciphertext
                    Array.Copy(result, i, feedback, 0, BlockSize);
                }
                else
                {
                    // Poslednji nepotpun blok - pomeri i dodaj novi ciphertext
                    Array.Copy(feedback, bytesToProcess, feedback, 0, BlockSize - bytesToProcess);
                    Array.Copy(result, i, feedback, BlockSize - bytesToProcess, bytesToProcess);
                }
            }

            return result;
        }

        public static byte[] Decrypt(byte[] inputBytes, byte[] key, byte[] iv)
        {
            if (inputBytes == null) throw new ArgumentNullException(nameof(inputBytes));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (iv == null) throw new ArgumentNullException(nameof(iv));
            if (iv.Length != BlockSize)
                throw new ArgumentException($"IV must be exactly {BlockSize} bytes", nameof(iv));

            if (inputBytes.Length == 0)
                return Array.Empty<byte>();

            byte[] result = new byte[inputBytes.Length];
            byte[] feedback = new byte[BlockSize];
            Array.Copy(iv, feedback, BlockSize);

            for (int i = 0; i < inputBytes.Length; i += BlockSize)
            {
                // Enkriptuj feedback buffer (uvek ceo blok!)
                byte[] encryptedFeedback = XXTEA.Encrypt(feedback, key);

                if (encryptedFeedback.Length < BlockSize)
                {
                    throw new InvalidOperationException(
                        $"XXTEA encryption returned less than {BlockSize} bytes"
                    );
                }

                int bytesToProcess = Math.Min(BlockSize, inputBytes.Length - i);

                // XOR ciphertext sa enkriptovanim feedback-om
                for (int j = 0; j < bytesToProcess; j++)
                {
                    result[i + j] = (byte)(inputBytes[i + j] ^ encryptedFeedback[j]);
                }

                // Pripremi feedback za sledeću iteraciju
                // KRITIČNO: Koristimo CIPHERTEXT (inputBytes), ne PLAINTEXT (result)!
                if (bytesToProcess == BlockSize)
                {
                    // Pun blok - kopiraj ceo ciphertext
                    Array.Copy(inputBytes, i, feedback, 0, BlockSize);
                }
                else
                {
                    // Poslednji nepotpun blok - pomeri i dodaj novi ciphertext
                    Array.Copy(feedback, bytesToProcess, feedback, 0, BlockSize - bytesToProcess);
                    Array.Copy(inputBytes, i, feedback, BlockSize - bytesToProcess, bytesToProcess);
                }
            }

            return result;
        }
    }
}
