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
    public class GastoDTO 
    {        
        public int Id { get; set; } 
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

}