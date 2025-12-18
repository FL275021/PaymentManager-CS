using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcepcionesPropias
{
    public class DatosInvalidosException : Exception
    {
        public DatosInvalidosException() {}

        public DatosInvalidosException(string mensaje) : base(mensaje) { }

        public DatosInvalidosException(string mensaje, Exception interna) : base(mensaje, interna) { }

    }
}
