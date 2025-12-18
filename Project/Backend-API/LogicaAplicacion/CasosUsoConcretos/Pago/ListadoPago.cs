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
    public class ListadoPagos : IListadoPagos
    {
        public IRepositorioPagos RepoPagos { get; set; }

        public ListadoPagos(IRepositorioPagos repo) 
        {
            RepoPagos = repo;
        }
 
        public List<PagoDTO> ObtenerListado()
        {    
            List<Pago> pagos = RepoPagos.FindAll();
            List<PagoDTO> dtos = MappersPago.ToListaPagosDTO(pagos).ToList();
            return dtos;
        }
        
        

   
    }
}
