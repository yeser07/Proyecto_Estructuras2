
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Windows.Forms;


namespace VirtualDisk
{
    public partial class Explorador : Form
    {
        public List<FAT16> fat1List = new List<FAT16>();
        public List<FAT16> fat2List = new List<FAT16>();
        public List<rootDir> listaRootDirectory = new List<rootDir>();
        public List<rootDir> listaDirectorioActual = new List<rootDir>();
        // rootDir carpetaActual = new rootDir();
        public rootDir folderActual = new rootDir();
       // long posicionCarpetaActual = new long();
        public DecodedMBR tablaMBR = new DecodedMBR();
        public int mbrOffset = 512;
        public long iTablaFat1 = new long();
        public long iTablaFat2 = new long();
        public bool viewRootDirectory = new bool();
        public int sizeCluster = new int();







        public Explorador()
        {
            InitializeComponent();
        }

        private void Explorador_Load_1(object sender, EventArgs e)
        {
            /*if (Principal.Default == null)
            {
                MessageBox.Show("Abra un disco o cree un nuevo desde el menu archivo",
                                   "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                Disco.Text = "Disco: " + Principal.Default;
                readHDDInformation();
                //Disco.Text = "Disco: " + Principal.Default;
                leerTablasFat();
                leerRootDirectory();
                sizeCluster = tablaMBR.bytesxSec * tablaMBR.sectorxCluster;
                cargarView(true);
            }*/

            Disco.Text = "Disco: " + Constants.discoDefault;
            readHDDInformation();
            leerTablasFat();
            leerRootDirectory();
            sizeCluster = tablaMBR.bytesxSec * tablaMBR.sectorxCluster;
            cargarView(true);
        }

        public void readHDDInformation()
        {
           // DecodedMBR tablaMBR = new DecodedMBR();
            byte[] temporalArray;
            //  using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
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

                /*for (int i = 0; i < 65525; i++)
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
                }*/


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
           // LstPropDisco.Items.Add("Espacio Libre: " + EspacioLibre() + " KB");
        }

        public void leerRootDirectory()
        {
            listaRootDirectory.Clear();
            //using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = (tablaMBR.reservedSectors * tablaMBR.bytesxSec) + mbrOffset;

                for (int i = 0; i < 512; i++)
                {
                    rootDir directorio = new rootDir();
                    directorio.byteP = reader.BaseStream.Position;

                    directorio.filename = reader.ReadBytes(8);
                    directorio.filenameExt = reader.ReadBytes(3);
                    directorio.fileAttributes = reader.ReadByte();
                    directorio.NT = reader.ReadByte();
                    directorio.millisegundos_Creado = reader.ReadByte();
                    directorio.horaC = reader.ReadUInt16();
                    directorio.fechaC = reader.ReadUInt16();
                    directorio.fechaLastaccess= reader.ReadUInt16();
                    directorio.reservedFAT32 = reader.ReadUInt16();
                    directorio.horaLastwrite = reader.ReadUInt16();
                    directorio.fechaLastwrite = reader.ReadUInt16();
                    directorio.startingCluster = reader.ReadUInt16();
                    directorio.fileSize = reader.ReadUInt32();

                    listaRootDirectory.Add(directorio);
                }
            }
        }

