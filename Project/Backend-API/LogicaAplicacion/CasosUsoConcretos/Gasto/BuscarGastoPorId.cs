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
     public class BuscarGastoPorId : IBuscarGastoPorId
    {
        public IRepositorioGastos RepoGastos { get; set; }

        public BuscarGastoPorId(IRepositorioGastos repo)
        {
            RepoGastos = repo;
        }


        public GastoDTO EjecutarBusqueda(int id)
        {
            Gasto t = RepoGastos.FindById(id);
            return MappersGasto.ToGastoDTO(t);
        }
    }
}
