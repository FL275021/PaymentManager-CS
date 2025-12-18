using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioGastos : IRepositorio<Gasto>
    {
      
        Gasto BuscarGastoPorNombre(string Nombre);
        List<Auditoria> ObtenerAuditoriaPorGasto(int id);
    }
}
