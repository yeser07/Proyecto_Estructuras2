using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace VirtualDisk
{
    public partial class Principal : Form

    {
        public static string Default { get; set; }
        public Principal()
        {
            InitializeComponent();
        }


   
        public byte[] objectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private void Principal_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.ArchivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CrearDiscoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbrirDiscoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExploradorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ArchivoToolStripMenuItem,
            this.ExploradorToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(284, 24);
            this.Menu.TabIndex = 1;
            this.Menu.Text = "menuStrip1";
            // 
            // ArchivoToolStripMenuItem
            // 
            this.ArchivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CrearDiscoToolStripMenuItem,
            this.AbrirDiscoToolStripMenuItem});
            this.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem";
            this.ArchivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.ArchivoToolStripMenuItem.Text = "Archivo";
            // 
            // CrearDiscoToolStripMenuItem
            // 
            this.CrearDiscoToolStripMenuItem.Name = "CrearDiscoToolStripMenuItem";
            this.CrearDiscoToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.CrearDiscoToolStripMenuItem.Text = "Crear Disco";
            this.CrearDiscoToolStripMenuItem.Click += new System.EventHandler(this.CrearDiscoToolStripMenuItem_Click_1);
            // 
            // AbrirDiscoToolStripMenuItem
            // 
            this.AbrirDiscoToolStripMenuItem.Name = "AbrirDiscoToolStripMenuItem";
            this.AbrirDiscoToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.AbrirDiscoToolStripMenuItem.Text = "Abrir Disco";
            this.AbrirDiscoToolStripMenuItem.Click += new System.EventHandler(this.AbrirDiscoToolStripMenuItem_Click_1);
            // 
            // ExploradorToolStripMenuItem
            // 
            this.ExploradorToolStripMenuItem.Name = "ExploradorToolStripMenuItem";
            this.ExploradorToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.ExploradorToolStripMenuItem.Text = "Explorador";
            this.ExploradorToolStripMenuItem.Click += new System.EventHandler(this.ExploradorToolStripMenuItem_Click);
            // 
            // Principal
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Principal";
            this.Load += new System.EventHandler(this.Principal_Load_1);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CrearDiscoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            /*SaveFileDialog dlgnewFile = new SaveFileDialog();

            if (dlgnewFile.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dlgnewFile.FileName))
                {
                    FileStream fs = new FileStream(dlgnewFile.FileName, FileMode.CreateNew);
                    fs.Seek(1024 * 1024 * 1024, SeekOrigin.Begin);
                    fs.WriteByte(0);
                    fs.Close();

                    MasterBootRecord tablaMBR = new MasterBootRecord();
                    tablaMBR.DatosMBR();
                    using (BinaryWriter stream = new BinaryWriter(File.Open(dlgnewFile.FileName, FileMode.Open)))
                    {
                        stream.BaseStream.Position = 0;
                        stream.Write(tablaMBR.jumpInstruction, 0, tablaMBR.jumpInstruction.Length);
                        stream.Write(tablaMBR.oemID, 0, tablaMBR.oemID.Length);

                        stream.Write(tablaMBR.bytesxSec);
                        stream.Write(tablaMBR.sectorxCluster);
                        stream.Write(tablaMBR.reservedSectors);
                        stream.Write(tablaMBR.numberOfFATs);
                        stream.Write(tablaMBR.rootEntries);
                        stream.Write(tablaMBR.smallSectors);
                        stream.Write(tablaMBR.mediaDescriptor);
                        stream.Write(tablaMBR.sectorxFATs);
                        stream.Write(tablaMBR.sectorxTrack);
                        stream.Write(tablaMBR.numberOfHeads);
                        stream.Write(tablaMBR.hiddenSectors);
                        stream.Write(tablaMBR.largeSectors);
                        stream.Write(tablaMBR.physicalDriveNo);
                        stream.Write(tablaMBR.reserved);
                        stream.Write(tablaMBR.extBootSignature);
                        stream.Write(tablaMBR.serialNo);
                        stream.Write(tablaMBR.volumeLabel);
                        stream.Write(tablaMBR.fileSystemType);
                        stream.Write(tablaMBR.bootstrapCode, 0, tablaMBR.bootstrapCode.Length);
                        stream.Write(tablaMBR.endOfSector);
                        FAT16[] tablaFAT = new FAT16[65525];

                        for (int a = 0; a < 2; a++)
                        {
                            for (int i = 0; i < 65525; i++)
                            {
                                tablaFAT[i] = new FAT16();
                                if (i >= 2)
                                {
                                    tablaFAT[i].FreeCluster();
                                }
                                else
                                {
                                    tablaFAT[i].ReservedCluster();
                                }
                            }
                            foreach (FAT16 fentry in tablaFAT)
                            {
                                stream.Write(fentry.inputFAT);
                            }
                        }
                        for (int i = 0; i < 512; i++)
                        {
                            rootDir directorioVacio = new rootDir();
                            stream.Write(directorioVacio.filename);
                            stream.Write(directorioVacio.filenameExt);
                            stream.Write(directorioVacio.fileAttributes);
                            stream.Write(directorioVacio.NT);
                            stream.Write(directorioVacio.millisegundos_Creado);
                            stream.Write(directorioVacio.horaC);
                            stream.Write(directorioVacio.fechaC);
                            stream.Write(directorioVacio.fechaLastaccess);
                            stream.Write(directorioVacio.reservedFAT32);
                            stream.Write(directorioVacio.horaLastwrite);
                            stream.Write(directorioVacio.fechaLastwrite);
                            stream.Write(directorioVacio.startingCluster);
                            stream.Write(directorioVacio.fileSize);
                        }
                    }

                    Principal.Default = Path.GetFullPath(dlgnewFile.FileName);

                    MessageBox.Show("Operacion Exitosa, se ha creado la unidad",
                                    "Informacion",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("No se puede reemplazar el archivo!",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

            }*/

            SaveFileDialog dlgNuevoArchivo = new SaveFileDialog();
            dlgNuevoArchivo.RestoreDirectory = true;
            dlgNuevoArchivo.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (dlgNuevoArchivo.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dlgNuevoArchivo.FileName))
                {
                    FileStream fs = new FileStream(dlgNuevoArchivo.FileName, FileMode.CreateNew);
                    fs.Seek(1024 * 1024 * 1024, SeekOrigin.Begin);
                    fs.WriteByte(0);
                    fs.Close();

                    MasterBootRecord tablaMBR = new MasterBootRecord();
                    tablaMBR.DatosMBR();
                    using (BinaryWriter stream = new BinaryWriter(File.Open(dlgNuevoArchivo.FileName, FileMode.Open)))
                    {
                        //Escribir master boot record
                        stream.BaseStream.Position = 0;
                        stream.Write(tablaMBR.jumpInstruction, 0, tablaMBR.jumpInstruction.Length);
                        stream.Write(tablaMBR.oemID, 0, tablaMBR.oemID.Length);

                        stream.Write(tablaMBR.bytesxSec);
                        stream.Write(tablaMBR.sectorxCluster);
                        stream.Write(tablaMBR.reservedSectors);
                        stream.Write(tablaMBR.numberOfFATs);
                        stream.Write(tablaMBR.rootEntries);
                        stream.Write(tablaMBR.smallSectors);
                        stream.Write(tablaMBR.mediaDescriptor);
                        stream.Write(tablaMBR.sectorxFATs);
                        stream.Write(tablaMBR.sectorxTrack);
                        stream.Write(tablaMBR.numberOfHeads);
                        stream.Write(tablaMBR.hiddenSectors);
                        stream.Write(tablaMBR.largeSectors);

                        stream.Write(tablaMBR.physicalDriveNo);
                        stream.Write(tablaMBR.reserved);
                        stream.Write(tablaMBR.extBootSignature);
                        stream.Write(tablaMBR.serialNo);
                        stream.Write(tablaMBR.volumeLabel);
                        stream.Write(tablaMBR.fileSystemType);

                        stream.Write(tablaMBR.bootstrapCode, 0, tablaMBR.bootstrapCode.Length);
                        stream.Write(tablaMBR.endOfSector);
                        FAT16[] tablaFAT = new FAT16[65536];

                        for (int i = 0; i < 65536; i++)
                        {
                            tablaFAT[i] = new FAT16();
                            if (i <= 16)
                            {
                                tablaFAT[i].ReservedCluster();
                            }
                            else
                            {
                                tablaFAT[i].FreeCluster();
                            }
                        }

                        for (int a = 0; a < 2; a++)
                        {
                            foreach (FAT16 fentry in tablaFAT)
                            {
                                stream.Write(fentry.inputFAT);
                            }
                        }
                        for (int i = 0; i < 512; i++)
                        {
                            rootDir directorioVacio = new rootDir();
                            stream.Write(directorioVacio.filename);
                            stream.Write(directorioVacio.filenameExt);
                            stream.Write(directorioVacio.fileAttributes);
                            stream.Write(directorioVacio.NT);
                            stream.Write(directorioVacio.millisegundos_Creado);
                            stream.Write(directorioVacio.horaC);
                            stream.Write(directorioVacio.fechaC);
                            stream.Write(directorioVacio.fechaLastaccess);
                            stream.Write(directorioVacio.reservedFAT32);
                            stream.Write(directorioVacio.horaLastwrite);
                            stream.Write(directorioVacio.fechaLastwrite);
                            stream.Write(directorioVacio.startingCluster);
                            stream.Write(directorioVacio.fileSize);
                        }
                    }

                    Principal.Default = Path.GetFullPath(dlgNuevoArchivo.FileName);

                    MessageBox.Show("Creado Con Exito! Abra el explorador para usar el Disco","Informacion",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("No se puede reemplazar el archivo!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AbrirDiscoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpenFile = new OpenFileDialog();

            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlgOpenFile.FileName))
                {
                    Default = Path.GetFullPath(dlgOpenFile.FileName);
                }
                else
                {
                    MessageBox.Show("No se puede Abrir el archivo!",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

            }
        }

        private void ExploradorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Default != null)
            {
                if (!Application.OpenForms.OfType<Explorador>().Any())
                {
                    Explorador explorador = new Explorador();
                    //explorador.MdiParent = this;
                    explorador.WindowState = FormWindowState.Maximized;
                    explorador.Show();
                }

            }
            else
            {
                MessageBox.Show("Cargue un disco desde el menu Archivo->Abrir Disco",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        private void Principal_Load_1(object sender, EventArgs e)
        {

        }
    }
}
