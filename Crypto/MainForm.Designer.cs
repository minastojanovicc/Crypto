
using System.Windows.Forms;

namespace Crypto
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBoxAlgoritmi = new GroupBox();
            txtR3 = new TextBox();
            txtR2 = new TextBox();
            txtR1 = new TextBox();
            label2 = new Label();
            rdoCFB = new RadioButton();
            rdoXXTea = new RadioButton();
            rdoEnigma = new RadioButton();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            pageSetings = new TabPage();
            btnLogDir = new Button();
            txtLogDir = new TextBox();
            label4 = new Label();
            chkFSW = new CheckBox();
            groupBoxDirektorijumi = new GroupBox();
            buttonOdaberiX = new Button();
            buttonOdaberiT = new Button();
            textBoxX = new TextBox();
            textBoxT = new TextBox();
            labelX = new Label();
            labelTarget = new Label();
            groupBoxKljucevi = new GroupBox();
            lblNewSeed = new Label();
            txtNewSeed = new TextBox();
            ucitajKljucBtn = new Button();
            sacuvajKljucBtn = new Button();
            generisiKljucBtn = new Button();
            pageCommunication = new TabPage();
            groupBox2 = new GroupBox();
            label1 = new Label();
            txtPortServer = new TextBox();
            btnStartServer = new Button();
            groupBox1 = new GroupBox();
            labelFajl = new Label();
            textBoxKlijent = new TextBox();
            buttonIzaberiFajl = new Button();
            buttonPosalji = new Button();
            labelIP = new Label();
            labelPort = new Label();
            textBoxPortKlijent = new TextBox();
            textBoxIPKlijent = new TextBox();
            pageDecryption = new TabPage();
            buttonDekodiraj = new Button();
            buttonIzaberiDir = new Button();
            buttonUcitajDekodiranje = new Button();
            textBoxIzlazDekod = new TextBox();
            textBoxUlazDekod = new TextBox();
            label3 = new Label();
            labelUlazniDekod = new Label();
            pageEncryption = new TabPage();
            label7 = new Label();
            lblOutDir = new Label();
            label5 = new Label();
            btnEncrypt = new Button();
            txtFIleEncryption = new TextBox();
            lblFileEncryption = new Label();
            btnOpenFile = new Button();
            tabEncryption = new TabControl();
            groupBoxAlgoritmi.SuspendLayout();
            statusStrip.SuspendLayout();
            pageSetings.SuspendLayout();
            groupBoxDirektorijumi.SuspendLayout();
            groupBoxKljucevi.SuspendLayout();
            pageCommunication.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            pageDecryption.SuspendLayout();
            pageEncryption.SuspendLayout();
            tabEncryption.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxAlgoritmi
            // 
            groupBoxAlgoritmi.Controls.Add(txtR3);
            groupBoxAlgoritmi.Controls.Add(txtR2);
            groupBoxAlgoritmi.Controls.Add(txtR1);
            groupBoxAlgoritmi.Controls.Add(label2);
            groupBoxAlgoritmi.Controls.Add(rdoCFB);
            groupBoxAlgoritmi.Controls.Add(rdoXXTea);
            groupBoxAlgoritmi.Controls.Add(rdoEnigma);
            groupBoxAlgoritmi.Location = new Point(45, 30);
            groupBoxAlgoritmi.Margin = new Padding(3, 4, 3, 4);
            groupBoxAlgoritmi.Name = "groupBoxAlgoritmi";
            groupBoxAlgoritmi.Padding = new Padding(3, 4, 3, 4);
            groupBoxAlgoritmi.Size = new Size(590, 111);
            groupBoxAlgoritmi.TabIndex = 10;
            groupBoxAlgoritmi.TabStop = false;
            groupBoxAlgoritmi.Text = "Algorithm";
            // 
            // txtR3
            // 
            txtR3.Location = new Point(216, 74);
            txtR3.Name = "txtR3";
            txtR3.Size = new Size(42, 27);
            txtR3.TabIndex = 6;
            txtR3.Text = "0";
            txtR3.TextAlign = HorizontalAlignment.Center;
            // 
            // txtR2
            // 
            txtR2.Location = new Point(151, 76);
            txtR2.Name = "txtR2";
            txtR2.Size = new Size(42, 27);
            txtR2.TabIndex = 5;
            txtR2.Text = "0";
            txtR2.TextAlign = HorizontalAlignment.Center;
            // 
            // txtR1
            // 
            txtR1.Location = new Point(82, 77);
            txtR1.Name = "txtR1";
            txtR1.Size = new Size(44, 27);
            txtR1.TabIndex = 4;
            txtR1.Text = "0";
            txtR1.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 50);
            label2.Name = "label2";
            label2.Size = new Size(247, 20);
            label2.TabIndex = 3;
            label2.Text = "Rotors           R1             R2            R3";
            // 
            // rdoCFB
            // 
            rdoCFB.AutoSize = true;
            rdoCFB.Location = new Point(388, 71);
            rdoCFB.Margin = new Padding(3, 4, 3, 4);
            rdoCFB.Name = "rdoCFB";
            rdoCFB.Size = new Size(148, 24);
            rdoCFB.TabIndex = 2;
            rdoCFB.TabStop = true;
            rdoCFB.Text = "XXTEA (CFB mod)";
            rdoCFB.UseVisualStyleBackColor = true;
            // 
            // rdoXXTea
            // 
            rdoXXTea.AutoSize = true;
            rdoXXTea.Location = new Point(390, 26);
            rdoXXTea.Margin = new Padding(3, 4, 3, 4);
            rdoXXTea.Name = "rdoXXTea";
            rdoXXTea.Size = new Size(74, 24);
            rdoXXTea.TabIndex = 1;
            rdoXXTea.TabStop = true;
            rdoXXTea.Text = "XXTEA";
            rdoXXTea.UseVisualStyleBackColor = true;
            // 
            // rdoEnigma
            // 
            rdoEnigma.AutoSize = true;
            rdoEnigma.Checked = true;
            rdoEnigma.Location = new Point(116, 18);
            rdoEnigma.Margin = new Padding(3, 4, 3, 4);
            rdoEnigma.Name = "rdoEnigma";
            rdoEnigma.Size = new Size(80, 24);
            rdoEnigma.TabIndex = 0;
            rdoEnigma.TabStop = true;
            rdoEnigma.Text = "Enigma";
            rdoEnigma.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 556);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(833, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 16);
            // 
            // pageSetings
            // 
            pageSetings.Controls.Add(btnLogDir);
            pageSetings.Controls.Add(txtLogDir);
            pageSetings.Controls.Add(label4);
            pageSetings.Controls.Add(chkFSW);
            pageSetings.Controls.Add(groupBoxDirektorijumi);
            pageSetings.Controls.Add(groupBoxKljucevi);
            pageSetings.Location = new Point(4, 29);
            pageSetings.Margin = new Padding(3, 4, 3, 4);
            pageSetings.Name = "pageSetings";
            pageSetings.Padding = new Padding(3, 4, 3, 4);
            pageSetings.Size = new Size(813, 363);
            pageSetings.TabIndex = 4;
            pageSetings.Text = "Settings";
            pageSetings.UseVisualStyleBackColor = true;
            // 
            // btnLogDir
            // 
            btnLogDir.Location = new Point(694, 27);
            btnLogDir.Name = "btnLogDir";
            btnLogDir.Size = new Size(94, 37);
            btnLogDir.TabIndex = 15;
            btnLogDir.Text = "Select";
            btnLogDir.UseVisualStyleBackColor = true;
            btnLogDir.Click += btnLogDir_Click;
            // 
            // txtLogDir
            // 
            txtLogDir.Location = new Point(355, 29);
            txtLogDir.Name = "txtLogDir";
            txtLogDir.Size = new Size(319, 27);
            txtLogDir.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(302, 29);
            label4.Name = "label4";
            label4.Size = new Size(34, 20);
            label4.TabIndex = 13;
            label4.Text = "Log";
            // 
            // chkFSW
            // 
            chkFSW.AutoSize = true;
            chkFSW.Location = new Point(58, 25);
            chkFSW.Margin = new Padding(3, 4, 3, 4);
            chkFSW.Name = "chkFSW";
            chkFSW.Size = new Size(163, 24);
            chkFSW.TabIndex = 12;
            chkFSW.Text = "File System Watcher";
            chkFSW.UseVisualStyleBackColor = true;
            chkFSW.CheckedChanged += chkFSW_CheckedChanged;
            // 
            // groupBoxDirektorijumi
            // 
            groupBoxDirektorijumi.Controls.Add(buttonOdaberiX);
            groupBoxDirektorijumi.Controls.Add(buttonOdaberiT);
            groupBoxDirektorijumi.Controls.Add(textBoxX);
            groupBoxDirektorijumi.Controls.Add(textBoxT);
            groupBoxDirektorijumi.Controls.Add(labelX);
            groupBoxDirektorijumi.Controls.Add(labelTarget);
            groupBoxDirektorijumi.Location = new Point(58, 71);
            groupBoxDirektorijumi.Margin = new Padding(3, 4, 3, 4);
            groupBoxDirektorijumi.Name = "groupBoxDirektorijumi";
            groupBoxDirektorijumi.Padding = new Padding(3, 4, 3, 4);
            groupBoxDirektorijumi.Size = new Size(715, 163);
            groupBoxDirektorijumi.TabIndex = 11;
            groupBoxDirektorijumi.TabStop = false;
            groupBoxDirektorijumi.Text = "FSW directories";
            // 
            // buttonOdaberiX
            // 
            buttonOdaberiX.Location = new Point(536, 104);
            buttonOdaberiX.Margin = new Padding(3, 4, 3, 4);
            buttonOdaberiX.Name = "buttonOdaberiX";
            buttonOdaberiX.Size = new Size(147, 31);
            buttonOdaberiX.TabIndex = 5;
            buttonOdaberiX.Text = "Select";
            buttonOdaberiX.UseVisualStyleBackColor = true;
            buttonOdaberiX.Click += buttonOdaberiX_Click;
            // 
            // buttonOdaberiT
            // 
            buttonOdaberiT.Location = new Point(536, 55);
            buttonOdaberiT.Margin = new Padding(3, 4, 3, 4);
            buttonOdaberiT.Name = "buttonOdaberiT";
            buttonOdaberiT.Size = new Size(147, 31);
            buttonOdaberiT.TabIndex = 4;
            buttonOdaberiT.Text = "Select";
            buttonOdaberiT.UseVisualStyleBackColor = true;
            buttonOdaberiT.Click += buttonOdaberiT_Click;
            // 
            // textBoxX
            // 
            textBoxX.Location = new Point(157, 104);
            textBoxX.Margin = new Padding(3, 4, 3, 4);
            textBoxX.Name = "textBoxX";
            textBoxX.ReadOnly = true;
            textBoxX.Size = new Size(363, 27);
            textBoxX.TabIndex = 3;
            textBoxX.TextChanged += textBoxX_TextChanged;
            // 
            // textBoxT
            // 
            textBoxT.Location = new Point(157, 56);
            textBoxT.Margin = new Padding(3, 4, 3, 4);
            textBoxT.Name = "textBoxT";
            textBoxT.ReadOnly = true;
            textBoxT.Size = new Size(363, 27);
            textBoxT.TabIndex = 2;
            // 
            // labelX
            // 
            labelX.AutoSize = true;
            labelX.Location = new Point(18, 104);
            labelX.Name = "labelX";
            labelX.Size = new Size(81, 20);
            labelX.TabIndex = 1;
            labelX.Text = "X directory";
            // 
            // labelTarget
            // 
            labelTarget.AutoSize = true;
            labelTarget.Location = new Point(18, 60);
            labelTarget.Name = "labelTarget";
            labelTarget.Size = new Size(113, 20);
            labelTarget.TabIndex = 0;
            labelTarget.Text = "Target directory";
            // 
            // groupBoxKljucevi
            // 
            groupBoxKljucevi.Controls.Add(lblNewSeed);
            groupBoxKljucevi.Controls.Add(txtNewSeed);
            groupBoxKljucevi.Controls.Add(ucitajKljucBtn);
            groupBoxKljucevi.Controls.Add(sacuvajKljucBtn);
            groupBoxKljucevi.Controls.Add(generisiKljucBtn);
            groupBoxKljucevi.Location = new Point(58, 241);
            groupBoxKljucevi.Name = "groupBoxKljucevi";
            groupBoxKljucevi.Padding = new Padding(6, 4, 6, 4);
            groupBoxKljucevi.Size = new Size(699, 115);
            groupBoxKljucevi.TabIndex = 2;
            groupBoxKljucevi.TabStop = false;
            groupBoxKljucevi.Text = "Keys";
            // 
            // lblNewSeed
            // 
            lblNewSeed.AutoSize = true;
            lblNewSeed.Location = new Point(360, 34);
            lblNewSeed.Name = "lblNewSeed";
            lblNewSeed.Size = new Size(74, 20);
            lblNewSeed.TabIndex = 4;
            lblNewSeed.Text = "New seed";
            // 
            // txtNewSeed
            // 
            txtNewSeed.Location = new Point(461, 27);
            txtNewSeed.Name = "txtNewSeed";
            txtNewSeed.Size = new Size(147, 27);
            txtNewSeed.TabIndex = 3;
            // 
            // ucitajKljucBtn
            // 
            ucitajKljucBtn.Location = new Point(70, 68);
            ucitajKljucBtn.Name = "ucitajKljucBtn";
            ucitajKljucBtn.Size = new Size(171, 31);
            ucitajKljucBtn.TabIndex = 2;
            ucitajKljucBtn.Text = "Import keys from file";
            ucitajKljucBtn.UseVisualStyleBackColor = true;
            ucitajKljucBtn.Click += ucitajKljucBtn_Click;
            // 
            // sacuvajKljucBtn
            // 
            sacuvajKljucBtn.Location = new Point(83, 23);
            sacuvajKljucBtn.Name = "sacuvajKljucBtn";
            sacuvajKljucBtn.Size = new Size(147, 31);
            sacuvajKljucBtn.TabIndex = 1;
            sacuvajKljucBtn.Text = "Export keys to file";
            sacuvajKljucBtn.UseVisualStyleBackColor = true;
            sacuvajKljucBtn.Click += sacuvajKljucBtn_Click;
            // 
            // generisiKljucBtn
            // 
            generisiKljucBtn.Location = new Point(461, 68);
            generisiKljucBtn.Name = "generisiKljucBtn";
            generisiKljucBtn.Size = new Size(147, 31);
            generisiKljucBtn.TabIndex = 0;
            generisiKljucBtn.Text = "Generate new keys";
            generisiKljucBtn.UseVisualStyleBackColor = true;
            generisiKljucBtn.Click += generisiKljucBtn_Click;
            // 
            // pageCommunication
            // 
            pageCommunication.Controls.Add(groupBox2);
            pageCommunication.Controls.Add(groupBox1);
            pageCommunication.Location = new Point(4, 29);
            pageCommunication.Margin = new Padding(3, 4, 3, 4);
            pageCommunication.Name = "pageCommunication";
            pageCommunication.Padding = new Padding(3, 4, 3, 4);
            pageCommunication.Size = new Size(813, 363);
            pageCommunication.TabIndex = 1;
            pageCommunication.Text = "Communication";
            pageCommunication.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(txtPortServer);
            groupBox2.Controls.Add(btnStartServer);
            groupBox2.Location = new Point(41, 255);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(726, 84);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Server Settings";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(106, 30);
            label1.Name = "label1";
            label1.Size = new Size(35, 20);
            label1.TabIndex = 11;
            label1.Text = "Port";
            // 
            // txtPortServer
            // 
            txtPortServer.Cursor = Cursors.IBeam;
            txtPortServer.Location = new Point(179, 27);
            txtPortServer.Margin = new Padding(3, 4, 3, 4);
            txtPortServer.Name = "txtPortServer";
            txtPortServer.Size = new Size(158, 27);
            txtPortServer.TabIndex = 10;
            txtPortServer.Text = "5000";
            txtPortServer.TextAlign = HorizontalAlignment.Center;
            // 
            // btnStartServer
            // 
            btnStartServer.Location = new Point(399, 25);
            btnStartServer.Margin = new Padding(3, 4, 3, 4);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(147, 31);
            btnStartServer.TabIndex = 9;
            btnStartServer.Text = "Start server";
            btnStartServer.UseVisualStyleBackColor = true;
            btnStartServer.Click += buttonServer_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(labelFajl);
            groupBox1.Controls.Add(textBoxKlijent);
            groupBox1.Controls.Add(buttonIzaberiFajl);
            groupBox1.Controls.Add(buttonPosalji);
            groupBox1.Controls.Add(labelIP);
            groupBox1.Controls.Add(labelPort);
            groupBox1.Controls.Add(textBoxPortKlijent);
            groupBox1.Controls.Add(textBoxIPKlijent);
            groupBox1.Location = new Point(41, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(727, 216);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Client Settings";
            // 
            // labelFajl
            // 
            labelFajl.AutoSize = true;
            labelFajl.Location = new Point(95, 120);
            labelFajl.Name = "labelFajl";
            labelFajl.Size = new Size(32, 20);
            labelFajl.TabIndex = 7;
            labelFajl.Text = "File";
            // 
            // textBoxKlijent
            // 
            textBoxKlijent.Location = new Point(160, 117);
            textBoxKlijent.Margin = new Padding(3, 4, 3, 4);
            textBoxKlijent.Name = "textBoxKlijent";
            textBoxKlijent.ReadOnly = true;
            textBoxKlijent.Size = new Size(376, 27);
            textBoxKlijent.TabIndex = 6;
            // 
            // buttonIzaberiFajl
            // 
            buttonIzaberiFajl.Location = new Point(557, 117);
            buttonIzaberiFajl.Margin = new Padding(3, 4, 3, 4);
            buttonIzaberiFajl.Name = "buttonIzaberiFajl";
            buttonIzaberiFajl.Size = new Size(147, 31);
            buttonIzaberiFajl.TabIndex = 5;
            buttonIzaberiFajl.Text = "Open";
            buttonIzaberiFajl.UseVisualStyleBackColor = true;
            buttonIzaberiFajl.Click += buttonIzaberiFajl_Click;
            // 
            // buttonPosalji
            // 
            buttonPosalji.Location = new Point(160, 168);
            buttonPosalji.Margin = new Padding(3, 4, 3, 4);
            buttonPosalji.Name = "buttonPosalji";
            buttonPosalji.Size = new Size(266, 31);
            buttonPosalji.TabIndex = 4;
            buttonPosalji.Text = "Send";
            buttonPosalji.UseVisualStyleBackColor = true;
            buttonPosalji.Click += buttonPosalji_Click;
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new Point(46, 34);
            labelIP.Name = "labelIP";
            labelIP.Size = new Size(76, 20);
            labelIP.TabIndex = 3;
            labelIP.Text = "IP address";
            // 
            // labelPort
            // 
            labelPort.AutoSize = true;
            labelPort.Location = new Point(87, 76);
            labelPort.Name = "labelPort";
            labelPort.Size = new Size(35, 20);
            labelPort.TabIndex = 2;
            labelPort.Text = "Port";
            // 
            // textBoxPortKlijent
            // 
            textBoxPortKlijent.Cursor = Cursors.IBeam;
            textBoxPortKlijent.Location = new Point(160, 73);
            textBoxPortKlijent.Margin = new Padding(3, 4, 3, 4);
            textBoxPortKlijent.Name = "textBoxPortKlijent";
            textBoxPortKlijent.Size = new Size(158, 27);
            textBoxPortKlijent.TabIndex = 1;
            textBoxPortKlijent.Text = "5000";
            textBoxPortKlijent.TextAlign = HorizontalAlignment.Center;
            // 
            // textBoxIPKlijent
            // 
            textBoxIPKlijent.Cursor = Cursors.IBeam;
            textBoxIPKlijent.Location = new Point(160, 31);
            textBoxIPKlijent.Margin = new Padding(3, 4, 3, 4);
            textBoxIPKlijent.Name = "textBoxIPKlijent";
            textBoxIPKlijent.Size = new Size(158, 27);
            textBoxIPKlijent.TabIndex = 0;
            textBoxIPKlijent.Text = "127.0.0.1";
            textBoxIPKlijent.TextAlign = HorizontalAlignment.Center;
            // 
            // pageDecryption
            // 
            pageDecryption.Controls.Add(buttonDekodiraj);
            pageDecryption.Controls.Add(buttonIzaberiDir);
            pageDecryption.Controls.Add(buttonUcitajDekodiranje);
            pageDecryption.Controls.Add(textBoxIzlazDekod);
            pageDecryption.Controls.Add(textBoxUlazDekod);
            pageDecryption.Controls.Add(label3);
            pageDecryption.Controls.Add(labelUlazniDekod);
            pageDecryption.Location = new Point(4, 29);
            pageDecryption.Margin = new Padding(3, 4, 3, 4);
            pageDecryption.Name = "pageDecryption";
            pageDecryption.Padding = new Padding(3, 4, 3, 4);
            pageDecryption.Size = new Size(813, 363);
            pageDecryption.TabIndex = 3;
            pageDecryption.Text = "Decryption";
            pageDecryption.UseVisualStyleBackColor = true;
            // 
            // buttonDekodiraj
            // 
            buttonDekodiraj.Location = new Point(229, 232);
            buttonDekodiraj.Margin = new Padding(3, 4, 3, 4);
            buttonDekodiraj.Name = "buttonDekodiraj";
            buttonDekodiraj.Size = new Size(266, 31);
            buttonDekodiraj.TabIndex = 6;
            buttonDekodiraj.Text = "Decrypt";
            buttonDekodiraj.UseVisualStyleBackColor = true;
            buttonDekodiraj.Click += buttonDekodiraj_Click;
            // 
            // buttonIzaberiDir
            // 
            buttonIzaberiDir.Location = new Point(627, 146);
            buttonIzaberiDir.Margin = new Padding(3, 4, 3, 4);
            buttonIzaberiDir.Name = "buttonIzaberiDir";
            buttonIzaberiDir.Size = new Size(147, 31);
            buttonIzaberiDir.TabIndex = 5;
            buttonIzaberiDir.Text = "Select";
            buttonIzaberiDir.UseVisualStyleBackColor = true;
            buttonIzaberiDir.Click += buttonIzaberiDir_Click;
            // 
            // buttonUcitajDekodiranje
            // 
            buttonUcitajDekodiranje.Location = new Point(625, 93);
            buttonUcitajDekodiranje.Margin = new Padding(3, 4, 3, 4);
            buttonUcitajDekodiranje.Name = "buttonUcitajDekodiranje";
            buttonUcitajDekodiranje.Size = new Size(147, 31);
            buttonUcitajDekodiranje.TabIndex = 4;
            buttonUcitajDekodiranje.Text = "Open";
            buttonUcitajDekodiranje.UseVisualStyleBackColor = true;
            buttonUcitajDekodiranje.Click += buttonUcitajDekodiranje_Click;
            // 
            // textBoxIzlazDekod
            // 
            textBoxIzlazDekod.Location = new Point(205, 146);
            textBoxIzlazDekod.Margin = new Padding(3, 4, 3, 4);
            textBoxIzlazDekod.Name = "textBoxIzlazDekod";
            textBoxIzlazDekod.ReadOnly = true;
            textBoxIzlazDekod.Size = new Size(396, 27);
            textBoxIzlazDekod.TabIndex = 3;
            // 
            // textBoxUlazDekod
            // 
            textBoxUlazDekod.Location = new Point(205, 94);
            textBoxUlazDekod.Margin = new Padding(3, 4, 3, 4);
            textBoxUlazDekod.Name = "textBoxUlazDekod";
            textBoxUlazDekod.ReadOnly = true;
            textBoxUlazDekod.Size = new Size(396, 27);
            textBoxUlazDekod.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 151);
            label3.Name = "label3";
            label3.Size = new Size(141, 20);
            label3.TabIndex = 2;
            label3.Text = "Decrypted directory";
            // 
            // labelUlazniDekod
            // 
            labelUlazniDekod.AutoSize = true;
            labelUlazniDekod.Location = new Point(86, 97);
            labelUlazniDekod.Name = "labelUlazniDekod";
            labelUlazniDekod.Size = new Size(100, 20);
            labelUlazniDekod.TabIndex = 0;
            labelUlazniDekod.Text = "Encrypted file";
            // 
            // pageEncryption
            // 
            pageEncryption.Controls.Add(label7);
            pageEncryption.Controls.Add(lblOutDir);
            pageEncryption.Controls.Add(label5);
            pageEncryption.Controls.Add(btnEncrypt);
            pageEncryption.Controls.Add(txtFIleEncryption);
            pageEncryption.Controls.Add(lblFileEncryption);
            pageEncryption.Controls.Add(btnOpenFile);
            pageEncryption.Location = new Point(4, 29);
            pageEncryption.Margin = new Padding(3, 4, 3, 4);
            pageEncryption.Name = "pageEncryption";
            pageEncryption.Padding = new Padding(3, 4, 3, 4);
            pageEncryption.Size = new Size(813, 363);
            pageEncryption.TabIndex = 0;
            pageEncryption.Text = "Encryption";
            pageEncryption.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(626, 143);
            label7.Name = "label7";
            label7.Size = new Size(142, 20);
            label7.TabIndex = 9;
            label7.Text = "(Change in Settings)";
            // 
            // lblOutDir
            // 
            lblOutDir.AutoSize = true;
            lblOutDir.Location = new Point(157, 143);
            lblOutDir.Name = "lblOutDir";
            lblOutDir.Size = new Size(116, 20);
            lblOutDir.TabIndex = 8;
            lblOutDir.Text = "output directory";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 143);
            label5.Name = "label5";
            label5.Size = new Size(118, 20);
            label5.TabIndex = 7;
            label5.Text = "Output directory";
            // 
            // btnEncrypt
            // 
            btnEncrypt.Location = new Point(157, 197);
            btnEncrypt.Margin = new Padding(3, 4, 3, 4);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new Size(266, 31);
            btnEncrypt.TabIndex = 6;
            btnEncrypt.Text = "Encrypt";
            btnEncrypt.UseVisualStyleBackColor = true;
            btnEncrypt.Click += enkriptujDugme_Click;
            // 
            // txtFIleEncryption
            // 
            txtFIleEncryption.Location = new Point(157, 85);
            txtFIleEncryption.Margin = new Padding(3, 4, 3, 4);
            txtFIleEncryption.Name = "txtFIleEncryption";
            txtFIleEncryption.ReadOnly = true;
            txtFIleEncryption.Size = new Size(436, 27);
            txtFIleEncryption.TabIndex = 4;
            // 
            // lblFileEncryption
            // 
            lblFileEncryption.AutoSize = true;
            lblFileEncryption.Location = new Point(32, 88);
            lblFileEncryption.Name = "lblFileEncryption";
            lblFileEncryption.Size = new Size(106, 20);
            lblFileEncryption.TabIndex = 2;
            lblFileEncryption.Text = "File encryption";
            // 
            // btnOpenFile
            // 
            btnOpenFile.Location = new Point(626, 83);
            btnOpenFile.Margin = new Padding(3, 4, 3, 4);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(147, 31);
            btnOpenFile.TabIndex = 0;
            btnOpenFile.Text = "Open";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += ucitajFajlDugme_Click;
            // 
            // tabEncryption
            // 
            tabEncryption.Controls.Add(pageEncryption);
            tabEncryption.Controls.Add(pageDecryption);
            tabEncryption.Controls.Add(pageCommunication);
            tabEncryption.Controls.Add(pageSetings);
            tabEncryption.Location = new Point(0, 157);
            tabEncryption.Margin = new Padding(0);
            tabEncryption.Name = "tabEncryption";
            tabEncryption.Padding = new Point(0, 0);
            tabEncryption.SelectedIndex = 0;
            tabEncryption.Size = new Size(821, 396);
            tabEncryption.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(833, 578);
            Controls.Add(statusStrip);
            Controls.Add(tabEncryption);
            Controls.Add(groupBoxAlgoritmi);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Crypto Project";
            FormClosing += MainForm_FormClosing;
            groupBoxAlgoritmi.ResumeLayout(false);
            groupBoxAlgoritmi.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            pageSetings.ResumeLayout(false);
            pageSetings.PerformLayout();
            groupBoxDirektorijumi.ResumeLayout(false);
            groupBoxDirektorijumi.PerformLayout();
            groupBoxKljucevi.ResumeLayout(false);
            groupBoxKljucevi.PerformLayout();
            pageCommunication.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            pageDecryption.ResumeLayout(false);
            pageDecryption.PerformLayout();
            pageEncryption.ResumeLayout(false);
            pageEncryption.PerformLayout();
            tabEncryption.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private GroupBox groupBoxAlgoritmi;
        private RadioButton rdoCFB;
        private RadioButton rdoXXTea;
        private RadioButton rdoEnigma;
        private FolderBrowserDialog folderBrowserDialog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private TabPage pageSetings;
        private CheckBox chkFSW;
        private GroupBox groupBoxDirektorijumi;
        private Button buttonOdaberiX;
        private Button buttonOdaberiT;
        private TextBox textBoxX;
        private TextBox textBoxT;
        private Label labelX;
        private Label labelTarget;
        private GroupBox groupBoxKljucevi;
        private Button ucitajKljucBtn;
        private Button sacuvajKljucBtn;
        private Button generisiKljucBtn;
        private TabPage pageCommunication;
        private GroupBox groupBox2;
        private Label label1;
        private TextBox txtPortServer;
        private Button btnStartServer;
        private GroupBox groupBox1;
        private Label labelFajl;
        private TextBox textBoxKlijent;
        private Button buttonIzaberiFajl;
        private Button buttonPosalji;
        private Label labelIP;
        private Label labelPort;
        private TextBox textBoxPortKlijent;
        private TextBox textBoxIPKlijent;
        private TabPage pageDecryption;
        private Button buttonDekodiraj;
        private Button buttonIzaberiDir;
        private Button buttonUcitajDekodiranje;
        private TextBox textBoxIzlazDekod;
        private TextBox textBoxUlazDekod;
        private Label label3;
        private Label labelUlazniDekod;
        protected internal TabPage pageEncryption;
        private Button btnEncrypt;
        private TextBox txtFIleEncryption;
        private Label lblFileEncryption;
        private Button btnOpenFile;
        private TabControl tabEncryption;
        private Label lblNewSeed;
        private TextBox txtNewSeed;
        private TextBox txtR3;
        private TextBox txtR2;
        private TextBox txtR1;
        private Label label2;
        private Button btnLogDir;
        private TextBox txtLogDir;
        private Label label4;
        private Label label7;
        private Label lblOutDir;
        private Label label5;
    }
}
