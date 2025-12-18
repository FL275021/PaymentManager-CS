using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.EntidadesDominio
{
    public class PagoUnico : Pago, IValidable
    {//Se pagan 1 sola vez, se registra la fecha de pago, los atributos comunes y ademas se registra tambien el numero de recibo de pago    

        public int? NumberoRecib { get; set; }
     
        private void ValidarUsuario() 
        {
            if (Usuario == null)
            {
                throw new DatosInvalidosException("El Usuario asociado a el Pago es obligatorio");
            }
        }

        private void ValidarMetodo()
        {
            if (!Metodo.Trim().ToUpper().Equals("CREDITO") || !Metodo.Trim().ToUpper().Equals("EFECTIVO"))
            {
                throw new DatosInvalidosException("El estado no es correcto");
            }
        }   

        public void Validar()
        {
            ValidarUsuario();
            ValidarMetodo();
        }

          public bool Equals(Pago? other)
        {
            return other.Id.Equals(Id);
        }
    }
}
