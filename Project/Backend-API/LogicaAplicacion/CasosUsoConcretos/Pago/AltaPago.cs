using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.Mapeadores; 
using LogicaNegocio.EntidadesDominio; 

namespace LogicaAplicacion.CasosUsoConcretos
{
    public class AltaPago : IAltaPago
    {
        public IRepositorioPagos RepoPagos { get; set; }

        public AltaPago(IRepositorioPagos repo)
        {
            RepoPagos = repo;   
        }

        public void EjecutarAlta(PagoDTO dto)
        {
            Pago pago = MappersPago.ToPago(dto);
            RepoPagos.Add(pago);
        }
    }
}
