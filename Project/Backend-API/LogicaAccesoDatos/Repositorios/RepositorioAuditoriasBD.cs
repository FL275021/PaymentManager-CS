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
    public class RepositorioAuditoriasBD : IRepositorioAuditorias
    {
        public EmpresaContext Contexto { get; set; }

        public RepositorioAuditoriasBD(EmpresaContext contexto)
        {
            Contexto = contexto;
        }

        public void Add(Auditoria obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de Auditoria");

            Contexto.Auditorias.Add(obj);
            Contexto.SaveChanges();

        }

        public void Remove(int id)
        {
            Auditoria aBorrar = FindById(id);

            if (aBorrar == null) throw new DatosInvalidosException("No existe el Auditoria a borrar");

            Contexto.Auditorias.Remove(aBorrar);
            Contexto.SaveChanges();

        }

        public void Update(Auditoria obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de Auditoria");
            Auditoria aModificar = FindById(obj.Id);
            Contexto.Entry(aModificar).State = EntityState.Detached;
            Contexto.Auditorias.Update(obj);
            Contexto.SaveChanges();
        }

        public List<Auditoria> FindAll()
        {
            return Contexto.Auditorias.ToList();
        }

        public Auditoria FindById(int id)
        {
            return Contexto.Auditorias.Where(Auditoria => Auditoria.Id == id)
            .OrderBy(Auditorias => Auditorias.GastoId)
            .AsNoTracking()
            .SingleOrDefault();
        }

 
        //RF6 - OB2
        public void RegistrarAuditoria(int gastoId, string tipoOperacion, int usuB)
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
