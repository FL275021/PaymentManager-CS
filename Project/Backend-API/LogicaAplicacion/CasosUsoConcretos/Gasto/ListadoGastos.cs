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
    public class ListadoGastos : IListadoGastos
    {
        public IRepositorioGastos RepoGastos { get; set; }

        public ListadoGastos(IRepositorioGastos repo)
        {
            RepoGastos = repo;
        }

        public List<GastoDTO> ObtenerListado()
        {
            List<Gasto> gastos = RepoGastos.FindAll();
            List<GastoDTO> dtos = (List<GastoDTO>)MappersGasto.ToListaGastosDTO(gastos);
            return dtos;            
        }

    }
}
