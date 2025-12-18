using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace LogicaNegocio.ValueObjects
{
  [Owned]
  public class Rol
  {
    public int Id { get; set; }
    public string Nombre { get; set; }

    public Rol()
    {

    }
    public Rol(string nombre, int id)
    {
      Nombre = nombre;
      Id = id;
    }

    private void Validar()
    {

      if (string.IsNullOrEmpty(Nombre) || Id <= 0)
      {
        //throw new DatosInvalidosException("El rol es obligatorio");
        Console.WriteLine($"⚠️ Rol inválido detectado: Nombre={Nombre}, Id={Id}");
      }
      /*   if(Nombre != "Administrador" || Nombre != "Gerente" || Nombre != "Empleado")
        {
            throw new DatosInvalidosException("El rol debe ser Administrador, Gerente o Empleado");
        } */
    }

  }
}
