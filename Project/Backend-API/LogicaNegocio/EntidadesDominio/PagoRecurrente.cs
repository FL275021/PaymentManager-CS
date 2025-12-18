using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.EntidadesDominio
{  
    public class PagoRecurrente : Pago
    {   // -Monto total se calcula multiplicando el monto del pago por la cantidad d emeses del periodo entre ambas fechas
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int MesesRestantes { get; set; } 
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SaldoApagar { get; set; }
 
        public PagoRecurrente() { }

        private void ValidarUsuario() 
        {
            if (Usuario == null)
            {
                throw new DatosInvalidosException("El Usuario asociado a el Pago es obligatorio");
            }
        }

             public void ValidarMetodo()
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
