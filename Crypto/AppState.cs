using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
    public static class AppState
    {
        public static int SelectedCryptoAlgorithm { get; set; }
        public static string TargetDir { get; set; } = Properties.Settings.Default.TargetDirectory;
        public static string XDir { get; set; } = Properties.Settings.Default.XDirectory;
        public static string DecryptionDir { get; set; } = Properties.Settings.Default.DecryptDir;

        public static string ReceiveDir { get; set; } = Properties.Settings.Default.ReceivedDirectory;
        public static string LogDir { get; set; } = Properties.Settings.Default.LogDir;

    }
}
