using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Archivos.Clases
{
    class ArchivoJson : IArchivo
    {
        public string Ruta
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LibrosJson.json"; }
        }

        public List<Libro> ObtenerLibros()
        {
            List<Libro> lista = new List<Libro>();
            // Necesita hacer using de System.IO (Input / Output)
            // File.Exists me permite validar si un archivo existe
            if (File.Exists(Ruta))
            {
                using (StreamReader sr = new StreamReader(Ruta))
                {
                    string datosJson = sr.ReadToEnd();

                    //Crear serializador
                    DataContractJsonSerializer oDataContractJsonSerializer = new DataContractJsonSerializer(typeof(List<Libro>));
                    MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(datosJson));

                    // Convertirlo en Objetos
                    lista = (List<Libro>)oDataContractJsonSerializer.ReadObject(ms);
                }
            }

            return lista;
            
        }

        public Libro BuscarLibroPorCodigo(int codigo)
        {
            List<Libro> lista = this.ObtenerLibros();
            foreach (Libro libro in lista)
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
            List<Libro> lista = this.ObtenerLibros();
            lista.Add(libro);

            MemoryStream ms = new MemoryStream();

            // Sacar del DataGridView a una lista

            DataContractJsonSerializer oDataContractJsonSerializer = new DataContractJsonSerializer(typeof(List<Libro>));

            oDataContractJsonSerializer.WriteObject(ms, lista);
            string datosJson = Encoding.Default.GetString(ms.ToArray());

            using (StreamWriter sw = new StreamWriter(Ruta, false))
            {
                sw.WriteLine(datosJson);
            }
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
            return "Archivo Json";
        }
    }
}
