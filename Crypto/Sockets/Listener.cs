using System.Net;
using System.Net.Sockets;

namespace Crypto.Sockets
{
    class Listener
    {
        private static Socket? socket;
        private static bool isStopped;

        public static void Start(int port)
        {
            if (socket != null)
                return;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(100);
            }
            catch (Exception)
            {
                Stop();
                throw;
            }

            isStopped = false;
            new Thread(() =>
            {
                while (!isStopped)
                {
                    try
                    {
                        var client = socket.Accept();
                        new Thread(() =>
                        {
                            try
                            {
                                var receiver = new Receiver(client);
                                receiver.LoadFile();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        if (!isStopped)
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }).Start();

        }
        public static void Stop()
        {
            try
            {
                isStopped = true;
                if (socket != null)
                {
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
            }
            catch { }
        }
    }
}
