using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    [Serializable]
    public class MasterBootRecord
    {
        public byte[] jumpInstruction = new byte[3];
        public byte[] oemID = new byte[8];
        public short bytesxSec = new short();
        public byte sectorxCluster = new byte();
        public short reservedSectors = new short();
        public byte numberOfFATs = new byte();
        public short rootEntries = new short();
        public short smallSectors = new short();
        public byte mediaDescriptor = new byte();
        public short sectorxFATs = new short();
        public short sectorxTrack = new short();
        public short numberOfHeads = new short();
        public int hiddenSectors = new int();
        public int largeSectors = new int();
        public byte physicalDriveNo = new byte();
        public byte reserved = new byte();
        public byte extBootSignature = new byte();
        public int serialNo = new int();
        public byte[] volumeLabel = new byte[11];
        public byte[] fileSystemType = new byte[8];
        public byte[] bootstrapCode = new byte[448];
        public short endOfSector = new short();
        public MasterBootRecord()
        {

        }
        public void DatosMBR()
        {
            oemID = Encoding.ASCII.GetBytes("MSWIN4.1");
            bytesxSec = 512;
            sectorxCluster = 32;
            reservedSectors = 2;
            numberOfFATs = 1;
            rootEntries = 512;
            smallSectors = 0;
            mediaDescriptor = 0xf8;
            sectorxFATs = 256;
            sectorxTrack = 0;
            numberOfHeads = 0;
            hiddenSectors = 0;
            largeSectors = 2097152;
            physicalDriveNo = 1;
            reserved = 0;
            extBootSignature = 29;
            serialNo = 1995123;
            byte[] vlabel = Encoding.ASCII.GetBytes("DiscoVirtual");
            Array.Resize<byte>(ref vlabel, 11);
            volumeLabel = vlabel;
            byte[] fstype = Encoding.ASCII.GetBytes("FAT16");
            Array.Resize<byte>(ref fstype, 8);
            fileSystemType = fstype;
            endOfSector = 1;
        }
    }

    public class DecodedMBR
    {
        public byte[] jumpInstruction { get; set; }
        public string oemID { get; set; }
        ///BPB
        public short bytesxSec { get; set; }
        public byte sectorxCluster { get; set; }
        public short reservedSectors { get; set; }
        public byte numberOfFATs { get; set; }
        public short rootEntries { get; set; }
        public short smallSectors { get; set; }
        public byte mediaDescriptor { get; set; }
        public short sectorxFATs { get; set; }
        public short sectorxTrack { get; set; }
        public short numberOfHeads { get; set; }
        public int hiddenSectors { get; set; }
        public int largeSectors { get; set; }
        public byte physicalDriveNo { get; set; }
        public byte reserved { get; set; }
        public byte extBootSignature { get; set; }
        public int serialNo { get; set; }
        public string volumeLabel { get; set; }
        public string fileSystemType { get; set; }
        public byte[] bootstrapCode { get; set; }
        public short endOfSector { get; set; }

        public DecodedMBR()
        {

        }


    }

    [Serializable]
    public class FAT16
    {
        public ushort inputFAT { get; set; }
        public FAT16()
        {
            FreeCluster();
        }
        public void FreeCluster()
        {
            inputFAT = 0;
        }
        public void BusyCluster(Nullable<ushort> nextCluster) 
        {
            if (nextCluster != null)
            {
                inputFAT = (ushort)nextCluster;
            }
            else
            {
                inputFAT = 65535;
            }
        }

        public void ReservedCluster()
        {
            inputFAT = 65526;
        }
        public void BadCluster()
        {
            inputFAT = 65527;
        }
    }


    [Serializable]
    public class rootDir
    {
        public long filename { get; set; }
        public byte[] filenameExt { get; set; } //3
        public byte fileAttributes { get; set; }
        public byte NT { get; set; }
        public byte millisegundos_Creado { get; set; }
        public byte[] horaC { get; set; }
        public byte[] fechaC { get; set; }
        public byte[] fechaLastaccess { get; set; }
        public ushort reservedFAT32 { get; set; }
        public byte[] horaLastwrite { get; set; }
        public byte[] fechaLastwrite { get; set; }
        public ushort startingCluster { get; set; }
        public uint fileSize { get; set; }

        public rootDir()
        {
            filename = new long();
            filenameExt = new byte[3];
            fileAttributes = new byte();
            NT = new byte();
            millisegundos_Creado = new byte();
            horaC = new byte[2];
            fechaC = new byte[2];
            fechaLastaccess = new byte[2];
            reservedFAT32 = new ushort();
            horaLastwrite = new byte[2];
            fechaLastwrite = new byte[2];
            startingCluster = new ushort();
            fileSize = new uint();
        }

        public void newFile(string FileName, char atributo, DateTime creado, ushort clusterInicio, uint tamanio)
        {
            string[] FileAndExtension = FileName.Split('.');
            filename = BitConverter.ToInt64(Encoding.ASCII.GetBytes(FileAndExtension[0]), 0);
            filenameExt = Encoding.ASCII.GetBytes(FileAndExtension[1]);
            fileAttributes = Convert.ToByte(atributo);
            NT = 0;
            millisegundos_Creado = Convert.ToByte(creado.Millisecond);
            horaC = Encoding.ASCII.GetBytes(creado.ToShortTimeString());
            fechaC = Encoding.ASCII.GetBytes(creado.ToShortDateString());
            fechaLastaccess = Encoding.ASCII.GetBytes(creado.ToShortDateString());
            reservedFAT32 = 0;
            horaLastwrite = Encoding.ASCII.GetBytes(creado.ToShortTimeString());
            fechaLastwrite = Encoding.ASCII.GetBytes(creado.ToShortDateString());
            startingCluster = clusterInicio;
            fileSize = tamanio;
        }
        public void newFolder(string FileName, DateTime creado)
        {
            filename = BitConverter.ToInt64(Encoding.ASCII.GetBytes(FileName), 0);
            filenameExt = new byte[3];
            fileAttributes = Convert.ToByte('D');
            NT = 0;
            millisegundos_Creado = Convert.ToByte(creado.Millisecond);
            horaC = Encoding.ASCII.GetBytes(creado.ToShortTimeString());
            fechaC = Encoding.ASCII.GetBytes(creado.ToShortDateString());
            fechaLastaccess = Encoding.ASCII.GetBytes(creado.ToShortDateString());
            reservedFAT32 = 0;
            horaLastwrite = Encoding.ASCII.GetBytes(creado.ToShortTimeString());
            fechaLastwrite = Encoding.ASCII.GetBytes(creado.ToShortDateString());
            startingCluster = 0;
            fileSize = 0;
        }

        public void setclusterSubdirectorio(ushort nCluster)
        {
            startingCluster = nCluster;
        }
    }
}
