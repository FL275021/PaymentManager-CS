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
    public class Auditoria
    {
        public int Id { get; set; } 
        [ForeignKey("Gasto")]
        public int GastoId { get; set; }
        public string TipoOperacion { get; set; } // alta, modificacion, etc
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }

        public Auditoria() { }
    }
} 