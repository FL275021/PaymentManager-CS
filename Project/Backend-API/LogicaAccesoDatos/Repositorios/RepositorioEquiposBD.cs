using ExcepcionesPropias;
using LogicaAccesoDatos.EF;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
 
namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioEquiposBD : IRepositorioEquipos
    {
        public EmpresaContext Contexto { get; set; }

        public RepositorioEquiposBD(EmpresaContext contexto)
        {
            Contexto = contexto;
        }

        public void Add(Equipo obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de Equ");

            obj.Validar();
            Equipo buscado = BuscarEquipoPorNombre(obj.Nombre);

            if (buscado != null)
                throw new DatosInvalidosException("El nombre del Equ ya existe");
            
            Contexto.Equipos.Add(obj);
            Contexto.SaveChanges();            
        }

        public void Remove(int id)
        { 
           Equipo aBorrar = FindById(id);
 
            if (aBorrar == null) throw new DatosInvalidosException("No existe el equipo a borrar");

            if (aBorrar.UsuariosEnEquipo != null) throw new DatosInvalidosException("No se puede eliminar el equipo, porque está en uso");

            Contexto.Equipos.Remove(aBorrar);
            Contexto.SaveChanges();
        }

        public List<Equipo> FindAll()
        {
            return Contexto.Equipos.ToList();
        }

        public Equipo FindById(int id)
        {
            return Contexto.Equipos.Where(equipo => equipo.Id == id)
            .OrderBy(Equipo => Equipo.Nombre)
            .AsNoTracking()
            .SingleOrDefault();
        }


        public List<Equipo> EquiposComienzanConInicial(char inicial)
        {
            return Contexto.Equipos
                    .Where(Equ => Equ.Nombre.StartsWith(inicial))
                    .ToList();
        }


        public List<Equipo> EquiposContienenTexto(string texto)
        {
            return Contexto.Equipos
                    .Where(Equ => Equ.Nombre.Contains(texto))
                    .ToList();
        }

      
        public void Update(Equipo obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de Equipo");
            obj.Validar();
            Equipo aModificar = FindById(obj.Id);
            if (aModificar.Nombre != obj.Nombre)
            {
                Equipo buscado = BuscarEquipoPorNombre(obj.Nombre);
                if (buscado != null)
                    throw new DatosInvalidosException("El nombre del Equipo ya existe");
            }
            Contexto.Entry(aModificar).State = EntityState.Detached;
            Contexto.Equipos.Update(obj);
            Contexto.SaveChanges();
        }


        public Equipo BuscarEquipoPorNombre(string nombre)
        {
            Equipo buscado = Contexto.Equipos
                            .Where(Equ => Equ.Nombre == nombre)
                            .SingleOrDefault();
            return buscado;
        }
         

         // RF4   
        public List<Equipo> PagosMayoresA(decimal monto)
        {
            var equiposConPagos = Contexto.Pagos    //se usa var en vez de List para que arme solo una query, sino hace 2, uno por cada List creado
                .Where(p => p.TipoPago == "unico" && p.MontoTotal > monto)
                .Select(p => p.Usuario.EquipoId) 
                .Distinct();

            List<Equipo> equipos = Contexto.Equipos     
                .Where(e => e.UsuariosEnEquipo.Any(u => equiposConPagos.Contains(u.Id)))
                .OrderByDescending(e => e.Nombre)
                .ToList();
             
            return equipos;
        }

       
    }
}
