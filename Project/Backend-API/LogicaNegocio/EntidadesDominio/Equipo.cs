using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace LogicaNegocio.EntidadesDominio     //Equipo(-ID:int(autogenerado, unique), -NOMBRE:string(unique), -USUARIOS:List<Usuario>)    
{    [Index(nameof(Nombre), IsUnique = true)]
     public class Equipo : IValidable
    { 
        public int Id { get; set; }

        public string Nombre{ get; set; }
        public List<Usuario> UsuariosEnEquipo { get; set; } = new List<Usuario>();


        public Equipo() { }

        public Equipo(string nombre)  
        {
            Nombre= nombre;
            Validar();
        } 

        public void Validar()
        {
             if (Nombre.Length <= 0) throw new DatosInvalidosException("El Nombre no puede estar vacio");
        }
        }   
    }

