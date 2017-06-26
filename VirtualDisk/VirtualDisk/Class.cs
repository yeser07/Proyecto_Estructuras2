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
            //constructor
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
            //Constructor
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
            inputFAT = new ushort();
        }
        public void BusyCluster(Nullable<ushort> nextCluster) 
        {
            if (nextCluster != null)
            {
                inputFAT = (ushort)nextCluster;
            }
            else
            {
                inputFAT = 1;
            }
        }

        public void ReservedCluster()
        {
            inputFAT = 2;
        }
        public void BadCluster()
        {
            inputFAT = 3;
        }
    }


    [Serializable]
    public class rootDir
    {
        public rootDir father { get; set; }
        public byte[] filename { get; set; }
        public byte[] filenameExt { get; set; } //3
        public byte fileAttributes { get; set; }
        public byte NT { get; set; }
        public byte millisegundos_Creado { get; set; }
        public ushort horaC { get; set; }
        public ushort fechaC { get; set; }
        public ushort fechaLastaccess { get; set; }
        public ushort reservedFAT32 { get; set; }
        public ushort horaLastwrite { get; set; }
        public ushort fechaLastwrite { get; set; }
        public ushort startingCluster { get; set; }
        public uint fileSize { get; set; }
        public long byteP { get; set; }
        public List<ushort> subDir { get; set; }
        public rootDir()
        {
            filename = new byte [8];
            filenameExt = new byte[3];
            fileAttributes = new byte();
            NT = new byte();
            millisegundos_Creado = new byte();
            horaC = new ushort();
            fechaC = new ushort();
            fechaLastaccess = new ushort();
            reservedFAT32 = new ushort();
            horaLastwrite = new ushort();
            fechaLastwrite = new ushort();
            startingCluster = new ushort();
            fileSize = new uint();
            subDir = new List<ushort>();
        }

        public void newFile(string FileName, char atributo, DateTime creado, ushort clusterInicio, uint size)
        {
            string[] FileAndExtension = FileName.Split('.');
            byte[] temp = Encoding.ASCII.GetBytes(FileAndExtension[0]);
            Array.Resize<byte>(ref temp, 8);
            filename = temp;
            temp = Encoding.ASCII.GetBytes(FileAndExtension[1]);
            Array.Resize<byte>(ref temp, 3);
            filenameExt = temp;
            fileAttributes = Convert.ToByte(atributo);
            NT = 0;
            millisegundos_Creado = Convert.ToByte(creado.Millisecond / 10);

            horaC = setHorasC(creado);
            fechaC = setDiasC(creado);
            fechaLastaccess = setDiasC(creado);
            reservedFAT32 = 0;
            horaLastwrite = setHorasC(creado);
            fechaLastwrite = setDiasC(creado);
            //startingCluster = 0;
            startingCluster = clusterInicio;
            //fileSize = 0;
            fileSize = size;
            subDir = new List<ushort>();
        }

        /* public void newFolder(rootDir Father, string nombreCarpeta, DateTime creado)
         {
             father = Father;
             byte[] temp = Encoding.ASCII.GetBytes(nombreCarpeta);
             Array.Resize<byte>(ref temp, 8);
             filename = temp;
             filenameExt = new byte[3];
             fileAttributes = Convert.ToByte('D');
             NT = 0;
             millisegundos_Creado = Convert.ToByte(creado.Millisecond / 10);
             horaC = setHorasC(creado);
             fechaC = setDiasC(creado);
             fechaLastaccess = setDiasC(creado);
             reservedFAT32 = 0;
             horaLastwrite = setHorasC(creado);
             fechaLastwrite = setDiasC(creado);
             startingCluster = 0;
             fileSize = 0;

             subDir = new List<ushort>();
         }*/

        public void newFolder(string folderName, DateTime creado)
        {
            byte[] temp = Encoding.ASCII.GetBytes(folderName);
            Array.Resize<byte>(ref temp, 8);
            filename = temp;
            filenameExt = new byte[3];
            fileAttributes = Convert.ToByte('D');
            NT = 0;
            millisegundos_Creado = Convert.ToByte(creado.Millisecond / 10);
            horaC = setHorasC(creado);
            fechaC = setDiasC(creado);
            fechaLastaccess = setDiasC(creado);
            reservedFAT32 = 0;
            horaLastwrite = setHorasC(creado);
            fechaLastwrite = setDiasC(creado);
            startingCluster = 0;
            fileSize = 0;
            subDir = new List<ushort>();
        }


        public void changeFolderName(string folderName)
        {
            byte[] temp = Encoding.ASCII.GetBytes(folderName);
            Array.Resize<byte>(ref temp, 8);
            filename = temp;
        }

        public void setclusterSubDir(ushort nCluster)
        {
            startingCluster = nCluster;
        }

        public ushort setDiasC(DateTime fecha)
        {
            DateTime fechabase = DateTime.Parse("28/04/2017");
            ushort result = (ushort)(fecha - fechabase).TotalDays;
            return result;
        }

        public ushort setHorasC(DateTime fecha)
        {
            DateTime fechabase = DateTime.Parse("00:00AM");
            TimeSpan TSpan = fecha.Subtract(fechabase);
            return (ushort)TSpan.TotalMinutes; ;
        }

        public DateTime getFechaC(ushort valor)
        {
            DateTime fechabase = DateTime.Parse("28/04/2017");
            DateTime fecha = fechabase.AddDays(valor);
            return fecha;
        }

        public DateTime getHoraC(ushort valor)
        {
            DateTime fechabase = DateTime.Parse("00:00AM");
            DateTime fecha = fechabase.AddMinutes(valor);
            return fecha;
        }

       /* public void newFolder(string FileName, DateTime creado)
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
        }*/

        /*public void setclusterSubdirectorio(ushort nCluster)
        {
            startingCluster = nCluster;
        }*/
    }

    static class Constants
    {
        public const int ordenArbol = 4;
        public static string discoDefault { get; set; }
        public static string discoIndice { get; set; }
    }

}