        public void leerTablasFat()
        {
            fat1List.Clear();
            fat2List.Clear();
            // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = mbrOffset;
                iTablaFat1 = reader.BaseStream.Position;
                for (int i = 0; i < 65536; i++)
                {
                    FAT16 fatentry = new FAT16();
                    fatentry.inputFAT = reader.ReadUInt16();
                    fat1List.Add(fatentry);
                }
                iTablaFat2 = reader.BaseStream.Position;
                for (int i = 0; i < 65536; i++)
                {
                    FAT16 fatentry = new FAT16();
                    fatentry.inputFAT = reader.ReadUInt16();
                    fat2List.Add(fatentry);
                }
            }
            LstPropDisco.Items[12] = "Espacio Libre: " + calcularEspacioLibre() + " KB";
        }

        public int calcularEspacioLibre()
        {
            int clustersVacios = fat1List.Count(x => x.inputFAT == 0);
            return 16 * clustersVacios;
        }

        public void cargarView(bool isRoot)
        {
            /* viewGeneral.Items.Clear();
             List<rootDir> listaDirectorios = new List<rootDir>();

             if (!isRoot)
             {
                 string[] info = { ".." + Encoding.ASCII.GetString(carpetaActual.filename), "", carpetaActual.byteP.ToString() };
                 ListViewItem nod = new ListViewItem(info, 0);
                 nod.Tag = "D," + folderActual.byteP;
                 viewGeneral.Items.Add(nod);
                 viewRootDirectory = false;
                 listaDirectorios = listaDirectorioActual;
             }
             else
             {
                 viewRootDirectory = true;
                 listaDirectorios = listaRootDirectory;
             }
             foreach (var file in listaDirectorios)
             {
                 if (file.filename[0] != 0)
                 {
                     if (Convert.ToChar(file.fileAttributes) == 'D')
                     {
                         string nombre = Encoding.ASCII.GetString(file.filename);
                         DateTime fechaCreacion = file.getFechaC(file.fechaC);
                         DateTime horaCreacion = file.getHoraC(file.horaC);

                         string[] info = { nombre,
                                           fechaCreacion.ToShortDateString() + ' ' + horaCreacion.ToShortTimeString(),
                                           file.fileSize.ToString()
                                         };

                         ListViewItem nod = new ListViewItem(info, 0);
                         nod.Tag = "D," + file.byteP;
                         viewGeneral.Items.Add(nod);
                     }
                     else if (Convert.ToChar(file.fileAttributes) == 'A')
                     {
                         string a = Encoding.ASCII.GetString(file.filename);
                         a = a + "." + Encoding.ASCII.GetString(file.filenameExt);
                         string nombre = a.Replace("\0", string.Empty);

                         DateTime fechaCreacion = file.getFechaC(file.fechaC);
                         DateTime horaCreacion = file.getHoraC(file.horaC);

                         string[] info = { nombre,
                                           fechaCreacion.ToShortDateString() + ' ' + horaCreacion.ToShortTimeString(),
                                           file.fileSize.ToString()
                                         };

                         ListViewItem nod = new ListViewItem(info, 1);
                         nod.Tag = "A," + file.byteP;
                         viewGeneral.Items.Add(nod);
                     }
                 }
             }*/

            viewGeneral.Items.Clear();
            List<rootDir> listaDirectory = new List<rootDir>();
            int inicioContador = 0;
            if (!isRoot)
            {
                viewRootDirectory = false;
                listaDirectory = listaDirectorioActual;
                rootDir directorioRaiz = listaDirectory.ElementAt(0);
                string[] info = { "..", "", "" };
                folderActual = directorioRaiz;
                inicioContador = 1;
                ListViewItem nod = new ListViewItem(info, 0);
                nod.Tag = "D," + directorioRaiz.father.startingCluster;
                viewGeneral.Items.Add(nod);
            }
            else
            {
                inicioContador = 0;
                viewRootDirectory = true;
                listaDirectory = listaRootDirectory;
            }

            for (int i = inicioContador; i < listaDirectory.Count(); i++)
            {
                var file = listaDirectory.ElementAt(i);

                if (file.filename[0] != 0)
                {
                    if (Convert.ToChar(file.fileAttributes) == 'D')
                    {
                        string nombre = Encoding.ASCII.GetString(file.filename);
                        DateTime fechaCreacion = file.getFechaC(file.fechaC);
                        DateTime horaCreacion = file.getHoraC(file.horaC);

                        string[] info = { nombre,fechaCreacion.ToShortDateString() + ' ' + horaCreacion.ToShortTimeString(),file.fileSize.ToString()};
                        ListViewItem nod = new ListViewItem(info, 0);
                        nod.Tag = "D," + file.startingCluster;
                        viewGeneral.Items.Add(nod);
                    }
                    else if (Convert.ToChar(file.fileAttributes) == 'A')
                    {
                        string a = Encoding.ASCII.GetString(file.filename);
                        a = a + "." + Encoding.ASCII.GetString(file.filenameExt);
                        string nombre = a.Replace("\0", string.Empty);

                        DateTime fechaC = file.getFechaC(file.fechaC);
                        DateTime horaC = file.getHoraC(file.horaC);

                        string[] info = { nombre,
                                              fechaC.ToShortDateString() + ' ' + horaC.ToShortTimeString(),file.fileSize.ToString()};
                        ListViewItem nod = new ListViewItem(info, 1);
                        ushort clusterDirectorioArchivo;
                        if (viewRootDirectory)
                        {
                            clusterDirectorioArchivo = file.startingCluster;
                        }
                        else
                        {
                            clusterDirectorioArchivo = clusterPosicionByte(file.byteP);
                        }
                        nod.Tag = "A," + clusterDirectorioArchivo;
                        viewGeneral.Items.Add(nod);
                    }
                }
            }
        }

        public long posicionByteLibreRootDirectory()
        {
            long posicionByteLibre = -1;
            for (int i = 0; i < listaRootDirectory.Count(); i++)
            {
                if (listaRootDirectory.ElementAt(i).filename[0] == 0)
                {
                    posicionByteLibre = (tablaMBR.reservedSectors * tablaMBR.bytesxSec) + (i * 32) + mbrOffset;
                    break;
                }
            }
            return posicionByteLibre;
        }


        /*  public void actualizarCarpetaActual()
          {
              carpetaActual.subDir.Clear();
              long posicion = carpetaActual.byteP;
              carpetaActual.father = new rootDir();
              using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
              {
                  reader.BaseStream.Position = posicion + 26;
                  carpetaActual.father.startingCluster = reader.ReadUInt16();
                  reader.BaseStream.Position = reader.BaseStream.Position + 2;
                  int limiteCluster = (sizeCluster - 64) / 2;
                  for (int j = 0; j < limiteCluster; j++)
                  {
                      ushort apuntadorFAT = reader.ReadUInt16();
                      carpetaActual.subDir.Add(apuntadorFAT);
                  }
              }
              listaDirectorioActual.Clear();
              listaDirectorioActual = getSubdirectorio(carpetaActual.subDir);
          }*/

        public void loadFolder(ushort cluster)
        {
            long p = posicionByteCluster(cluster);
            rootDir folder = new rootDir();
            folder = leerEntradaDirectorio(p);
            // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = p + 64;
                int limiteCluster = (sizeCluster - 64) / 2;
                for (int j = 0; j < limiteCluster; j++)
                {
                    ushort apuntadorFAT = reader.ReadUInt16();
                    folder.subDir.Add(apuntadorFAT);
                }
            }
            folderActual = folder;
            listaDirectorioActual.Clear();
            listaDirectorioActual.Add(folder);
            var listaDirectorios = getSubdirectorio(folder.subDir);
            listaDirectorioActual.AddRange(listaDirectorios.ToList());
        }

        public void crearCarpetaEnRoot(rootDir newfolder)
        {
            long posicionByteLibre = posicionByteLibreRootDirectory();

            //int posicion = 0;
            if (posicionByteLibre >= 0)
            {
                ushort clusterSubCarpeta = buscarClusterVacio();
                newfolder.startingCluster = clusterSubCarpeta;
                // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                {
                    stream.BaseStream.Position = posicionByteLibre;
                    stream.Write(newfolder.filename);
                    stream.Write(newfolder.filenameExt);
                    stream.Write(newfolder.fileAttributes);
                    stream.Write(newfolder.NT);
                    stream.Write(newfolder.millisegundos_Creado);
                    stream.Write(newfolder.horaC);
                    stream.Write(newfolder.fechaC);
                    stream.Write(newfolder.fechaLastaccess);
                    stream.Write(newfolder.reservedFAT32);
                    stream.Write(newfolder.horaLastwrite);
                    stream.Write(newfolder.fechaLastwrite);
                    stream.Write(newfolder.startingCluster);
                    stream.Write(newfolder.fileSize);

                    long posicionBytesSubCarpeta = posicionByteCluster(clusterSubCarpeta);
                    stream.BaseStream.Position = posicionBytesSubCarpeta;
                    stream.Write(newfolder.filename);
                    stream.Write(newfolder.filenameExt);
                    stream.Write(newfolder.fileAttributes);
                    stream.Write(newfolder.NT);
                    stream.Write(newfolder.millisegundos_Creado);
                    stream.Write(newfolder.horaC);
                    stream.Write(newfolder.fechaC);
                    stream.Write(newfolder.fechaLastaccess);
                    stream.Write(newfolder.reservedFAT32);
                    stream.Write(newfolder.horaLastwrite);
                    stream.Write(newfolder.fechaLastwrite);
                    ushort clusterDirectorio = new ushort();
                    stream.Write(clusterDirectorio);
                    stream.Write(newfolder.fileSize);

                    stream.Write(newfolder.filename);
                    stream.Write(newfolder.filenameExt);
                    stream.Write(newfolder.fileAttributes);
                    stream.Write(newfolder.NT);
                    stream.Write(newfolder.millisegundos_Creado);
                    stream.Write(newfolder.horaC);
                    stream.Write(newfolder.fechaC);
                    stream.Write(newfolder.fechaLastaccess);
                    stream.Write(newfolder.reservedFAT32);
                    stream.Write(newfolder.horaLastwrite);
                    stream.Write(newfolder.fechaLastwrite);
                    stream.Write(clusterSubCarpeta);
                    stream.Write(newfolder.fileSize);

                    int limiteCluster = (sizeCluster - 64) / 2;
                    for (int i = 0; i < limiteCluster; i++)
                    {
                        ushort apuntadorFAT = 0;
                        stream.Write(apuntadorFAT);
                    }
                }

                setmarcadorCluster(clusterSubCarpeta, 1);
            }
            else
            {
                MessageBox.Show("Error al generar la carpeta,no se puede crear!", "Informacion" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void borrarCarpetaEnRoot(string nombreCarpeta)
        {
            rootDir carpeta = listaRootDirectory.Where(x => Encoding.ASCII.GetString(x.filename) == nombreCarpeta).FirstOrDefault();

            long posicion = carpeta.byteP;
            rootDir carpetaenblanco = new rootDir();
            using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
            {
                stream.BaseStream.Position = posicion;
                stream.Write(carpetaenblanco.filename);
                stream.Write(carpetaenblanco.filenameExt);
                stream.Write(carpetaenblanco.fileAttributes);
                stream.Write(carpetaenblanco.NT);
                stream.Write(carpetaenblanco.millisegundos_Creado);
                stream.Write(carpetaenblanco.horaC);
                stream.Write(carpetaenblanco.fechaC);
                stream.Write(carpetaenblanco.fechaLastaccess);
                stream.Write(carpetaenblanco.reservedFAT32);
                stream.Write(carpetaenblanco.horaLastwrite);
                stream.Write(carpetaenblanco.fechaLastwrite);
                stream.Write(carpetaenblanco.startingCluster);
                stream.Write(carpetaenblanco.fileSize);
            }
            setmarcadorCluster(carpeta.startingCluster, 0);
        }


        public void crearCarpeta(rootDir newfolder)
        {
            ushort cluster = buscarClusterVacio();

            //int posicion = 0;
            if (cluster > 0)
            {
                long posicionByte = posicionByteCluster(cluster);
                newfolder.startingCluster = cluster;
                // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                {
                    stream.BaseStream.Position = posicionByte;

                    stream.Write(folderActual.filename);
                    stream.Write(folderActual.filenameExt);
                    stream.Write(folderActual.fileAttributes);
                    stream.Write(folderActual.NT);
                    stream.Write(folderActual.millisegundos_Creado);
                    stream.Write(folderActual.horaC);
                    stream.Write(folderActual.fechaC);
                    stream.Write(folderActual.fechaLastaccess);
                    stream.Write(folderActual.reservedFAT32);
                    stream.Write(folderActual.horaLastwrite);
                    stream.Write(folderActual.fechaLastwrite);
                    stream.Write(folderActual.startingCluster);
                    stream.Write(folderActual.fileSize);

                    stream.Write(newfolder.filename);
                    stream.Write(newfolder.filenameExt);
                    stream.Write(newfolder.fileAttributes);
                    stream.Write(newfolder.NT);
                    stream.Write(newfolder.millisegundos_Creado);
                    stream.Write(newfolder.horaC);
                    stream.Write(newfolder.fechaC);
                    stream.Write(newfolder.fechaLastaccess);
                    stream.Write(newfolder.reservedFAT32);
                    stream.Write(newfolder.horaLastwrite);
                    stream.Write(newfolder.fechaLastaccess);
                    stream.Write(newfolder.startingCluster);
                    stream.Write(newfolder.fileSize);

                    int limiteCluster = (sizeCluster - 64) / 2;
                    for (int i = 0; i < limiteCluster; i++)
                    {
                        ushort apuntadorFAT = 0;
                        stream.Write(apuntadorFAT);
                    }
                }
                setmarcadorCluster(cluster, 1);
                agregarSubDirectorioCarpetaActual(cluster);
            }
            else
            {
                MessageBox.Show("No se puede crear la carpeta!" , "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void guardarArchivoEnRoot(rootDir Archivo, byte[] archivo)
        {

            long posicionByteLibre = posicionByteLibreRootDirectory();
            if (posicionByteLibre >= 0)
            {
                ushort clusterInicioArchivo = buscarClusterVacio();
                Archivo.startingCluster = clusterInicioArchivo;

                long posicionClusterArchivo = posicionByteCluster(clusterInicioArchivo);

                // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                {
                    stream.BaseStream.Position = posicionByteLibre;
                    stream.Write(Archivo.filename);
                    stream.Write(Archivo.filenameExt);
                    stream.Write(Archivo.fileAttributes);
                    stream.Write(Archivo.NT);
                    stream.Write(Archivo.millisegundos_Creado);
                    stream.Write(Archivo.horaC);
                    stream.Write(Archivo.fechaC);
                    stream.Write(Archivo.fechaC);
                    stream.Write(Archivo.reservedFAT32);
                    stream.Write(Archivo.horaC);
                    stream.Write(Archivo.fechaC);
                    stream.Write(Archivo.startingCluster);
                    stream.Write(Archivo.fileSize);
                }

                if (Archivo.fileSize < sizeCluster)
                {

                    // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))

                    using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                    {
                        stream.BaseStream.Position = posicionClusterArchivo;
                        stream.Write(archivo, 0, archivo.Length);
                    }
                    setmarcadorCluster(clusterInicioArchivo, 1);
                }
                else
                {
                    int cantidadClusters = (int)Math.Ceiling((double)Archivo.fileSize / sizeCluster);
                    ushort[] clusterAsignado = new ushort[cantidadClusters];
                    clusterAsignado[0] = clusterInicioArchivo;
                    setmarcadorCluster(clusterInicioArchivo, 1);

                    for (int j = 1; j < cantidadClusters; j++)
                    {
                        clusterAsignado[j] = buscarClusterVacio();
                        setmarcadorCluster(clusterAsignado[j], 1);
                    }

                    for (int i = 0; i < cantidadClusters; i++)
                    {
                        posicionClusterArchivo = posicionByteCluster(clusterAsignado[i]);
                        // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                        using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                        {
                            stream.BaseStream.Position = posicionClusterArchivo;
                            int offset = 0;
                            if (i > 0) offset = (i * sizeCluster);

                            if (i == cantidadClusters - 1)
                            {
                                long bytesSobrantes = Archivo.fileSize - (sizeCluster * (cantidadClusters - 1));
                                stream.Write(archivo, offset, (int)bytesSobrantes);
                            }
                            else
                            {
                                stream.Write(archivo, offset, sizeCluster);
                            }
                        }

                        if (i != cantidadClusters - 1)
                        {
                            setmarcadorCluster(clusterAsignado[i], clusterAsignado[i + 1]);
                        }
                        else
                        {
                            setmarcadorCluster(clusterAsignado[i], 1);
                        }
                    }
                }
                leerRootDirectory();
            }
            else
            {
                MessageBox.Show("No se puede guardar el archivo!", "Informacion" , MessageBoxButtons.OK , MessageBoxIcon.Error);
            }
        }

        /*public byte[] sacarArchivoDeRoot(ushort cluster)
        {
            /*var archivo = listaRootDirectory.Where(x => (Encoding.ASCII.GetString(x.filename) + "." + Encoding.ASCII.GetString(x.filenameExt)) == nombreArchivo).FirstOrDefault();
            byte[] result = new byte[archivo.fileSize];
            long posicionInicioCluster = 0;
            ushort[] clusters = clustersArchivo(archivo);
            if (clusters.Length == 1)
            {
                posicionInicioCluster = posicionByteCluster(archivo.startingCluster);
                using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                {
                    reader.BaseStream.Position = posicionInicioCluster;
                    reader.Read(result, 0, (int)archivo.fileSize);
                }
            }
            else
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    posicionInicioCluster = posicionByteCluster(clusters[i]);
                    using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                    {
                        reader.BaseStream.Position = posicionInicioCluster;
                        int offset = 0;
                        if (i > 0) offset = (i * sizeCluster);

                        if (i == clusters.Length - 1)
                        {
                            long bytesSobrantes = archivo.fileSize - (sizeCluster * (clusters.Length - 1));
                            reader.Read(result, offset, (int)bytesSobrantes);
                        }
                        else
                        {
                            reader.Read(result, offset, sizeCluster);
                        }
                    }
                }
            }
            return result;

            var archivo = listaRootDirectory.Where(x => x.startingCluster == cluster).FirstOrDefault();
            byte[] result = new byte[archivo.fileSize];
            long posicionInicioCluster = 0;
            ushort[] clusters = clustersArchivo(archivo);
            if (clusters.Length == 1)
            {
                posicionInicioCluster = posicionByteCluster(archivo.startingCluster);
                using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                {
                    reader.BaseStream.Position = posicionInicioCluster;
                    reader.Read(result, 0, (int)archivo.fileSize);
                }
            }
            else
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    posicionInicioCluster = posicionByteCluster(clusters[i]);
                    using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                    {
                        reader.BaseStream.Position = posicionInicioCluster;
                        int offset = 0;
                        if (i > 0) offset = (i * sizeCluster);

                        if (i == clusters.Length - 1)
                        {
                            long bytesSobrantes = archivo.fileSize - (sizeCluster * (clusters.Length - 1));
                            reader.Read(result, offset, (int)bytesSobrantes);
                        }
                        else
                        {
                            reader.Read(result, offset, sizeCluster);
                        }
                    }
                }
            }
            return result;


        }*/


        public ushort guardarArchivo(rootDir Archivo, byte[] archivo)
        {
            ushort clusterDirectoryEntry = buscarClusterVacio();
            long posicionByteDirectoryEntry = posicionByteCluster(clusterDirectoryEntry);
            if (posicionByteDirectoryEntry >= 0)
            {
                if (Archivo.fileSize < sizeCluster - 32)
                {
                    Archivo.startingCluster = 0;
                    //using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                    using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                    {
                        stream.BaseStream.Position = posicionByteDirectoryEntry;
                        stream.Write(Archivo.filename);
                        stream.Write(Archivo.filenameExt);
                        stream.Write(Archivo.fileAttributes);
                        stream.Write(Archivo.NT);
                        stream.Write(Archivo.millisegundos_Creado);
                        stream.Write(Archivo.horaC);
                        stream.Write(Archivo.fechaC);
                        stream.Write(Archivo.fechaLastaccess);
                        stream.Write(Archivo.reservedFAT32);
                        stream.Write(Archivo.horaLastwrite);
                        stream.Write(Archivo.fechaLastwrite);
                        stream.Write(Archivo.startingCluster);
                        stream.Write(Archivo.fileSize);
                        stream.Write(archivo);
                    }
                    setmarcadorCluster(clusterDirectoryEntry, 1);
                }
                else
                {
                    setmarcadorCluster(clusterDirectoryEntry, 1);
                    ushort[] clustersAsignados = asignarClustersArchivo(Archivo.fileSize);

                    setmarcadorCluster(clusterDirectoryEntry, clustersAsignados[0]);
                    Archivo.startingCluster = clustersAsignados[0];
                    long posicionClusterArchivo = posicionByteCluster(clusterDirectoryEntry);
                    // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                    using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                    {
                        stream.BaseStream.Position = posicionByteDirectoryEntry;
                        stream.Write(Archivo.filename);
                        stream.Write(Archivo.filenameExt);
                        stream.Write(Archivo.fileAttributes);
                        stream.Write(Archivo.NT);
                        stream.Write(Archivo.millisegundos_Creado);
                        stream.Write(Archivo.horaC);
                        stream.Write(Archivo.fechaC);
                        stream.Write(Archivo.fechaLastaccess);
                        stream.Write(Archivo.reservedFAT32);
                        stream.Write(Archivo.horaLastwrite);
                        stream.Write(Archivo.fechaLastwrite);
                        stream.Write(Archivo.startingCluster);
                        stream.Write(Archivo.fileSize);
                    }

                    for (int i = 0; i < clustersAsignados.Length; i++)
                    {
                        posicionClusterArchivo = posicionByteCluster(clustersAsignados[i]);
                        // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
                        using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
                        {
                            stream.BaseStream.Position = posicionClusterArchivo;
                            int offset = 0;
                            if (i > 0) offset = (i * sizeCluster);

                            if (i == clustersAsignados.Length - 1)
                            {
                                long bytesSobrantes = Archivo.fileSize - (sizeCluster * (clustersAsignados.Length - 1));
                                stream.Write(archivo, offset, (int)bytesSobrantes);
                            }
                            else
                            {
                                stream.Write(archivo, offset, sizeCluster);
                            }
                        }

                        if (i != clustersAsignados.Length - 1)
                        {
                            setmarcadorCluster(clustersAsignados[i], clustersAsignados[i + 1]);
                        }
                        else
                        {
                            setmarcadorCluster(clustersAsignados[i], 1);
                        }
                    }
                }
                agregarSubDirectorioCarpetaActual(clusterDirectoryEntry);
            }
            else
            {
                MessageBox.Show("No se puede Guardar el Archivo!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return clusterDirectoryEntry;
        }


        public rootDir leerEntradaDirectorio(long posicionBytes)
        {
            rootDir result = new rootDir();
            result.father = new rootDir();
            // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = posicionBytes;
                result.father.byteP = reader.BaseStream.Position;
                result.father.filename = reader.ReadBytes(8);
                result.father.filenameExt = reader.ReadBytes(3);
                result.father.fileAttributes = reader.ReadByte();
                result.father.NT = reader.ReadByte();
                result.father.millisegundos_Creado = reader.ReadByte();
                result.father.horaC = reader.ReadUInt16();
                result.father.fechaC = reader.ReadUInt16();
                result.father.fechaLastaccess = reader.ReadUInt16();
                result.father.reservedFAT32 = reader.ReadUInt16();
                result.father.horaLastwrite = reader.ReadUInt16();
                result.father.fechaLastwrite = reader.ReadUInt16();
                result.father.startingCluster = reader.ReadUInt16();
                result.father.fileSize = reader.ReadUInt32();
                result.byteP = posicionBytes;
                result.filename = reader.ReadBytes(8);
                result.filenameExt = reader.ReadBytes(3);
                result.fileAttributes = reader.ReadByte();
                result.NT = reader.ReadByte();
                result.millisegundos_Creado = reader.ReadByte();
                result.horaC = reader.ReadUInt16();
                result.fechaC = reader.ReadUInt16();
                result.fechaLastaccess = reader.ReadUInt16();
                result.reservedFAT32 = reader.ReadUInt16();
                result.horaLastwrite = reader.ReadUInt16();
                result.fechaLastwrite = reader.ReadUInt16();
                result.startingCluster = reader.ReadUInt16();
                result.fileSize = reader.ReadUInt32();
            }
            return result;
        }

        public List<rootDir> getSubdirectorio(List<ushort> clusterSubdirectorio)
        {
            /* List<rootDir> result = new List<rootDir>();
             var filtro = clusterSubdirectorio.Where(x => x != 0);
             foreach (ushort c in filtro)
             {
                 rootDir entradaDirectorio = new rootDir();
                 entradaDirectorio.father = new rootDir();
                 long posicionBytes = posicionByteCluster(c);
                 using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                 {
                     reader.BaseStream.Position = posicionBytes;
                     entradaDirectorio.father.byteP = reader.BaseStream.Position;
                     ///reader.BaseStream.Position = posicionBytes + 32; 
                     entradaDirectorio.father.filename = reader.ReadBytes(8);
                     entradaDirectorio.father.filenameExt = reader.ReadBytes(3);
                     entradaDirectorio.father.fileAttributes = reader.ReadByte();
                     entradaDirectorio.father.NT = reader.ReadByte();
                     entradaDirectorio.father.millisegundos_Creado = reader.ReadByte();
                     entradaDirectorio.father.horaC = reader.ReadUInt16();
                     entradaDirectorio.father.fechaC = reader.ReadUInt16();
                     entradaDirectorio.father.fechaLastaccess = reader.ReadUInt16();
                     entradaDirectorio.father.reservedFAT32 = reader.ReadUInt16();
                     entradaDirectorio.father.horaLastwrite = reader.ReadUInt16();
                     entradaDirectorio.father.fechaLastwrite = reader.ReadUInt16();
                     entradaDirectorio.father.startingCluster = reader.ReadUInt16();
                     entradaDirectorio.father.fileSize = reader.ReadUInt32();

                     entradaDirectorio.byteP = posicionBytes;
                     entradaDirectorio.filename = reader.ReadBytes(8);
                     entradaDirectorio.filenameExt = reader.ReadBytes(3);
                     entradaDirectorio.fileAttributes = reader.ReadByte();
                     entradaDirectorio.NT = reader.ReadByte();
                     entradaDirectorio.millisegundos_Creado = reader.ReadByte();
                     entradaDirectorio.horaC = reader.ReadUInt16();
                     entradaDirectorio.fechaC = reader.ReadUInt16();
                     entradaDirectorio.fechaLastaccess = reader.ReadUInt16();
                     entradaDirectorio.reservedFAT32 = reader.ReadUInt16();
                     entradaDirectorio.horaLastwrite = reader.ReadUInt16();
                     entradaDirectorio.fechaLastwrite = reader.ReadUInt16();
                     entradaDirectorio.startingCluster = reader.ReadUInt16();
                     entradaDirectorio.fileSize = reader.ReadUInt32();
                 }
                 result.Add(entradaDirectorio);
             }


             return result;*/

            List<rootDir> result = new List<rootDir>();
            var filtro = clusterSubdirectorio.Where(x => x != 0);
            foreach (ushort c in filtro)
            {
                rootDir DirEntry = new rootDir();
                DirEntry.father = new rootDir();
                long posicionBytes = posicionByteCluster(c);
                // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
                {
                    reader.BaseStream.Position = posicionBytes;
                    DirEntry.father.byteP = reader.BaseStream.Position;
                    ///reader.BaseStream.Position = posicionBytes + 32; 
                    DirEntry.father.filename = reader.ReadBytes(8);
                    DirEntry.father.filenameExt = reader.ReadBytes(3);
                    DirEntry.father.fileAttributes = reader.ReadByte();
                    DirEntry.father.NT = reader.ReadByte();
                    DirEntry.father.millisegundos_Creado = reader.ReadByte();
                    DirEntry.father.horaC = reader.ReadUInt16();
                    DirEntry.father.fechaC = reader.ReadUInt16();
                    DirEntry.father.fechaLastaccess = reader.ReadUInt16();
                    DirEntry.father.reservedFAT32 = reader.ReadUInt16();
                    DirEntry.father.horaLastwrite = reader.ReadUInt16();
                    DirEntry.father.fechaLastwrite = reader.ReadUInt16();
                    DirEntry.father.startingCluster = reader.ReadUInt16();
                    DirEntry.father.fileSize = reader.ReadUInt32();
                    char attr = Convert.ToChar(DirEntry.father.fileAttributes);
                    if (attr == 'A')
                    {
                        DirEntry.byteP = posicionBytes;
                        DirEntry.filename = DirEntry.father.filename;
                        DirEntry.filenameExt = DirEntry.father.filenameExt;
                        DirEntry.fileAttributes = DirEntry.father.fileAttributes;
                        DirEntry.NT = DirEntry.father.NT;
                        DirEntry.millisegundos_Creado = DirEntry.father.millisegundos_Creado;
                        DirEntry.horaC = DirEntry.father.horaC;
                        DirEntry.fechaC = DirEntry.father.fechaC;
                        DirEntry.fechaLastaccess = DirEntry.father.fechaLastaccess;
                        DirEntry.reservedFAT32 = DirEntry.father.reservedFAT32;
                        DirEntry.horaLastwrite = DirEntry.father.horaLastwrite;
                        DirEntry.fechaLastwrite = DirEntry.father.fechaLastwrite;
                        DirEntry.startingCluster = DirEntry.father.startingCluster;
                        DirEntry.fileSize = DirEntry.father.fileSize;
                        DirEntry.father = null;
                    }
                    else
                    {
                        DirEntry.byteP = posicionBytes;
                        DirEntry.filename = reader.ReadBytes(8);
                        DirEntry.filenameExt = reader.ReadBytes(3);
                        DirEntry.fileAttributes = reader.ReadByte();
                        DirEntry.NT = reader.ReadByte();
                        DirEntry.millisegundos_Creado = reader.ReadByte();
                        DirEntry.horaC = reader.ReadUInt16();
                        DirEntry.fechaC = reader.ReadUInt16();
                        DirEntry.fechaLastaccess = reader.ReadUInt16();
                        DirEntry.reservedFAT32 = reader.ReadUInt16();
                        DirEntry.horaLastwrite = reader.ReadUInt16();
                        DirEntry.fechaLastwrite = reader.ReadUInt16();
                        DirEntry.startingCluster = reader.ReadUInt16();
                        DirEntry.fileSize = reader.ReadUInt32();
                    }
                }
                result.Add(DirEntry);
            }
            return result;
        }

        public void agregarSubDirectorioCarpetaActual(ushort cluster)
        {
            int entradaLibre = 0;
            int limiteCluster = (sizeCluster - 64) / 2;
            long posicion = folderActual.byteP + 64;

            //  using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = posicion;

                for (int j = 0; j < limiteCluster; j++)
                {
                    ushort apuntadorFAT = reader.ReadUInt16();
                    if (apuntadorFAT == 0)
                    {
                        entradaLibre = j;
                        break;
                    }
                }
            }

            // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
            using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
            {
                stream.BaseStream.Position = (posicion) + (entradaLibre * 2);
                stream.Write(cluster);
            }
           // actualizarCarpetaActual();
        }

        public void borrarSubDirectorioCarpetaActual(ushort cluster)
        {
            int posicionCluster = 0;
            int limiteCluster = (sizeCluster - 64) / 2;
            long posicion = folderActual.byteP + 64;

            // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = posicion;

                for (int j = 0; j < limiteCluster; j++)
                {
                    ushort apuntadorFAT = reader.ReadUInt16();
                    if (apuntadorFAT == cluster)
                    {
                        posicionCluster = j;
                        break;
                    }
                }
            }

            // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))

            using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
            {
                stream.BaseStream.Position = (posicion) + (posicionCluster * 2);
                ushort vacio = new ushort();
                stream.Write(0);
            }
        }

        void setmarcadorCluster(ushort numeroCluster, ushort marcador)
        {
            //using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
            using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
            {
                stream.BaseStream.Position = iTablaFat1 + (numeroCluster * 2);
                stream.Write(marcador);
            }
            leerTablasFat();
        }

        public ushort buscarClusterVacio()
        {
            for (ushort i = 0; i < fat1List.Count(); i++)
            {
                if (fat1List.ElementAt(i).inputFAT == 0)
                {
                    return i;
                }
            }
            return 0;
        }


        public long posicionByteCluster(ushort numeroCluster)
        {
            long posicion = (numeroCluster * sizeCluster) + mbrOffset;
            return posicion;
        }

        public ushort clusterPosicionByte(long posicionBytes)
        {
            ushort cluster = (ushort)((posicionBytes - mbrOffset) / sizeCluster);
            return cluster;
        }


        public ushort[] clustersArchivo(rootDir archivo)
        {
            /*List<ushort> listaClusters = new List<ushort>();
            ushort clusterInicio = archivo.startingCluster;
            listaClusters.Add(clusterInicio);
            ushort contadorCluster = clusterInicio;
            do
            {
                if (fat1List.ElementAt(contadorCluster).inputFAT == 1) 
                {
                    break;
                }
                else
                {
                    listaClusters.Add(fat1List.ElementAt(contadorCluster).inputFAT);
                    contadorCluster = fat1List.ElementAt(contadorCluster).inputFAT;
                }
            } while (contadorCluster < fat1List.Count());
            return listaClusters.ToArray();*/

            List<ushort> listaClusters = new List<ushort>();
            ushort clusterInicio = archivo.startingCluster;
            if (clusterInicio > 0)
            {
                listaClusters.Add(clusterInicio);
                ushort contadorCluster = clusterInicio;
                do
                {
                    if (fat1List.ElementAt(contadorCluster).inputFAT == 1)
                    {
                        break;
                    }
                    else
                    {
                        listaClusters.Add(fat1List.ElementAt(contadorCluster).inputFAT);
                        contadorCluster = fat1List.ElementAt(contadorCluster).inputFAT;
                    }
                } while (contadorCluster < fat1List.Count());
            }
            else
            {
                ushort cluster = clusterPosicionByte(archivo.byteP);
                listaClusters.Add(cluster);
            }
            return listaClusters.ToArray();
        }

        public ushort[] asignarClustersArchivo(uint FileSize)
        {
            int cantidadClusters = (int)Math.Ceiling((double)FileSize / sizeCluster);
            ushort[] clusterAsignado = new ushort[cantidadClusters];

            for (int j = 0; j < cantidadClusters; j++)
            {
                clusterAsignado[j] = buscarClusterVacio();
                setmarcadorCluster(clusterAsignado[j], 1);
            }
            return clusterAsignado;
        }

        public int EspacioLibre()
        {
            return 0;
        }

        private void NuevaCarpeta_Click(object sender, EventArgs e)
        {
            string nombreCarpeta = Interaction.InputBox("Nombre", "Nueva Carpeta");

            /*do
            {
                nombreCarpeta = Interaction.InputBox("Nombre", "Nueva Carpeta");

                if (nombreCarpeta.Length > 0 && nombreCarpeta.Length <= 8)
                {
                    break;
                }
                else
                {
                    MessageBox.Show("El nombre debe ser menor o igual a 8 caracteres"
                                    , "Informacion"
                                    , MessageBoxButtons.OK
                                    , MessageBoxIcon.Exclamation);
                }
            } while (true);

            if (viewRootDirectory)
            {
                int existecarpeta = 0;
                foreach (var v in listaRootDirectory)
                {
                    string a = Encoding.ASCII.GetString(v.filename);
                    string nombre = a.Replace("\0", string.Empty);
                    if (nombre == nombreCarpeta)
                    {
                        existecarpeta = 1;
                    }
                }

                if (existecarpeta > 0)
                {
                    MessageBox.Show("Ya existe una carpeta con ese nombre!"
                                    , "Informacion"
                                    , MessageBoxButtons.OK
                                    , MessageBoxIcon.Error);
                }
                else
                {
                    rootDir Carpeta = new rootDir();
                    Carpeta.newFolder (null, nombreCarpeta, DateTime.Now);
                    crearCarpetaEnRoot(Carpeta);

                    leerRootDirectory();
                    cargarView(true);
                }
            }
            else
            {
                int existecarpeta = 0;
                foreach (var v in listaDirectorioActual)
                {
                    string a = Encoding.ASCII.GetString(v.filename);
                    string nombre = a.Replace("\0", string.Empty);
                    if (nombre == nombreCarpeta)
                    {
                        existecarpeta = 1;
                    }
                }

                if (existecarpeta > 0)
                {
                    MessageBox.Show("Ya existe una carpeta con ese nombre!"
                                    , "Informacion"
                                    , MessageBoxButtons.OK
                                    , MessageBoxIcon.Error);
                }
                else
                {
                    rootDir Carpeta = new rootDir();
                    Carpeta.newFolder(carpetaActual, nombreCarpeta, DateTime.Now);
                    crearCarpeta(Carpeta);
                    actualizarCarpetaActual();
                    cargarView(false);
                }
            }*/

            if (nombreCarpeta.Length > 0 && nombreCarpeta.Length <= 8)
            {
                if (viewRootDirectory)
                {
                    int existeFolder = 0;
                    foreach (var v in listaRootDirectory)
                    {
                        string a = Encoding.ASCII.GetString(v.filename);
                        string nombre = a.Replace("\0", string.Empty);
                        if (nombre == nombreCarpeta)
                        {
                            existeFolder = 1;
                        }
                    }

                    if (existeFolder > 0)
                    {
                        MessageBox.Show("Ya existe una carpeta con ese nombre!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        rootDir Folder = new rootDir();
                        Folder.newFolder(nombreCarpeta, DateTime.Now);
                        crearCarpetaEnRoot(Folder);
                        leerRootDirectory();
                        cargarView(true);
                    }
                }
                else
                {
                    int existecarpeta = 0;
                    foreach (var v in listaDirectorioActual)
                    {
                        string a = Encoding.ASCII.GetString(v.filename);
                        string nombre = a.Replace("\0", string.Empty);
                        if (nombre == nombreCarpeta)
                        {
                            existecarpeta = 1;
                        }
                    }

                    if (existecarpeta > 0)
                    {
                        MessageBox.Show("Ya existe una carpeta con ese nombre!", "Informacion" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        rootDir Folder = new rootDir();
                        Folder.newFolder(nombreCarpeta, DateTime.Now);
                        crearCarpeta(Folder);
                        loadFolder(folderActual.startingCluster);
                        cargarView(false);
                    }
                }
            }
            else
            {
                MessageBox.Show("El nombre debe ser menor o igual a 8 caracteres", "Informacion" , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BorrarCarpeta_Click(object sender, EventArgs e)
        {
            /* if (viewGeneral.SelectedItems.Count == 0 || viewGeneral.SelectedItems.Count > 1)
             {
                 MessageBox.Show("Seleccione una carpeta"
                                     , "Informacion"
                                     , MessageBoxButtons.OK
                                     , MessageBoxIcon.Error);
             }
             else
             {
                 string nombre = viewGeneral.SelectedItems[0].Text;
                 borrarCarpetaEnRoot(nombre);
                 MessageBox.Show("Carpeta Borrada"
                                     , "Informacion"
                                     , MessageBoxButtons.OK
                                     , MessageBoxIcon.Exclamation);
             }
             leerRootDirectory();
             cargarView(true);*/

            ListViewItem seleccionado = viewGeneral.SelectedItems[0];
            string[] prop = seleccionado.Tag.ToString().Split(',');
            if (prop[0] == "A") 
            {
                ushort cluster = Convert.ToUInt16(prop[1].ToString());
                if (viewRootDirectory)
                {
                    borrarArchivoRootDirectory(cluster);
                    leerRootDirectory();
                    cargarView(true);
                }
                else
                {
                    borrarArchivo(cluster);
                    loadFolder(folderActual.startingCluster);
                    cargarView(false);
                }
                MessageBox.Show("Borrado Con Exito!","Informacion", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }


        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgAbrirArchivo = new OpenFileDialog();
            byte[] archivo;

            if (dlgAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlgAbrirArchivo.FileName))
                {
                    using (FileStream stream = File.OpenRead(dlgAbrirArchivo.FileName))
                    {
                        archivo = new byte[stream.Length];
                        stream.Read(archivo, 0, archivo.Length);
                    }
                    rootDir entradaArchivo = new rootDir();
                    FileInfo info = new FileInfo(dlgAbrirArchivo.FileName);
                    uint filesize = (uint)info.Length;

                    entradaArchivo.newFile(info.Name, 'A', DateTime.Now, 0, filesize);

                    //guardarArchivoEnRoot(entradaArchivo, archivo);

                    if (viewRootDirectory)
                    {
                        guardarArchivoEnRoot(entradaArchivo, archivo);
                        leerRootDirectory();
                        cargarView(true);
                    }
                    else
                    {
                        guardarArchivo(entradaArchivo, archivo);
                        loadFolder(folderActual.startingCluster);
                        cargarView(false);
                    }
                }
            }
        }

        private void Extraer_Click(object sender, EventArgs e)
        {
            /*  string seleccionado = viewGeneral.SelectedItems[0].Text;
              if (viewRootDirectory)
              {
                  byte[] archivo = sacarArchivoDeRoot(seleccionado);

                  SaveFileDialog dlgGuardarArchivo = new SaveFileDialog();
                  dlgGuardarArchivo.FileName = seleccionado;
                  if (dlgGuardarArchivo.ShowDialog() == DialogResult.OK)
                  {
                      if (!File.Exists(dlgGuardarArchivo.FileName))
                      {
                          //File.WriteAllBytes(dlgGuardarArchivo.FileName, archivo);

                          using (Stream file = File.OpenWrite(dlgGuardarArchivo.FileName))
                          {
                              file.Write(archivo, 0, archivo.Length);
                          }

                          MessageBox.Show("Creado Con Exito!",
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

                  }
              }*/

            ListViewItem seleccionado = viewGeneral.SelectedItems[0];
            string[] prop = seleccionado.Tag.ToString().Split(',');

            if (prop[0] == "A") 
            {
                byte[] archivo;
                ushort cluster = Convert.ToUInt16(prop[1].ToString());
                if (viewRootDirectory)
                {
                    archivo = sacarArchivoDeRoot(cluster);

                }
                else
                {
                    archivo = sacarArchivo(cluster);
                }

                SaveFileDialog dlgGuardarArchivo = new SaveFileDialog();
                dlgGuardarArchivo.FileName = seleccionado.Text.ToString();
                if (dlgGuardarArchivo.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(dlgGuardarArchivo.FileName))
                    {
                        using (Stream file = File.OpenWrite(dlgGuardarArchivo.FileName))
                        {
                            file.Write(archivo, 0, archivo.Length);
                        }

                        MessageBox.Show("Creado Con Exito!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("No se puede reemplazar el archivo!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }

                }
            }
        }

       /* private void viewGeneral_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem seleccionado = viewGeneral.SelectedItems[0];

            string[] prop = seleccionado.Tag.ToString().Split(',');
            if (prop[0] == "D")
            {
                if (seleccionado.Text == "..")
                {
                    ushort clusterCarpeta = Convert.ToUInt16(prop[1].ToString());
                    if (clusterCarpeta == 0)
                    {
                        leerRootDirectory();
                        folder.Text = "Usted esta en: Root";
                        cargarView(true);
                    }
                    else
                    {
                        loadFolder(clusterCarpeta);
                        folder.Text = "Carpeta Actual: " + Encoding.ASCII.GetString(folderActual.filename);
                        cargarView(false);
                    }
                }
                else
                {
                    ushort clusterCarpeta = Convert.ToUInt16(prop[1].ToString());
                    loadFolder(clusterCarpeta);
                    folder.Text = "Carpeta Actual: " + Encoding.ASCII.GetString(folderActual.filename);
                    cargarView(false);
                }
            }
        }*/

        public rootDir leerEntradaArchivo(long posicionBytes)
        {
            rootDir result = new rootDir();
            result.father = new rootDir();
            // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
            {
                reader.BaseStream.Position = posicionBytes;
                result.filename = reader.ReadBytes(8);
                result.filenameExt = reader.ReadBytes(3);
                result.fileAttributes = reader.ReadByte();
                result.NT = reader.ReadByte();
                result.millisegundos_Creado = reader.ReadByte();
                result.horaC = reader.ReadUInt16();
                result.fechaC = reader.ReadUInt16();
                result.fechaLastaccess = reader.ReadUInt16();
                result.reservedFAT32 = reader.ReadUInt16();
                result.horaLastwrite = reader.ReadUInt16();
                result.fechaLastwrite = reader.ReadUInt16();
                result.startingCluster = reader.ReadUInt16();
                result.fileSize = reader.ReadUInt32();
                result.byteP = posicionBytes;
            }
            return result;
        }

        public byte[] sacarArchivoDeRoot(ushort cluster)
        {
            var archivo = listaRootDirectory.Where(x => x.startingCluster == cluster).FirstOrDefault();
            byte[] result = new byte[archivo.fileSize];
            long posicionInicioCluster = 0;
            ushort[] clusters = clustersArchivo(archivo);
            if (clusters.Length == 1)
            {
                posicionInicioCluster = posicionByteCluster(archivo.startingCluster);
                // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))

                {
                    reader.BaseStream.Position = posicionInicioCluster;
                    reader.Read(result, 0, (int)archivo.fileSize);
                }
            }
            else
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    posicionInicioCluster = posicionByteCluster(clusters[i]);
                    //using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                    using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
                    {
                        reader.BaseStream.Position = posicionInicioCluster;
                        int offset = 0;
                        if (i > 0) offset = (i * sizeCluster);
                        if (i == clusters.Length - 1)
                        {
                            long bytesSobrantes = archivo.fileSize - (sizeCluster * (clusters.Length - 1));
                            reader.Read(result, offset, (int)bytesSobrantes);
                        }
                        else
                        {
                            reader.Read(result, offset, sizeCluster);
                        }
                    }
                }
            }
            return result;
        }

        public byte[] sacarArchivo(ushort cluster)
        {
            long posicion = posicionByteCluster(cluster);
            var archivo = leerEntradaArchivo(posicion);
            byte[] result = new byte[archivo.fileSize];
            long posicionInicioCluster = 0;
            ushort[] clusters = clustersArchivo(archivo);

            if (archivo.fileSize < sizeCluster - 32)
            {
                posicionInicioCluster = posicion + 32;
                // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
                {
                    reader.BaseStream.Position = posicionInicioCluster;
                    reader.Read(result, 0, (int)archivo.fileSize);
                }
            }
            else
            {
                if (clusters.Length == 1)
                {
                    posicionInicioCluster = posicionByteCluster(archivo.startingCluster);
                    // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                    using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
                    {
                        reader.BaseStream.Position = posicionInicioCluster;
                        reader.Read(result, 0, (int)archivo.fileSize);
                    }
                }
                else
                {
                    for (int i = 0; i < clusters.Length; i++)
                    {
                        posicionInicioCluster = posicionByteCluster(clusters[i]);
                        // using (BinaryReader reader = new BinaryReader(new FileStream(Principal.Default, FileMode.Open)))
                        using (BinaryReader reader = new BinaryReader(new FileStream(Constants.discoDefault, FileMode.Open)))
                        {
                            reader.BaseStream.Position = posicionInicioCluster;
                            int offset = 0;
                            if (i > 0) offset = (i * sizeCluster);

                            if (i == clusters.Length - 1)
                            {
                                long bytesSobrantes = archivo.fileSize - (sizeCluster * (clusters.Length - 1));
                                reader.Read(result, offset, (int)bytesSobrantes);
                            }
                            else
                            {
                                reader.Read(result, offset, sizeCluster);
                            }

                        }
                    }
                }
            }
            return result;
        }

        public void borrarArchivo(ushort cluster)
        {
            long posicion = posicionByteCluster(cluster);
            var archivo = leerEntradaArchivo(posicion);
            ushort[] clusters = clustersArchivo(archivo);
            foreach (ushort c in clusters)
            {
                setmarcadorCluster(c, 0);
            }
            borrarSubDirectorioCarpetaActual(cluster);
            leerTablasFat();
        }

        public void borrarArchivoRootDirectory(ushort cluster)
        {
            var file = listaRootDirectory.Where(x => x.startingCluster == cluster).FirstOrDefault();
            rootDir folder0 = new rootDir();
            // using (BinaryWriter stream = new BinaryWriter(File.Open(Principal.Default, FileMode.Open)))
            using (BinaryWriter stream = new BinaryWriter(File.Open(Constants.discoDefault, FileMode.Open)))
            {
                stream.BaseStream.Position = file.byteP;
                stream.Write(folder0.filename);
                stream.Write(folder0.filenameExt);
                stream.Write(folder0.fileAttributes);
                stream.Write(folder0.NT);
                stream.Write(folder0.millisegundos_Creado);
                stream.Write(folder0.horaC);
                stream.Write(folder0.fechaC);
                stream.Write(folder0.fechaLastaccess);
                stream.Write(folder0.reservedFAT32);
                stream.Write(folder0.horaLastwrite);
                stream.Write(folder0.fechaLastwrite);
                stream.Write(folder0.startingCluster);
                stream.Write(folder0.fileSize);
            }
            ushort[] clusters = clustersArchivo(file);
            foreach (ushort c in clusters)
            {
                setmarcadorCluster(c, 0);
            }
            leerTablasFat();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void BorrarCarpeta_Click_1(object sender, EventArgs e)
        {
            /* if (viewGeneral.SelectedItems.Count == 0 || viewGeneral.SelectedItems.Count > 1)
             {
                 MessageBox.Show("Seleccione una carpeta"
                                     , "Informacion"
                                     , MessageBoxButtons.OK
                                     , MessageBoxIcon.Error);
             }
             else
             {
                 string nombre = viewGeneral.SelectedItems[0].Text;
                 borrarCarpetaEnRoot(nombre);
                 MessageBox.Show("Carpeta Borrada"
                                     , "Informacion"
                                     , MessageBoxButtons.OK
                                     , MessageBoxIcon.Exclamation);
             }
             leerRootDirectory();
             cargarView(true);*/

            ListViewItem seleccionado = viewGeneral.SelectedItems[0];
            string[] prop = seleccionado.Tag.ToString().Split(',');
            if (prop[0] == "A")
            {
                ushort cluster = Convert.ToUInt16(prop[1].ToString());
                if (viewRootDirectory)
                {
                    borrarArchivoRootDirectory(cluster);
                    leerRootDirectory();
                    cargarView(true);
                }
                else
                {
                    borrarArchivo(cluster);
                    loadFolder(folderActual.startingCluster);
                    cargarView(false);
                }
                MessageBox.Show("Borrado Con Exito!", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Borrar_Click(object sender, EventArgs e)
        {
            ListViewItem seleccionado = viewGeneral.SelectedItems[0];
            string[] prop = seleccionado.Tag.ToString().Split(',');
            if (prop[0] == "A") 
            {
                ushort cluster = Convert.ToUInt16(prop[1].ToString());
                if (viewRootDirectory)
                {
                    borrarArchivoRootDirectory(cluster);
                    leerRootDirectory();
                    cargarView(true);
                }
                else
                {
                    borrarArchivo(cluster);
                    loadFolder(folderActual.startingCluster);
                    cargarView(false);
                }
                MessageBox.Show("Borrado Con Exito!","Informacion",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void viewGeneral_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void viewGeneral_DoubleClick_1(object sender, EventArgs e)
        {
            ListViewItem seleccionado = viewGeneral.SelectedItems[0];

            string[] prop = seleccionado.Tag.ToString().Split(',');
            if (prop[0] == "D")
            {
                if (seleccionado.Text == "..")
                {
                    ushort clusterCarpeta = Convert.ToUInt16(prop[1].ToString());
                    if (clusterCarpeta == 0)
                    {
                        leerRootDirectory();
                        folder.Text = "Usted esta en: Root";
                        cargarView(true);
                    }
                    else
                    {
                        loadFolder(clusterCarpeta);
                        folder.Text = "Carpeta Actual: " + Encoding.ASCII.GetString(folderActual.filename);
                        cargarView(false);
                    }
                }
                else
                {
                    ushort clusterCarpeta = Convert.ToUInt16(prop[1].ToString());
                    loadFolder(clusterCarpeta);
                    folder.Text = "Carpeta Actual: " + Encoding.ASCII.GetString(folderActual.filename);
                    cargarView(false);
                }
            }
        }

        private void BorrarCarp_Click(object sender, EventArgs e)
        {
            //Falta generar esto
        }
    }
}
