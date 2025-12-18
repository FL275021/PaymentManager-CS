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
    public class RepositorioGastosBD : IRepositorioGastos
    {
        public EmpresaContext Contexto { get; set; }

        public RepositorioGastosBD(EmpresaContext contexto)
        {
            Contexto = contexto;
        }

        public void Add(Gasto obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de gasto");

            obj.Validar();
            Gasto buscado = BuscarGastoPorNombre(obj.Nombre);

            if (buscado != null)
                throw new DatosInvalidosException("El nombre del gasto ya existe");

            Contexto.Gastos.Add(obj);
            Contexto.SaveChanges();
            RegistrarAuditoria(obj.Id, "Alta", obj.UsuarioID);
        }

        public void Remove(int id)
        {
            Gasto aBorrar = FindById(id);

            if (aBorrar == null) throw new DatosInvalidosException("No existe el gasto a borrar");

            Contexto.Gastos.Remove(aBorrar);
            Contexto.SaveChanges();
            RegistrarAuditoria(aBorrar.Id, "Baja", aBorrar.UsuarioID);
        }

        public List<Gasto> FindAll()
        {
            return Contexto.Gastos.ToList();
        }

        public Gasto FindById(int id)
        {
            return Contexto.Gastos.Where(gasto => gasto.Id == id)
            .OrderBy(Gastos => Gastos.Nombre)
            .AsNoTracking()
            .SingleOrDefault();
        }


        public void Update(Gasto obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de gasto");
            obj.Validar();      //VALIDACION EN LOGNEGOCIO
            Gasto aModificar = FindById(obj.Id);

            if (aModificar.Nombre != obj.Nombre)
            {
                if (obj.Descripcion.Length <= 0) throw new DatosInvalidosException("El gasto no puede estar vacio");
            }
            Contexto.Entry(aModificar).State = EntityState.Detached;
            Contexto.Gastos.Update(obj);
            Contexto.SaveChanges();
            RegistrarAuditoria(obj.Id, "Modificacion", obj.UsuarioID);
        }

        public Gasto BuscarGastoPorNombre(string Nombre)
        {
            Gasto buscado = Contexto.Gastos
                            .Where(gasto => gasto.Nombre == Nombre)
                            .SingleOrDefault();
            return buscado;
        }
 
        //RF6 - OB2
        private void RegistrarAuditoria(int gastoId, string tipoOperacion, int usuB)
        {
            Usuario usuE = Contexto.Usuarios.SingleOrDefault(u => u.Id == usuB);
            Auditoria auditoria = new Auditoria
            {
                GastoId = gastoId,
                TipoOperacion = tipoOperacion,
                Fecha = DateTime.Now,
                Usuario = "Nombre:" + usuE.Nombre + " Apellido:" + usuE.Apellido + " Email:" + usuE.Email
            };
            Contexto.Auditorias.Add(auditoria);
            Contexto.SaveChanges();
        }
 
        public List<Auditoria> ObtenerAuditoriaPorGasto(int gastoId)
        {
            return Contexto.Auditorias
            .Where(a => a.GastoId == gastoId)
            .OrderByDescending(a => a.Fecha)
            .ToList();
        }


    }
}
