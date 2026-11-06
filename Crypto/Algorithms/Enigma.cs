using System;
using System.Collections.Generic;

namespace Crypto.Algorithms
{
    public class Enigma
    {
        public Rotor r1 { get; set; }
        public Rotor r2 { get; set; }
        public Rotor r3 { get; set; }

        public Reflector reflector { get; set; }
        public Plugboard plugboard { get; set; }


        public Enigma(byte r1Pos, byte[] r1Bytes, byte r1Notch, 
                      byte r2Pos, byte[] r2Bytes, byte r2Notch, 
                      byte r3Pos, byte[] r3Bytes, byte r3Notch, 
                      byte[] reflectorBytes, byte[] plugboardBytes)
        {
            r1 = new Rotor(r1Pos, r1Bytes, r1Notch);
            r2 = new Rotor(r2Pos, r2Bytes, r2Notch);
            r3 = new Rotor(r3Pos, r3Bytes, r3Notch);
            reflector = new Reflector(reflectorBytes);
            plugboard = new Plugboard(plugboardBytes);

            // Lanac: r3 (desni) -> r2 (srednji) -> r1 (levi)
            r3.SetNextRotor(r2);
            r2.SetNextRotor(r1);
            r2.SetPreviousRotor(r3);
            r1.SetPreviousRotor(r2);
        }

        public Enigma(int rl, int rm, int rr, int seed)
        {
            Random rnd = new Random(seed);

            var r1bytes = Generator.GenerateRotor((byte)rnd.Next(0, 256));
            var r2bytes = Generator.GenerateRotor((byte)rnd.Next(0, 256));
            var r3bytes = Generator.GenerateRotor((byte)rnd.Next(0, 256));

            var r1Notch = (byte)rnd.Next(0, 256);
            var r2Notch = (byte)rnd.Next(0, 256);
            var r3Notch = (byte)rnd.Next(0, 256);

            r1 = new Rotor((byte)rl, r1bytes, r1Notch);
            r2 = new Rotor((byte)rm, r2bytes, r2Notch);
            r3 = new Rotor((byte)rr, r3bytes, r3Notch);

            reflector = new Reflector(Generator.GenerateReflector((byte)rnd.Next(0, 256)));
            plugboard = new Plugboard(Generator.GeneratePlugboard((byte)rnd.Next(0, 256), rnd.Next(0, 129)));

            //var config = new EnigmaConfig();
            //r3 = new Rotor((byte)rr, Generator.GenerateRotor(config.RotorSeeds[0]), config.NotchPositions[0]);
            //r2 = new Rotor((byte)rm, Generator.GenerateRotor(config.RotorSeeds[1]), config.NotchPositions[1]);
            //r1 = new Rotor((byte)rl, Generator.GenerateRotor(config.RotorSeeds[2]), config.NotchPositions[2]);
            //reflector = new Reflector(Generator.GenerateReflector(config.ReflectorSeed));
            //plugboard = new Plugboard(Generator.GeneratePlugboard(config.PlugboardSeed, config.PlugboardPairs));


            r3.SetNextRotor(r2);
            r2.SetNextRotor(r1);
            r2.SetPreviousRotor(r3);
            r1.SetPreviousRotor(r2);
        }

        public void SetRotorsPos(byte rl, byte rm, byte rr)
        {
            r3.SetRotorPosition(rr);
            r2.SetRotorPosition(rm);
            r1.SetRotorPosition(rl);
        }

        public byte[] Encrypt(byte[] message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            byte[] mes = new byte[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                // Double-stepping mehanizam
                if (r2.AtNotch())
                {
                    r1.Step();
                    r2.Step();
                }
                else if (r3.AtNotch())  // ISPRAVLJENO: else if umesto if
                {
                    r2.Step();
                }
                r3.Step();

                // Enkripcija
                byte b = message[i];
                
                // Plugboard ulaz
                b = plugboard.Substitute(b);
                
                // Forward pass kroz rotore (r3 → r2 → r1)
                b = r3.ForwardPass(b);
                
                // Reflektor
                b = reflector.Reflect(b);
                
                // Backward pass kroz rotore (r1 → r2 → r3)
                b = r1.BackPass(b);  
                
                // Plugboard izlaz
                b = plugboard.Substitute(b);
                
                mes[i] = b;
            }
            return mes;
        }

        public byte[] Decrypt(byte[] message)
        {
            // Enigma je simetrična - Encrypt = Decrypt
            return Encrypt(message);
        }

        public void SetConfig(byte r1Pos, byte[] r1Bytes, byte r1Notch,
              byte r2Pos, byte[] r2Bytes, byte r2Notch,
              byte r3Pos, byte[] r3Bytes, byte r3Notch,
              byte[] reflectorBytes, byte[] plugboardBytes)
        {
            r1.pos = r1Pos;
            r1.bytes = r1Bytes;
            r1.notch = r1Notch;
            r2.pos = r2Pos;
            r2.bytes = r2Bytes;
            r2.notch = r2Notch;
            r3.pos = r3Pos;
            r3.bytes = r3Bytes;
            r3.notch = r3Notch;
            reflector.bytes = reflectorBytes;
            plugboard.bytes = plugboardBytes;
            return;
        }

