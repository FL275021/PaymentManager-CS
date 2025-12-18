using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicaAplicacion.CasosUsoConcretos
{
   public class PagosUsuarioDado : IPagosUsuarioDado
{
    public IRepositorioPagos RepoPagos { get; set; }

    public PagosUsuarioDado(IRepositorioPagos repo)
    {
        RepoPagos = repo;
    }

    public List<PagoDTO> EjecutarPagosUsuarioDado(int id)
    {    
        List<Pago> pagos = RepoPagos.PagosDeUsuarioDado(id);
        List<PagoDTO> dtos = MappersPago.ToListaPagosDTO(pagos).ToList();
        return dtos; 
    }
}
}
