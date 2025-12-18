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
    public class ListadoPagosMensual : IListadoPagosMensual
    {
        public IRepositorioPagos RepoPagos { get; set; }

        public ListadoPagosMensual(IRepositorioPagos repo) 
        {
            RepoPagos = repo;
        }

        public List<PagoDTO> ObtenerListadoMensual(int mes, int año)
        {    
            List<Pago> pagos = RepoPagos.PagosPorMesYAño(mes, año);
            List<PagoDTO> dtos = MappersPago.ToListaPagosDTO(pagos).ToList();
            return dtos;
        }
        
        

   
    }
}