        public class EnigmaConfig
        {
            public byte[] RotorSeeds { get; set; } = { 20, 170, 2 };
            public byte[] NotchPositions { get; set; } = { 60, 140, 220 };
            public byte ReflectorSeed { get; set; } = 100;
            public byte PlugboardSeed { get; set; } = 50;
            public byte PlugboardPairs { get; set; } = 10;
        }
    }


    public class Rotor
    {
        public byte pos { get; set; }
        Rotor prev, next;
        public byte[] bytes { get; set; }
        public byte notch { get; set; }

        public Rotor(byte position, byte[] bytes, byte notch)
        {
            pos = position;
            this.bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 256)
                throw new ArgumentException("Rotor must have 256 bytes");
            this.notch = notch;
        }

        public void SetNextRotor(Rotor next)
        {
            this.next = next;
        }

        public void SetPreviousRotor(Rotor prev)
        {
            this.prev = prev;
        }

        public void SetRotorPosition(byte position)
        {
            pos = position;
        }

        public void Step()
        {
            pos = (byte)((pos + 1) % 256);
        }

        public bool AtNotch()
        {
            return pos == notch;
        }

        public byte ForwardPass(byte b)
        {
            int res;
            
            if (prev == null)
            {
                // Prvi rotor (r3) - direktan pristup
                res = bytes[(pos + b) % 256];
            }
            else
            {
                // Srednji/levi rotor - sa offsetom
                res = bytes[(b + (pos - prev.pos) + 256) % 256];
            }

            if (next != null)
            {
                // Prosledi dalje kroz lanac
                res = next.ForwardPass((byte)res);
            }
            else
            {
                // Poslednji rotor - ukloni offset
                res = (res - pos + 256) % 256;
            }

            return (byte)res;
        }

        public byte BackPass(byte b)
        {
            int res;

            if (next == null)
                res = SearchPosition((pos + b) % 256);
            else
                res = SearchPosition((b - (next.pos - pos) + 256) % 256);

            if (prev != null)
                res = prev.BackPass((byte)res);
            else
                res = (res - pos + 256) % 256;

            return (byte)res;
        }

        private int SearchPosition(int b)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == b)
                {
                    return i;
                }
            }
            throw new InvalidOperationException($"Value {b} not found in rotor");
        }
    }

    public class Reflector
    {
        public byte[] bytes;

        public Reflector(byte[] bytes)
        {
            this.bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 256)
                throw new ArgumentException("Reflector must have 256 bytes");
        }

        public byte Reflect(byte b)
        {
            return bytes[b];
        }
    }

    public class Plugboard
    {
        public byte[] bytes;

        public Plugboard(byte[] bytes)
        {
            this.bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 256)
                throw new ArgumentException("Plugboard must have 256 bytes");
        }

        public byte Substitute(byte b)
        {
            return bytes[b];
        }
    }

    public class Generator
    {
        public static byte[] GenerateRotor(byte start)
        {
            byte[] rotor = new byte[256];
            for (int i = 0; i < 256; i++)
                rotor[i] = (byte)i;
            
            return Shuffle(rotor, start);
        }

        private static byte[] Shuffle(byte[] rotor, byte start)
        {
            Random rand = new (start);
            for (int i = rotor.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (rotor[i], rotor[j]) = (rotor[j], rotor[i]);
            }
            return rotor;
        }

        public static byte[] GenerateReflector(byte start)
        {
            byte[] reflector = new byte[256];
            Random rand = new (start);
            bool[] used = new bool[256];

            for (int i = 0; i < 256; i++)
            {
                if (!used[i])
                {
                    int j;
                    do
                    {
                        j = rand.Next(256);
                    } while (used[j] || i == j);

                    reflector[i] = (byte)j;
                    reflector[j] = (byte)i;
                    used[i] = used[j] = true;
                }
            }

            return reflector;
        }

        public static byte[] GeneratePlugboard(byte start, int pairCount)
        {
            if (pairCount > 128) throw new ArgumentException("Max 128 pairs");

            byte[] plugboard = new byte[256];
            for (int i = 0; i < 256; i++)
                plugboard[i] = (byte)i;

            Random rand = new (start);
            HashSet<byte> used = new HashSet<byte>();
            int count = 0;

            while (count < pairCount)
            {
                byte a = (byte)rand.Next(256);
                byte b = (byte)rand.Next(256);

                if (a != b && !used.Contains(a) && !used.Contains(b))
                {
                    plugboard[a] = b;
                    plugboard[b] = a;
                    used.Add(a);
                    used.Add(b);
                    count++;
                }
            }

            return plugboard;
        }
    }
}
