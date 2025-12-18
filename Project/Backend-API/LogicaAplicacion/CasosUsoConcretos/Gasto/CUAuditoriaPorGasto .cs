using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.CasosUsoConcretos
{
     public class CUAuditoriaPorGasto  : IAuditoriaPorGasto 
    {
        public IRepositorioGastos RepoGastos { get; set; }

        public CUAuditoriaPorGasto(IRepositorioGastos repo)
        {
            RepoGastos = repo;
        }

 
        public IEnumerable<AuditoriaDTO> AuditoriaPorGasto(int id)
        {
            IEnumerable<Auditoria> a = RepoGastos.ObtenerAuditoriaPorGasto(id);
            return MappersAuditoria.ToListaAuditoriasDTO(a);  
        }
    }
}
