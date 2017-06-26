using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    [Serializable]
   public class ArbolIndex
    {
        public Hoja raiz { get; set; }

        public ArbolIndex()
        {
            raiz = new Hoja();
            raiz.nuevaHoja();
        }

        public void agregarNodo(Nodo nuevoNodo)
        {
            Nodo resultado = Acciones.insertarNodo(nuevoNodo, raiz);

            if (resultado != null)
            {
                if (raiz.indice == false)
                {
                    raiz = new Hoja();
                    raiz.nuevoIndice();
                    raiz.Nodos.Add(resultado);
                }
                else
                {
                    raiz.Nodos.Add(resultado);
                    raiz.Nodos = raiz.Nodos.OrderBy(x => x.name).ToList();

                    if (raiz.Nodos.Count() > Constants.ordenArbol)
                    {
                        Nodo promovido = Acciones.Promover(raiz);
                        raiz = new Hoja();
                        raiz.nuevoIndice();
                        raiz.Nodos.Add(promovido);
                    }
                }
            }
        }
        ///
    }

    [Serializable]
    public class Nodo
    {
        public string name { get; set; }
        public ushort cluster { get; set; }
        public Hoja Der { get; set; }
        public Hoja Izq { get; set; }

        public Nodo()
        {
            //constructor
        }
        public void entradaIndice(string nombreArchivo, Hoja izquierdo, Hoja derecho)
        {
            name = nombreArchivo;
            cluster = 0;
            Izq = izquierdo;
            Der = derecho;
        }

        public void entradaHoja(string nombreArchivo, ushort clusterArchivo)
        {
            name = nombreArchivo;
            cluster = clusterArchivo;
            Izq = null;
            Der = null;
        }
    }
    ///

    [Serializable]
    public class Hoja
    {
        public bool indice { get; set; }
        public List<Nodo> Nodos { get; set; }
        public Hoja hojaSiguiente { get; set; }

        public Hoja()
        {

        }
        public void nuevoIndice()
        {
            indice = true;
            Nodos = new List<Nodo>();
            hojaSiguiente = null;
        }

        public void nuevaHoja()
        {
            indice = false;
            Nodos = new List<Nodo>();
            hojaSiguiente = null;
        }
    }

    public static class Acciones
    {
        public static Nodo insertarNodo(Nodo nuevoNodo, Hoja hoja)
        {
            if (hoja.indice == false)
            {
                hoja.Nodos.Add(nuevoNodo);
                hoja.Nodos = hoja.Nodos.OrderBy(x => x.name).ToList();
                if (hoja.Nodos.Count() > Constants.ordenArbol)
                {
                    Nodo promovido = Promover(hoja);
                    return promovido;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                int sizeHoja = hoja.Nodos.Count();
                for (int i = 0; i < sizeHoja; i++)
                {
                    Nodo nodoActual = hoja.Nodos.ElementAt(i);

                    int c = string.Compare(nuevoNodo.name, nodoActual.name);
                    if (c == -1)
                    {
                        Nodo promovido = insertarNodo(nuevoNodo, nodoActual.Izq);
                        if (promovido != null)
                        {
                            nodoActual.Izq = promovido.Der;
                        }
                        return promovido;
                    }
                    else
                    {
                        if (i != sizeHoja - 1)
                        {
                            Nodo nodoSiguiente = hoja.Nodos.ElementAt(i + 1);
                            c = string.Compare(nuevoNodo.name, nodoSiguiente.name);
                            if (c == -1)
                            {
                                Nodo promovido = insertarNodo(nuevoNodo, nodoActual.Der);
                                if (promovido != null)
                                {
                                    nodoActual.Der = new Hoja();
                                    nodoActual.Der = promovido.Izq;
                                    nodoSiguiente.Izq = promovido.Der;
                                }
                                return promovido;
                            }
                            else
                            {
                                Nodo promovido = insertarNodo(nuevoNodo, nodoSiguiente.Der);
                                if (promovido != null)
                                {
                                    nodoSiguiente.Der = new Hoja();
                                    nodoSiguiente.Der = promovido.Izq;
                                }
                                return promovido;
                            }
                        }
                        else
                        {
                            Nodo promovido = insertarNodo(nuevoNodo, nodoActual.Der);
                            if (promovido != null)
                            {
                                nodoActual.Der = new Hoja();
                                nodoActual.Der = promovido.Izq;
                            }
                            return promovido;
                        }
                    }
                }
                return null;
            }
        }
        public static Nodo Promover(Hoja hoja)
        {
            Nodo nodoaPromover;
            int posicionMedio = Constants.ordenArbol / 2;
            nodoaPromover = hoja.Nodos.ElementAt(posicionMedio);
            Hoja hijosIzq = new Hoja();
            Hoja hijosDer = new Hoja();

            if (hoja.indice)
            {
                hijosIzq.nuevoIndice();
                hijosDer.nuevoIndice();
                hoja.Nodos.RemoveAt(posicionMedio);
            }
            else
            {
                hijosIzq.nuevaHoja();
                hijosDer.nuevaHoja();
            }

            hijosIzq.Nodos = hoja.Nodos.Take(posicionMedio).ToList();
            hijosDer.Nodos = hoja.Nodos.Skip(posicionMedio).ToList();

            if (hoja.indice)
            {
                hijosIzq.hojaSiguiente = null;
            }
            else
            {
                hijosIzq.hojaSiguiente = hijosDer;
            }
            nodoaPromover.Izq = hijosIzq;
            nodoaPromover.Der = hijosDer;
            return nodoaPromover;
        }

        public static void rotornarArbol(Hoja raiz)
        {
            recorrer(raiz);
            Console.Write("\n");
        }

        public static void recorrer(Hoja hoja)
        {
            if (hoja.indice == true)
            {
                foreach (Nodo n in hoja.Nodos)
                {
                    Console.Write("\n");

                    string nombre = n.name;
                    Console.WriteLine(nombre);
                    Console.Write("\tHijos_Izquierda\n");
                    recorrer(n.Izq);
                    Console.Write("\tHijos_Derecha\n");
                    recorrer(n.Der);
                    Console.WriteLine("____________________________");
                }
            }
            else
            {
                Console.Write("\t");
                foreach (Nodo n in hoja.Nodos)
                {
                    string nombre = n.name;
                    Console.Write(nombre + " ");
                }
                Console.Write("\n");

            }
        }
        public static void saveTree(ArbolIndex arbol)
        {
            string serializationFile = Constants.discoIndice;
            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, arbol);
            }
        }
        public static ArbolIndex LoadTree()
        {
            string serializationFile = Constants.discoIndice;
            ArbolIndex result = new ArbolIndex();
            using (Stream stream = File.Open(serializationFile, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                result = (ArbolIndex)bformatter.Deserialize(stream);
            }
            return result;
        }
    }
}
