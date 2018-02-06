using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Archivos.Clases
{
    class ArchivoXml : IArchivo
    {
        public string Ruta
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LibrosXml.xml"; }
        }

        public List<Libro> ObtenerLibros()
        {
            List<Libro> lista = new List<Libro>();
            try
            {
                if (File.Exists(Ruta) == false)
                    return lista;

                // Crear documento XML
                XmlDocument doc = new XmlDocument();

                // Cargarlo en el documento de Tipo XML                
                doc.Load(Ruta);

                //lector.Close();
                // Seleccionar los nodos que deseo leer
                XmlNodeList listaNodos = doc.SelectNodes("/LIBROS/LIBRO");

                // Recorrer los nodos
                foreach (XmlNode nodo in listaNodos)
                {
                    Libro libro = new Libro();

                    // InnerText retorna el contenido del nodo:
                    // Ejemplo: <CODIGO>123</CODIGO> => InnerText = 123                    
                    libro.Codigo = Int32.Parse(nodo["CODIGO"].InnerText);
                    libro.Titulo = nodo["TITULO"].InnerText;
                    libro.Autor = nodo["AUTOR"].InnerText;

                    // Agregar el Arreglo en la Lista
                    lista.Add(libro);
                }

                return lista;
            }
            catch
            {
                // Captura de Errores
                throw;
            }
        }

        public Libro BuscarLibroPorCodigo(int codigo)
        {
            // preguntar si el archivo NO existe
            if (File.Exists(Ruta) == false)
                return null;

            XmlDocument doc = new XmlDocument();
            doc.Load(Ruta);

            // Raiz del archivo xml
            XmlElement root = doc.DocumentElement;

            // buscar el Nodo
            XmlNode nodo = root.SelectSingleNode("/LIBROS/LIBRO[CODIGO='" + codigo.ToString() + "']");

            // Si existe NO es null
            if (nodo != null)
            {
                Libro libro = new Libro();
                libro.Codigo = int.Parse(nodo["CODIGO"].InnerText);
                libro.Titulo = nodo["TITULO"].InnerText;
                libro.Autor = nodo["AUTOR"].InnerText;

                return libro;
            }

            return null;
        }

        public void GuardarLibro(Libro libro)
        {
            if (this.BuscarLibroPorCodigo(libro.Codigo) != null)
                this.EditarLibro(libro);
            else
                this.AgregarLibro(libro);
        }

        public void AgregarLibro(Libro libro)
        {
            XmlDocument doc = new XmlDocument();

            //  Si el archivo no existe debe crearlo
            // con la estructura raiz
            if (File.Exists(Ruta) == false)
            {
                // LoadXml carga un string con formato xml
                doc.LoadXml("<LIBROS></LIBROS>");
            }
            else
            {
                // Load carga un archivo con formato xml en una ruta 
                doc.Load(Ruta);
            }
            // DocumentElement = raiz (solo puede existir 1)
            XmlElement root = doc.DocumentElement;

            XmlElement newRegistro = doc.CreateElement("LIBRO");

            newRegistro.InnerXml = "<CODIGO>" + libro.Codigo.ToString() + "</CODIGO>" +
                                   "<TITULO>" + libro.Titulo + "</TITULO>" +
                                   "<AUTOR>" + libro.Autor + "</AUTOR>";

            //root.InsertBefore(newRegistro, root.FirstChild);
            root.AppendChild(newRegistro);

            // Salvar
            doc.Save(Ruta);
        }

        public void EditarLibro(Libro libro)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Ruta);

            //Select the cd node with the matching title            
            XmlElement root = doc.DocumentElement;

            // buscar el Nodo   (/LIBROS/LIBRO[CODIGO='123'])
            XmlNode oldRegistro = root.SelectSingleNode("/LIBROS/LIBRO[CODIGO='" + libro.Codigo.ToString() + "']");

            XmlElement newRegistro = doc.CreateElement("LIBRO");
            newRegistro.InnerXml = "<CODIGO>" + libro.Codigo.ToString() + "</CODIGO>" +
                                   "<TITULO>" + libro.Titulo + "</TITULO>" +
                                   "<AUTOR>" + libro.Autor + "</AUTOR>";

            root.ReplaceChild(newRegistro, oldRegistro);

            // Salvar
            doc.Save(Ruta);
        }

        public void BorrarLibro(Libro libro)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Ruta);

            XmlElement root = doc.DocumentElement;

            // buscar el Nodo
            XmlNode registro = root.SelectSingleNode("/LIBROS/LIBRO[CODIGO='" + libro.Codigo.ToString() + "']");

            if (registro != null)
            {
                root.RemoveChild(registro);               
                // Salvar
                doc.Save(Ruta);
            }            
        }

        public void EliminarArchivo()
        {
            File.Delete(Ruta);
        }

        public override string ToString()
        {
            return "Archivo XML";
        }
    }
}
