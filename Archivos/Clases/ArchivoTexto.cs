using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos.Clases
{
    class ArchivoTexto : IArchivo
    {
        public string Ruta
        {
            //Abre la ventana del escritorio para guardar el archivo
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LibrosTexto.txt"; }
        }

        public List<Libro> ObtenerLibros()
        {
            List<Libro> lista = new List<Libro>();

            // Necesita hacer using de System.IO (Input / Output)
            // File.Exists me permite validar si un archivo existe
            if (File.Exists(Ruta))
            {
                // FileStream es la clase encargada de establecer como vamos a abrir el archivo
                FileStream fs = new FileStream(Ruta, FileMode.Open, FileAccess.Read, FileShare.Read);
                // StreamReader es la clase encargada de abrir el archivo y recorrerlo
                StreamReader sr = new StreamReader(fs);

                try
                {
                    string linea = "";

                    // ReadLine lee una linea y la retorna
                    while ((linea = sr.ReadLine()) != null)
                    {
                        // Split: toma un string y lo separa en un arreglo usando un caracter especial
                        string[] dato = linea.Split('|');

                        Libro libro = new Libro();

                        libro.Codigo = Int32.Parse(dato[0]);
                        libro.Titulo = dato[1];
                        libro.Autor = dato[2];
                        libro.Precio = Convert.ToDouble(dato[3]);

                        lista.Add(libro);
                    }

                    // debe ir en un finally (buenas practicas)
                    //sr.Close();

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sr.Close();
                }
            } // final del if

            return lista;
        }

        public Libro BuscarLibroPorCodigo(int codigo)
        {
            foreach (Libro libro in this.ObtenerLibros())
            {
                if (libro.Codigo == codigo)
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
            // Indicar cual archivo y como se va a usar (opcional)
            // path = ruta del archivo
            FileStream fs = new FileStream(Ruta, FileMode.Append, FileAccess.Write, FileShare.Write);
            // Para abrir un archivo y escribir en él
            StreamWriter sw = new StreamWriter(fs);
            //StreamWriter sw = new StreamWriter(Ruta); // manera rápida

            string dato = String.Format("{0}|{1}|{2}|{3}", libro.Codigo, libro.Titulo, libro.Autor, libro.Precio);

            sw.WriteLine(dato);

            sw.Close();
        }

        public void EditarLibro(Libro libro)
        {
            // Procedimiento similar al de borrar
            List<Libro> todos = this.ObtenerLibros();
            // Elimina el archivo y luego va agregando los libros viejos y el actualizado
            this.EliminarArchivo();

            foreach (Libro actual in todos)
            {
                if (actual.Codigo != libro.Codigo)
                {
                    this.AgregarLibro(actual);
                }
                else
                {
                    // libro actualizado: se agrega nuevo
                    this.AgregarLibro(libro);
                }
            }
        }

        public void BorrarLibro(Libro libro)
        {
            // para eliminar un registro necesito borrar el archivo
            // volver a agregar todos los registros MENOS el que iba a eliminar
            List<Libro> todos = this.ObtenerLibros();
            this.EliminarArchivo();

            foreach (Libro actual in todos)
            {
                if (actual.Codigo != libro.Codigo)
                {
                    this.AgregarLibro(actual);
                }
            }
        }

        public void EliminarArchivo()
        {
            // Permite eliminar el archivo en el directorio donde fue creado
            File.Delete(Ruta);
        }

        public override string ToString()
        {
            return "Archivo de Texto";
        }

    }
}
