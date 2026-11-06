using Crypto.Algorithms;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Crypto.Sockets
{
    class Receiver(Socket socket)
    {
        private readonly Socket socket = socket;

        public void LoadFile()
        {
            string targetFolderPath = AppState.ReceiveDir;
            Directory.CreateDirectory(targetFolderPath);

            string filePath;
            byte[] buffer;
            byte[] hashedBytes;

            try
            {
                using (var networkStream = new NetworkStream(socket))
                using (var binaryReader = new BinaryReader(networkStream))
                {
                    string fileName = binaryReader.ReadString();
                    long fileSize = binaryReader.ReadInt64();

                    int hashLength = binaryReader.ReadInt32();
                    hashedBytes = binaryReader.ReadBytes(hashLength);

                    filePath = Path.Combine(targetFolderPath, fileName);
                    int bufferLength = binaryReader.ReadInt32();
                    buffer = binaryReader.ReadBytes(bufferLength);
                }
                CheckHash(hashedBytes, buffer, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Receive error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Dispose();
            }
        }

        private static void CheckHash(byte[] hashedBytes, byte[] buffer, string filePath)
        {
            byte[] hash = TigerHash.Hash(buffer, buffer.Length);

            if (hash.SequenceEqual(hashedBytes))
            {
                MessageBox.Show($"Hash OK - Receive {hashedBytes.Length} bytes succeed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogHash(filePath, "Tiger Hash", buffer.Length);
                DecryptFile(filePath, buffer);
            }
            else
            {
                MessageBox.Show("Hash Error - Receive not succeed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static byte[] ReadMetadata(byte[] decryptedData)
        {
            // 1. Pročitaj prva 4 bajta da saznaš dužinu JSON-a
            int headerLen = BitConverter.ToInt32(decryptedData, 0);

            // 2. Izdvoj JSON bajtove
            byte[] headerBytes = new byte[headerLen];
            Buffer.BlockCopy(decryptedData, 4, headerBytes, 0, headerLen);

            // 3. Deserijalizacija
            string jsonString = Encoding.UTF8.GetString(headerBytes);
            FileMetadata metadata = JsonSerializer.Deserialize<FileMetadata>(jsonString);

            // Prikaz u WinForms-u
            MessageBox.Show($"File: {metadata.FileName}\nSize: {metadata.FileSize} bytes" + $"\nCreation time: {metadata.CreationTime.ToString()}" +
                $"\nAlgorithm: {metadata.EncryptionAlgorithm}\nHash: {metadata.HashAlgorithm}", "Received");

            // 4. Ostatak su podaci fajla
            int dataStart = 4 + headerLen;
            int dataLen = decryptedData.Length - dataStart;
            byte[] originalFile = new byte[dataLen];
            Buffer.BlockCopy(decryptedData, dataStart, originalFile, 0, dataLen);
            return originalFile;
        }
        private static void DecryptFile(string filePath, byte[] buffer)
        {
            try
            {
                byte[] decrypted = new byte[buffer.Length];
                byte[] originalFile;

                byte[] xxtea = new byte[16];
                byte[] cfb = new byte[16];
                byte[] iv = new byte[16];
                int seed=0;
                
                Random rnd = new Random(seed);

                rnd.NextBytes(xxtea);
                rnd.NextBytes(cfb);
                rnd.NextBytes(iv);
                byte r1_pos = 0;
                byte r2_pos = 0;
                byte r3_pos = 0;
                String algorithm ="";

                        
                if (AppState.SelectedCryptoAlgorithm == 1)
                {
                    Enigma enigma = new Enigma(r1_pos, r2_pos, r3_pos, seed);
                                    
                    enigma.SetRotorsPos(r1_pos, r2_pos, r3_pos);
                    decrypted = enigma.Encrypt(buffer);
                    algorithm = "Enigma";

                }
                else if (AppState.SelectedCryptoAlgorithm == 2)
                {                
                    decrypted = XXTEA.Decrypt(buffer, xxtea);
                    algorithm = "XXTEA";
                }
                else if (AppState.SelectedCryptoAlgorithm == 3)
                {
                    decrypted = CFB.Decrypt(buffer, cfb, iv);
                    algorithm = "XXTEA (CFB)";
                }

                originalFile = ReadMetadata(decrypted);
                File.WriteAllBytes(filePath, originalFile);
                LogReceive(filePath, algorithm, decrypted.Length, originalFile.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Decryption error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void LogReceive(string outputPath, string algorithm, int decryptedSize, int originalSize)
        {
            try
            {
                string logDir = AppState.LogDir.Trim();
                string logPath = Path.Combine(logDir, "receive.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                    $"RECEIVE & DECRYPT: {Path.GetFileName(outputPath)} | " +
                    $"Algorithm: {algorithm} | " +
                    $"Size: {decryptedSize} → {originalSize} bytes{Environment.NewLine}";

                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private static void LogHash(string outputPath, string algorithm, int fileSize)
        {
            try
            {
                string logDir = AppState.LogDir.Trim();
                string logPath = Path.Combine(logDir, "receive.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                    $"HASH: {Path.GetFileName(outputPath)} | " +
                    $"Algorithm: {algorithm} | " +
                    $"File Size: {fileSize} {Environment.NewLine}";

                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Dispose()
        {
            try
            {
                socket.Disconnect(false);
                socket.Close();
                socket.Dispose();
            }
            catch { }
        }
    }
}
