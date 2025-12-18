using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioPagos : IRepositorio<Pago>
    {
        public List<Pago> PagosPorMesYAño(int mes, int año);
        public List<Pago> PagosDeUsuarioDado(int id);
     }
}
 