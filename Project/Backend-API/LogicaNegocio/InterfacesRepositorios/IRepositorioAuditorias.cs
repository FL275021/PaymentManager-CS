using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioAuditorias : IRepositorio<Auditoria>
    {
       public void RegistrarAuditoria(int gastoId, string tipoOperacion, int usuB);
        public List<Auditoria> ObtenerAuditoriaPorGasto(int gastoId);
     }
}
 