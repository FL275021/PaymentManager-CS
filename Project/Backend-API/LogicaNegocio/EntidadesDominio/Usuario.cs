using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using Microsoft.EntityFrameworkCore;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicaNegocio.EntidadesDominio
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario : IValidable 
    { 
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Rol Rol { get; set; }    //Administrador, Gerente, Empleado
        [ForeignKey("EquipoId")]
        public int EquipoId {get; set;}
       
       
        public Usuario() { }
 
        public Usuario(string email, string password, Rol rol, string nombre, string apellido, int equipoId)
        {
            Email = email;
            Password = password;
            Rol = rol; 
            Nombre = nombre;
            Apellido = apellido;
            EquipoId = equipoId;
            Validar();
        } 


        public void Validar()
        {
            if (Nombre==null || Apellido==null  || Password==null)
            {
                throw new DatosInvalidosException("Debe completar todos los campos");
            }
            if(Password.Length < 8)
            {
                throw new DatosInvalidosException("La contraseña debe tener al menos 8 caracteres");
            }
        }

        public bool Equals(Usuario? other)
        {
            return Email == other.Email;
        }
    }
}
