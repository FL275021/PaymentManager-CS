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
    public class ModificarPago : IModificarPago
    {
        public IRepositorioPagos RepoPagos { get; set; }

        public ModificarPago(IRepositorioPagos repo)
        {
            RepoPagos = repo;
        }

        public void EjecutarModificacion(PagoDTO dto)
        {
            RepoPagos.Update(MappersPago.ToPago(dto));
        }



         }
}
