using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebMVC.DTOs 
{      
    public class AuditoriaDTO 
    {        
        public int Id { get; set; } 
        public int GastoId { get; set; }
        public string TipoOperacion { get; set; } // alta, modificacion, etc
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
    }

}