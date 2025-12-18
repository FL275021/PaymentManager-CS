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
    public class BuscarPagoPorId : IBuscarPagoPorId
    {
        public IRepositorioPagos RepoPagos { get; set; }

        public BuscarPagoPorId(IRepositorioPagos repo)
        {
            RepoPagos = repo;
        }


        public PagoDTO EjecutarBusqueda(int id)
        {
            Pago t = RepoPagos.FindById(id);
            return MappersPago.ToPagoDTO(t);
        }
    }
}
