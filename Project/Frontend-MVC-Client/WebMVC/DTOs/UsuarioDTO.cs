using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.ValueObjects;

namespace WebMVC.DTOs 
{
      
    public class UsuarioDTO
    { 
        public int Id { get; set; } 
        public string Email {  get; set; }  
        public string Password { get; set; }
        public Rol Rol {  get; set; }   
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }   
        public int? EquipoId { get; set; }
        public string? Token { get; set; }  
        public IEnumerable<EquipoDTO> Equipos { get; set; }
    }
}
