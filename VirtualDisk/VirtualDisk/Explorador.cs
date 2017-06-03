using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace VirtualDisk
{
    public partial class Explorador : Form
    {
        public List<FAT16> fat1List = new List<FAT16>();
        public List<FAT16> fat2List = new List<FAT16>();

        public Explorador()
        {
            InitializeComponent();
        }

        private void Explorador_Load_1(object sender, EventArgs e)
        {
            if (Principal.Default == null)
            {
                MessageBox.Show("Abra un disco o cree un nuevo desde el menu archivo",
                                   "Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                Disco.Text = "Disco: " + Principal.Default;
                readHDDInformation();
            }

        }

        public void readHDDInformation()
        {
            DecodedMBR tablaMBR = new DecodedMBR();
            byte[] temporalArray;
            using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                temporalArray = reader.ReadBytes(3);
                tablaMBR.jumpInstruction = temporalArray;
                temporalArray = new byte[8];
                temporalArray = reader.ReadBytes(8);
                tablaMBR.oemID = Encoding.ASCII.GetString(temporalArray);
                tablaMBR.bytesxSec = reader.ReadInt16();
                tablaMBR.sectorxCluster = reader.ReadByte();
                tablaMBR.reservedSectors = reader.ReadInt16();
                tablaMBR.numberOfFATs = reader.ReadByte();
                tablaMBR.rootEntries = reader.ReadInt16();
                tablaMBR.smallSectors = reader.ReadInt16();
                tablaMBR.mediaDescriptor = reader.ReadByte();
                tablaMBR.sectorxFATs = reader.ReadInt16();
                tablaMBR.sectorxTrack = reader.ReadInt16();
                tablaMBR.numberOfHeads = reader.ReadInt16();
                tablaMBR.hiddenSectors = reader.ReadInt32();
                tablaMBR.largeSectors = reader.ReadInt32();
                tablaMBR.physicalDriveNo = reader.ReadByte();
                tablaMBR.reserved = reader.ReadByte();
                tablaMBR.extBootSignature = reader.ReadByte();
                tablaMBR.serialNo = reader.ReadInt32();
                temporalArray = new byte[11];
                temporalArray = reader.ReadBytes(11);
                tablaMBR.volumeLabel = Encoding.ASCII.GetString(temporalArray);
                temporalArray = new byte[8];
                temporalArray = reader.ReadBytes(8);
                tablaMBR.fileSystemType = Encoding.ASCII.GetString(temporalArray);
                tablaMBR.bootstrapCode = reader.ReadBytes(448);
                tablaMBR.endOfSector = reader.ReadInt16();

                ////leer 2 tablas FAT

                for (int i = 0; i < 65525; i++)
                {
                    FAT16 fatentry = new FAT16();
                    fatentry.inputFAT = reader.ReadUInt16();
                    fat1List.Add(fatentry);
                }

                for (int i = 0; i < 65525; i++)
                {
                    FAT16 fatentry = new FAT16();
                    fatentry.inputFAT = reader.ReadUInt16();
                    fat2List.Add(fatentry);
                }

             
            }

            LstPropDisco.Items.Add("OEM ID " + tablaMBR.oemID);
            LstPropDisco.Items.Add("Bytes por Sector: " + tablaMBR.bytesxSec);
            LstPropDisco.Items.Add("Sectores por Cluster: " + tablaMBR.sectorxCluster);
            LstPropDisco.Items.Add("Sectores Reservados: " + tablaMBR.reservedSectors);
            LstPropDisco.Items.Add("Numero de FATs: " + tablaMBR.numberOfFATs);
            LstPropDisco.Items.Add("Entradas directorio Root: " + tablaMBR.rootEntries);
            LstPropDisco.Items.Add("Tipo de Disco: " + tablaMBR.mediaDescriptor);
            LstPropDisco.Items.Add("Sectores por FAT: " + tablaMBR.sectorxFATs);
            LstPropDisco.Items.Add("Sectores Ocultos: " + tablaMBR.hiddenSectors);
            LstPropDisco.Items.Add("Total Sectores: " + tablaMBR.largeSectors);
            LstPropDisco.Items.Add("Numero Serie: " + tablaMBR.serialNo);
            LstPropDisco.Items.Add("Etiqueta de Volumen: " + tablaMBR.volumeLabel);
            LstPropDisco.Items.Add("Formato de Volumen: " + tablaMBR.fileSystemType);
            LstPropDisco.Items.Add("Espacio Libre: " + EspacioLibre() + " KB");
        }

        public int EspacioLibre()
        {
            return 0;
        }

        
    }
}
