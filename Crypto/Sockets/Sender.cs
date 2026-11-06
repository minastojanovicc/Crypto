using System.Net;
using System.Net.Sockets;

namespace Crypto.Sockets
{
    class Sender
    {
        private static readonly List<Socket> sockets = [];

        public static void Send(IPAddress iPAddress, int port, string filePath, byte[] bytes, byte[] hashedBytes)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sockets.Add(socket);

            try
            {
                var localEndPoint = new IPEndPoint(iPAddress, port);
                socket.Connect(localEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Došlo je do greške.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var fileInfo = new FileInfo(filePath);

                using var networkStream = new NetworkStream(socket);
                using var binaryWriter = new BinaryWriter(networkStream);

                binaryWriter.Write(fileInfo.Name);
                binaryWriter.Write(fileInfo.Length);

                binaryWriter.Write(hashedBytes.Length);
                binaryWriter.Write(hashedBytes);

                binaryWriter.Write(bytes.Length);
                binaryWriter.Write(bytes);
            }
            catch { }
            finally
            {
                try
                {
                    if (socket.Connected)
                        socket.Disconnect(false);
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                catch { }
            }
        }

        public static void Close()
        {
            foreach (Socket socket in sockets)
            {
                if (socket != null)
                {
                    try
                    {
                        if (socket.Connected)
                            socket.Disconnect(false);
                        socket.Close();
                        socket.Dispose();
                    }
                    catch { }
                }
            }
        }
    }
}
