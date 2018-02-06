using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Archivos.Clases
{
    [DataContract]
    class Libro
    {
        // Propiedades
        [DataMember] 
        public int Codigo { get; set; }

        [DataMember] 
        public string Titulo { get; set; }

        [DataMember] 
        public string Autor { get; set; }

        public double Precio { get; set; }
    }
}
