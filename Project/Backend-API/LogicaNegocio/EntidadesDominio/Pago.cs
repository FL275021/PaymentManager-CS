using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.EntidadesDominio    
{
    [Index(nameof(Id), IsUnique = true)]
    public abstract class Pago : IValidable
    {
        public int Id { get; set; } 
        [ForeignKey("Usuario")] 
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public string Metodo { get; set; } //credito o efectivo
        public string Descripcion { get; set; }
        [ForeignKey("Gasto")] 
         public int GastoID { get; set; }
        public Gasto TipoGasto { get; set; }
        public string TipoPago { get; set; } //recurrente o unico
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MontoTotal { get; set; }
        public DateTime FechaPago{ get; set; }
     

        public Pago()
        {
            FechaPago = DateTime.Now; 
        } 


     

        public void Validar()
        {
        
        }
    }
}
