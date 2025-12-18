using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;


namespace WebMVC.DTOs
{
    public class RolDTO
    {
        public int Id { get; set;}
        public string? Nombre { get; set; }
    }
}