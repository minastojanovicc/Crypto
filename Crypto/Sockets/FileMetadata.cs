using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Sockets
{
    public class FileMetadata
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationTime { get; set; }
        public string EncryptionAlgorithm { get; set; }
        public string HashAlgorithm { get; set; }
    }
}
