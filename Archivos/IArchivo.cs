using Archivos.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivos
{
    interface IArchivo
    {
        string Ruta { get; }

        List<Libro> ObtenerLibros();

        Libro BuscarLibroPorCodigo(int codigo);

        void GuardarLibro(Libro libro);       

        void AgregarLibro(Libro libro);

        void EditarLibro(Libro libro);

        void BorrarLibro(Libro libro);

        void EliminarArchivo();
    }
}
