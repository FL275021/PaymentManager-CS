using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    [Table("PagoTabla")]
    public class PagoDTO
    {
            public int Id { get; set; }
            public int UsuarioID { get; set; } 
            public Usuario usuario { get; set; }
            public string UsuarioEmail { get; set; }
            public string GastoNombre { get; set; }
            public int GastoID { get; set; }
            public string Metodo { get; set; } //credito o efectivo
            public string Descripcion { get; set; }
            public decimal MontoTotal { get; set; } 
            public string TipoPago { get; set; } //recurrente o unico
            public bool EsRecurrente { get; set; }
            public DateTime FechaPago { get; set; }
            ////////////////////////////////////////////////////
            public DateTime? FechaDesde { get; set; }
            public DateTime? FechaHasta { get; set; }
            public int MesesRestantes { get; set; }
            public decimal SaldoPendiente { get; set; }// -SaldoPendiente se calcula multiplicando el monto del pago por la cantidad d emeses del periodo entre ambas fechas
            //////////////////////////////////////////////////
            public int? NumberoRecib { get; set; }       
            public IEnumerable<UsuarioDTO> Usuarios { get; set; }
            public IEnumerable<GastoDTO> Gastos { get; set; }
            
        }
    }
