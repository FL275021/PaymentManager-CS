using System;
using ExcepcionesPropias;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using LogicaNegocio.InterfacesDominio;

namespace LogicaNegocio.EntidadesDominio
{
    [Index(nameof(Id), IsUnique = true)] 
    public class Gasto : IValidable

/*
Gasto(-ID:int(autogenerado), -NOMBRE:string, -DESCRIPCION:string)   
Estos gastos son generales y pueden reutilizarse en distintos pagos.*/
    { 
        public int Id { get; set; }
        [ForeignKey("Usuario")] 
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        
        //Los gastos son generales y pueden reutilizarse en distintos pagos.
        //Por ejemplo, el gasto "Auto" puede incluir tanto los gastos mensuales del vehículo como los relacionados con nafta y arreglos.
        //Otro ejemplo es "Afters", que agrupa todos los gastos relacionados con salidas del equipo. 

        public void Validar()
        {
           if (string.IsNullOrEmpty(Nombre)) 
        throw new DatosInvalidosException("El Nombre no puede estar vacío");
    
        if (string.IsNullOrEmpty(Descripcion)) 
            throw new DatosInvalidosException("La Descripción no puede estar vacía");
        }
    }
}
