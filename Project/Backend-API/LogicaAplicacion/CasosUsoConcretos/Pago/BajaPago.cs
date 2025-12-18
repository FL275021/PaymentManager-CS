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
    public class BajaPago : IBajaPago
    {
        public IRepositorioPagos RepositorioPagos { get; set; }

        public BajaPago(IRepositorioPagos repo)
        {
            RepositorioPagos = repo;
        }

        public void EjecutarBaja(int id)
        {
            RepositorioPagos.Remove(id);
        }
    }
}
