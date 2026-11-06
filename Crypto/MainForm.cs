using Crypto.Algorithms;
using Crypto.Sockets;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Crypto
{
    public partial class MainForm : Form
    {
        private FileSystemWatcher watcher;
        private BlockingCollection<string> queue;
        private Task[] tasks;
        private CancellationTokenSource cancellationTokenSource;

        public byte r1_pos { get; set; } = 0;
        public byte r2_pos { get; set; } = 0;
        public byte r3_pos { get; set; } = 0;

        public Enigma enigma { get; set; }
        public byte[] xxtea { get; set; } = new byte[16];
        public byte[] cfb { get; set; } = new byte[16];
        public byte[] iv { get; set; } = new byte[16];


        enum Algorithm { Enigma = 1, XXTEA, CFB };
        int seed = 0;

        public MainForm()
        {
            InitializeComponent();

            AppState.SelectedCryptoAlgorithm = 0;

            r1_pos = byte.Parse(txtR1.Text);
            r2_pos = byte.Parse(txtR2.Text);
            r3_pos = byte.Parse(txtR3.Text);

            enigma = new Enigma(r1_pos, r2_pos, r3_pos, seed);

            GenerateXXTEAKeys(0);

            textBoxT.Text = AppState.TargetDir;
            textBoxX.Text = AppState.XDir;
            lblOutDir.Text = AppState.XDir;
            txtLogDir.Text = AppState.LogDir;

            watcher = new FileSystemWatcher();
            CheckForIllegalCrossThreadCalls = false;

            foreach (Control control in groupBoxAlgoritmi.Controls)
            {
                if (control is RadioButton radioButton)
                {
                    radioButton.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
                }
            }
        }
        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                if (rdoEnigma.Checked)
                {
                    AppState.SelectedCryptoAlgorithm = 1;
                }
                if (rdoXXTea.Checked)
                {
                    AppState.SelectedCryptoAlgorithm = 2;
                }
                else if (rdoCFB.Checked)
                {
                    AppState.SelectedCryptoAlgorithm = 3;
                }
            }
        }

        private void SetWatcher()
        {
            string targetDir = textBoxT.Text.Trim();
            string xDir = textBoxX.Text.Trim();

            // Validacija direktorijuma
            if (string.IsNullOrEmpty(targetDir))
            {
                throw new InvalidOperationException("Target directory is not selected!");
            }

            AppState.TargetDir = targetDir;

            if (string.IsNullOrEmpty(xDir))
            {
                throw new InvalidOperationException("X directory is not selected!");
            }

            AppState.XDir = xDir;

            queue = new BlockingCollection<string>();
            tasks = new Task[1];
            cancellationTokenSource = new CancellationTokenSource();

            // Kreiranje direktorijuma ako ne postoje
            Directory.CreateDirectory(targetDir);
            Directory.CreateDirectory(xDir);

            // Konfiguracija watchera
            watcher.Path = targetDir;  // ← Koristi textBoxT.Text
            watcher.Filter = "*.*";
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            watcher.Created += OnCreated;

            // Dodaj postojeće fajlove u red
            foreach (string file in Directory.GetFiles(watcher.Path))
            {
                queue.Add(file);
                DodajStatusPoruku($"Processing file: {Path.GetFileName(file)}");
            }

            DodajStatusPoruku($"FSW monitor: {targetDir}");
            DodajStatusPoruku($"Encrypted in: {xDir}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            queue.Add(e.FullPath);

            DodajStatusPoruku($"New file: {Path.GetFileName(e.FullPath)}");

            // Log zapis o detekciji
            LogDetection(e.FullPath);
        }

        private void StartProcessing()
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                int taskId = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    foreach (var path in queue.GetConsumingEnumerable(cancellationTokenSource.Token))
                    {
                        try
                        {
                            ProcessFile(path, taskId);
                        }
                        catch (Exception ex)
                        {
                            DodajStatusPoruku($"Error in file processing {Path.GetFileName(path)}: {ex.Message}");
                            LogError(path, ex.Message);
                        }
                    }
                }, cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            watcher.EnableRaisingEvents = true;
            DodajStatusPoruku($"FSW started - {tasks.Length} worker tasks active");
        }

        private async Task StopProcessing()
        {
            try
            {
                watcher.EnableRaisingEvents = false;

                // Signaliziraj zaustavljanje
                cancellationTokenSource?.Cancel();
                queue?.CompleteAdding();

                // Sačekaj da se svi task-ovi završe
                if (tasks != null)
                {
                    await Task.WhenAll(tasks);

                    foreach (var task in tasks)
                    {
                        task?.Dispose();
                    }
                }

                // Cleanup
                queue?.Dispose();
                cancellationTokenSource?.Dispose();

                tasks = null;
                queue = null;
                cancellationTokenSource = null;

                DodajStatusPoruku("FSW stopped - all tasks completed");
            }
            catch (Exception ex)
            {
                DodajStatusPoruku($"Error in FSW stopping: {ex.Message}");
            }
        }

        private void ProcessFile(string path, int taskId)
        {
            byte[] buffer = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    // Sačekaj da fajl bude potpuno zapisan
                    Thread.Sleep(100);
                    buffer = File.ReadAllBytes(path);
                    break;
                }
                catch (IOException)
                {
                    if (i == 4) throw; // Poslednji pokušaj - baci exception
                    Thread.Sleep(500); // Sačekaj duže
                }
            }

            if (buffer == null || buffer.Length == 0)
            {
                DodajStatusPoruku($"✗ Fajl je prazan: {Path.GetFileName(path)}");
                LogError(path, "Fajl is empty or cannot be read");
                return;
            }

            DodajStatusPoruku($"[Task {taskId}] Encryption: {Path.GetFileName(path)} ({buffer.Length:N0} bytes)");

            byte[] encrypted;
            string algorithm;

            // Enkripcija sa odabranim algoritmom
            if (rdoEnigma.Checked)
            {
                enigma.SetRotorsPos(r1_pos, r2_pos, r3_pos);
                encrypted = enigma.Encrypt(buffer);
                algorithm = "Enigma";
            }
            else if (rdoXXTea.Checked)
            {
                encrypted = XXTEA.Encrypt(buffer, xxtea);
                algorithm = "XXTEA";
            }
            else if (rdoCFB.Checked)
            {
                encrypted = CFB.Encrypt(buffer, cfb, iv);
                algorithm = "CFB";
            }
            else
            {
                DodajStatusPoruku("Crypto algorithm is not selected");
                return;
            }

            // Sačuvaj enkriptovani fajl
            string xDir = textBoxX.Text.Trim();
            Directory.CreateDirectory(xDir);

            string outputPath = Path.Combine(xDir, Path.GetFileName(path));
            File.WriteAllBytes(outputPath, encrypted);

            DodajStatusPoruku($"✓ [Task {taskId}] Encrypted: {Path.GetFileName(path)} → {Path.GetFileName(outputPath)}");

            // Log zapis o kriptovanju
            LogFSWEncryption(path, outputPath, algorithm, buffer.Length, encrypted.Length);
        }

        private void LogDetection(string filePath)
        {
            try
            {
                string logDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(logDir, "fsw_detection.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] DETECTION: {filePath}{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LogFSWEncryption(string inputPath, string outputPath, string algorithm, int originalSize, int encryptedSize)
        {
            try
            {
                string logDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(logDir, "fsw_encryption.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                                $"FSW ENCRYPTION: {Path.GetFileName(inputPath)} → {Path.GetFileName(outputPath)} | " +
                                $"Algorithm: {algorithm} | " +
                                $"Size: {originalSize} → {encryptedSize} bytes{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LogEncryption(string inputPath, string outputPath, string algorithm, int originalSize, int encryptedSize)
        {
            try
            {
                string logDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(logDir, "encryption-decryption.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                                $"ENCRYPTION: {Path.GetFileName(inputPath)} → {Path.GetFileName(outputPath)} | " +
                                $"Algorithm: {algorithm} | " +
                                $"Size: {originalSize} → {encryptedSize} bytes{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void LogDecryption(string inputPath, string outputPath, string algorithm, int originalSize, int encryptedSize)
        {
            try
            {
                string logDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(logDir, "encryption-decryption.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                    $"DECRYPTION: {Path.GetFileName(inputPath)} → {Path.GetFileName(outputPath)} | " +
                    $"Algorithm: {algorithm} | " +
                    $"Size: {originalSize} → {encryptedSize} bytes{Environment.NewLine}";

                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LogSend(string inputPath, IPAddress ip, string port, string algorithm,
                                        int originalSize, int encryptedSize)
        {
            try
            {
                string logDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(logDir, "send.log");
                string logEntry;
                logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                        $"ENCRYPT & SEND: {Path.GetFileName(inputPath)} → " +
                        $"IP address: {ip.ToString()}:port  | " +
                        $"Algorithm: {algorithm} | " +
                        $"Size: {originalSize} → {encryptedSize} bytes{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LogError(string filePath, string errorMessage)
        {
            try
            {
                string logDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(logDir, "fsw_errors.log");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                                $"ERROR: {Path.GetFileName(filePath)} - {errorMessage}{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogFSW(string filePath, bool started)
        {
            try
            {
                string xDir = txtLogDir.Text.Trim();
                string logPath = Path.Combine(xDir, "fsw.log");
                string logEntry;
                if (started)
                    logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] FSW started";
                else
                    logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] FSW stopped";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ucitajFajlDugme_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Open file";
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFIleEncryption.Text = openFileDialog.FileName;
            }
        }

        private async void enkriptujDugme_Click(Object sender, EventArgs e)
        {
            string filename = txtFIleEncryption.Text;
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("File must be specified!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            btnEncrypt.Enabled = false;

            try
            {
                await Task.Run(() =>
                {
                    byte[] buffer = File.ReadAllBytes(filename);
                    byte[] encrypted = new byte[buffer.Length];
                    string algorithm = "";

                    if (rdoEnigma.Checked)
                    {
                        enigma.SetRotorsPos(r1_pos, r2_pos, r3_pos);
                        encrypted = enigma.Encrypt(buffer);
                        algorithm = "Enigma";
                    }
                    else if (rdoXXTea.Checked)
                    {
                        encrypted = XXTEA.Encrypt(buffer, xxtea);
                        algorithm = "XXTEA";
                    }
                    else if (rdoCFB.Checked)
                    {
                        encrypted = CFB.Encrypt(buffer, cfb, iv);
                        algorithm = "XXTEA (CFB)";
                    }
                    Directory.CreateDirectory(textBoxX.Text);
                    File.WriteAllBytes(Path.Combine(textBoxX.Text, Path.GetFileName(filename)), encrypted);
                    LogEncryption(filename, textBoxX.Text, algorithm, buffer.Length, encrypted.Length);
                });

                DodajStatusPoruku($"Encryption finished {filename}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Encryption error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                txtFIleEncryption.Text = "";
                btnEncrypt.Enabled = true;
            }
        }

        private async void chkFSW_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFSW.Checked)
                {
                    string targetDir = textBoxT.Text.Trim();
                    string xDir = textBoxX.Text.Trim();

                    if (string.IsNullOrEmpty(targetDir))
                    {
                        MessageBox.Show("Target directory must be selected!", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chkFSW.Checked = false;
                        return;
                    }

                    if (string.IsNullOrEmpty(xDir))
                    {
                        MessageBox.Show("X directory must be selected!", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chkFSW.Checked = false;
                        return;
                    }

                    if (!Directory.Exists(targetDir))
                    {
                        MessageBox.Show($"Target directory not exists: {targetDir}", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chkFSW.Checked = false;
                        return;
                    }

                    // Proveri da li je izabran algoritam
                    if (!rdoEnigma.Checked && !rdoXXTea.Checked && !rdoCFB.Checked)
                    {
                        MessageBox.Show("Encryption algorithm must be selected!", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chkFSW.Checked = false;
                        return;
                    }

                    pageEncryption.Enabled = false;
                    pageDecryption.Enabled = false;
                    DodajStatusPoruku("File System Watcher STARTED");
                    LogFSW(AppState.LogDir, true);
                    SetWatcher();
                    StartProcessing();
                }
                else
                {
                    pageEncryption.Enabled = true;
                    pageDecryption.Enabled = true;
                    DodajStatusPoruku("File System Watcher STOPPED");
                    LogFSW(AppState.LogDir, false);
                    await StopProcessing();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"FSW error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkFSW.Checked = false;
            }
        }

        private async void DodajStatusPoruku(string poruka)
        {
            toolStripStatusLabel.Text = poruka;
            await Task.Delay(3000);
            toolStripStatusLabel.Text = "";
        }

        private void buttonIzaberiFajl_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Open file for encryption";
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxKlijent.Text = openFileDialog.FileName;
            }
        }

        private async void buttonPosalji_Click(object sender, EventArgs e)
        {
            string filePath = textBoxKlijent.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("The file for encryption must be specified", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            buttonPosalji.Enabled = false;

            byte[] buffer = [];

            //for (int i = 0; i < 3; i++)
            //{
            //    try
            //    {
            //        buffer = PrepareFileWithHeader(filePath);
            //    }
            //    catch (IOException)
            //    {
            //        Thread.Sleep(200);
            //    }
            //}

            //         buffer = File.ReadAllBytes(filePath); // bez METADATA headera

            buffer = PrepareFileWithHeader(filePath);

            byte[] encrypted = new byte[buffer.Length];

            if (buffer.Length == 0)
            {
                buttonPosalji.Enabled = true;
                MessageBox.Show($"Error in file reading", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DodajStatusPoruku($"File transfer starts {filePath}.");
                byte[] hashedBytes = [];
                String alg = "";
                await Task.Run(() =>
                {
                    if (rdoEnigma.Checked)
                    {
                        enigma.SetRotorsPos(r1_pos, r2_pos, r3_pos);
                        encrypted = enigma.Encrypt(buffer);
                        alg = "Enigma";
                    }
                    else if (rdoXXTea.Checked)
                    {
                        encrypted = XXTEA.Encrypt(buffer, xxtea);
                        alg = "XXTEA";
                    }
                    else if (rdoCFB.Checked)
                    {
                        encrypted = CFB.Encrypt(buffer, cfb, iv);
                        alg = "XXTEA (CFB)";
                    }

                    hashedBytes = TigerHash.Hash(encrypted, encrypted.Length);
                });

                IPAddress iPAddress = IPAddress.Parse(textBoxIPKlijent.Text);
                int port = int.Parse(textBoxPortKlijent.Text);

                new Thread(() =>
                {
                    Sender.Send(iPAddress, port, filePath, encrypted, hashedBytes);
                }).Start();

                DodajStatusPoruku($"File {filePath} is sent");
                LogSend(filePath, iPAddress, textBoxPortKlijent.Text, alg, buffer.Length, encrypted.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonPosalji.Enabled = true;
                textBoxKlijent.Text = "";
            }
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            if (btnStartServer.Text == "Start server")
            {
                try
                {
                    DodajStatusPoruku("Server started");
                    Listener.Start(int.Parse(txtPortServer.Text));
                    btnStartServer.Text = "Stop server";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error in server starting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                DodajStatusPoruku("Server stopped");
                Listener.Stop();
                btnStartServer.Text = "Start server";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Listener.Stop();
            Sender.Close();
        }

        private void buttonOdaberiT_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxT.Text = folderBrowserDialog.SelectedPath;
                AppState.TargetDir = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonOdaberiX_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxX.Text = folderBrowserDialog.SelectedPath;
                AppState.XDir = folderBrowserDialog.SelectedPath;
                lblOutDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonUcitajDekodiranje_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Open file for decryption";
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxUlazDekod.Text = openFileDialog.FileName;
            }
        }

        private void buttonIzaberiDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxIzlazDekod.Text = folderBrowserDialog.SelectedPath;
                AppState.DecryptionDir = folderBrowserDialog.SelectedPath;
            }
        }

        private async void buttonDekodiraj_Click(object sender, EventArgs e)
        {
            string filename = textBoxUlazDekod.Text;
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("File for decription must be specified!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string folder = textBoxIzlazDekod.Text;
            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Output directory must be specified!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            buttonDekodiraj.Enabled = false;

            try
            {
                await Task.Run(() =>
                {
                    byte[] buffer = File.ReadAllBytes(filename);
                    byte[] decrypted = new byte[buffer.Length];
                    String algorithm = "";

                    if (rdoEnigma.Checked)
                    {
                        enigma.SetRotorsPos(r1_pos, r2_pos, r3_pos);
                        decrypted = enigma.Encrypt(buffer);
                        algorithm = "Enigma";
                    }
                    else if (rdoXXTea.Checked)
                    {
                        decrypted = XXTEA.Decrypt(buffer, xxtea);
                        algorithm = "XXTEA";
                    }
                    else if (rdoCFB.Checked)
                    {
                        decrypted = CFB.Decrypt(buffer, cfb, iv);
                        algorithm = "XXTEA (CFB)";
                    }

                    File.WriteAllBytes(Path.Combine(folder, Path.GetFileName(filename)), decrypted);
                    LogDecryption(filename, textBoxIzlazDekod.Text, algorithm, buffer.Length, decrypted.Length);
                });

                DodajStatusPoruku($"File {filename} decryption is finished");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                textBoxUlazDekod.Text = "";
                textBoxIzlazDekod.Text = "";
                buttonDekodiraj.Enabled = true;
            }
        }

        private void GenerateXXTEAKeys(int s)
        {
            Random rnd = new Random(s);

            rnd.NextBytes(xxtea);
            rnd.NextBytes(cfb);
            rnd.NextBytes(iv);

        }

        private void GenerateKeys(int s)
        {
            Random rnd = new Random(s);

            //var r1Pos = (byte)rnd.Next(0, 256);
            //var r2Pos = (byte)rnd.Next(0, 256);
            //var r3Pos = (byte)rnd.Next(0, 256);

            //this.r1_pos = r1Pos;
            //this.r2_pos = r2Pos;
            //this.r3_pos = r3Pos;

            var r1 = Generator.GenerateRotor((byte)rnd.Next(0, 256));
            var r2 = Generator.GenerateRotor((byte)rnd.Next(0, 256));
            var r3 = Generator.GenerateRotor((byte)rnd.Next(0, 256));

            var r1Notch = (byte)rnd.Next(0, 256);
            var r2Notch = (byte)rnd.Next(0, 256);
            var r3Notch = (byte)rnd.Next(0, 256);

            var reflector = Generator.GenerateReflector((byte)rnd.Next(0, 256));
            var plugboard = Generator.GeneratePlugboard((byte)rnd.Next(0, 256), rnd.Next(0, 129));

            enigma.SetConfig(this.r1_pos, r1, r1Notch, this.r2_pos, r2, r2Notch, this.r3_pos, r3, r3Notch, reflector, plugboard);

            Random rnd1 = new(s);
            rnd1.NextBytes(xxtea);
            rnd1.NextBytes(cfb);
            rnd1.NextBytes(iv);
        }
        private void generisiKljucBtn_Click(object sender, EventArgs e)
        {
            int newseed;
            if (int.TryParse(txtNewSeed.Text, out newseed))
            {
                this.seed = newseed; // za sada ostaviti seed = 0
                this.seed = 0;
                GenerateKeys(seed);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }

            DodajStatusPoruku("Keys are generated");
        }

        private void sacuvajKljucBtn_Click(object sender, EventArgs e)
        {
            string folder = "";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folder = folderBrowserDialog.SelectedPath;
            }
            if (folder == "")
            {
                MessageBox.Show("Specify directory for key storage!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string filePath = Path.Combine(folder, "crypto.key");

            try
            {
                using (BinaryWriter writer = new(File.Open(filePath, FileMode.Create)))
                {
                    writer.Write(r1_pos);
                    writer.Write(enigma.r1.bytes);
                    writer.Write(enigma.r1.notch);

                    writer.Write(r2_pos);
                    writer.Write(enigma.r2.bytes);
                    writer.Write(enigma.r2.notch);

                    writer.Write(r3_pos);
                    writer.Write(enigma.r3.bytes);
                    writer.Write(enigma.r3.notch);

                    writer.Write(enigma.reflector.bytes);
                    writer.Write(enigma.plugboard.bytes);

                    writer.Write(xxtea);
                    writer.Write(iv);
                    writer.Write(cfb);
                }

                DodajStatusPoruku($"Keys are stored in {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ucitajKljucBtn_Click(object sender, EventArgs e)
        {
            string filePath = "";

            openFileDialog.Title = "Choose file";
            openFileDialog.Filter = "Keys file (*.key)|*.key";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            if (filePath == "")
            {
                MessageBox.Show("Keys file must be specified!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                long size = new FileInfo(filePath).Length;
                if (size != 1334)
                {
                    MessageBox.Show($"Error: The file lengths must be 1334B.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
                {
                    var r1Pos = reader.ReadByte();
                    var r1 = reader.ReadBytes(256);
                    var r1Notch = reader.ReadByte();

                    var r2Pos = reader.ReadByte();
                    var r2 = reader.ReadBytes(256);
                    var r2Notch = reader.ReadByte();

                    var r3Pos = reader.ReadByte();
                    var r3 = reader.ReadBytes(256);
                    var r3Notch = reader.ReadByte();

                    var reflector = reader.ReadBytes(256);
                    var plugboard = reader.ReadBytes(256);

                    this.xxtea = reader.ReadBytes(16);
                    this.iv = reader.ReadBytes(16);
                    this.cfb = reader.ReadBytes(16);

                    if (r1.Length != 256 || r2.Length != 256 || r3.Length != 256)
                        throw new Exception("Rotor length error");

                    if (reflector.Length != 256 || plugboard.Length != 256)
                        throw new Exception("Reflector or plugboard length error");

                    if (xxtea.Length != 16 || iv.Length != 16 || cfb.Length != 16)
                        throw new Exception("XXTEA, IV or CFB error");

                    if (reflector.Distinct().Count() != 256)
                        throw new Exception("Reflector permutation is not valid");

                    if (plugboard.Distinct().Count() != 256)
                        throw new Exception("Plugboard permutation is not valid");

                    for (int i = 0; i < 256; i++)
                    {
                        if (reflector[reflector[i]] != i)
                            throw new Exception("Reflector simetry error");
                        if (reflector[i] == i)
                            throw new Exception("Reflector must not connect the same number");
                    }

                    for (int i = 0; i < 256; i++)
                    {
                        if (plugboard[plugboard[i]] != i)
                            throw new Exception("Plugboard simetry error");
                    }

                    this.r1_pos = r1Pos;
                    this.r2_pos = r2Pos;
                    this.r3_pos = r3Pos;

                    enigma.SetConfig(r1Pos, r1, r1Notch, r2Pos, r2, r2Notch, r3Pos, r3, r3Notch, reflector, plugboard);
                }

                DodajStatusPoruku("Keys loaded");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GenerateKeys(0);
                DodajStatusPoruku("New keys genertated");
            }
        }

        private void textBoxX_TextChanged(object sender, EventArgs e)
        {
            if (chkFSW.Checked)
            {
                MessageBox.Show("X directory cannot be changed while FSW is active!");
                textBoxT.Text = watcher.Path; // Vrati staru vrednost
            }
        }


        public byte[] PrepareFileWithHeader(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);

            string encryptionAlg;
            if (rdoEnigma.Checked)
            {
                encryptionAlg = "Enigma";
            }
            else if (rdoXXTea.Checked)
            {
                encryptionAlg = "XXTEA";
            }
            else
            {
                encryptionAlg = "XXTEA (CFB)";
            }

            // 1. Kreiranje objekta sa metapodacima
            var metadata = new FileMetadata
            {
                FileName = fi.Name,
                FileSize = fi.Length,
                CreationTime = fi.CreationTime,
                EncryptionAlgorithm = encryptionAlg,
                HashAlgorithm = "TigerHash"
            };

            // 2. Serijalizacija u JSON
            string jsonString = JsonSerializer.Serialize(metadata);
            byte[] headerBytes = Encoding.UTF8.GetBytes(jsonString);

            // 3. Priprema dužine headera (4 bajta)
            byte[] headerLength = BitConverter.GetBytes(headerBytes.Length);

            // 4. Čitanje originalnog sadržaja fajla
            byte[] fileContent = File.ReadAllBytes(filePath);

            // 5. Spajanje svega u jedan paket: [Length (4b)] + [JSON Header] + [Original Data]
            byte[] combinedData = new byte[headerLength.Length + headerBytes.Length + fileContent.Length];

            Buffer.BlockCopy(headerLength, 0, combinedData, 0, headerLength.Length);
            Buffer.BlockCopy(headerBytes, 0, combinedData, headerLength.Length, headerBytes.Length);
            Buffer.BlockCopy(fileContent, 0, combinedData, headerLength.Length + headerBytes.Length, fileContent.Length);

            return combinedData;
        }

        private void btnLogDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtLogDir.Text = folderBrowserDialog.SelectedPath;
                AppState.LogDir = txtLogDir.Text;
            }
        }
    }
}
