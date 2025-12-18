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
    public class AltaGasto : IAltaGasto
    {
        public IRepositorioGastos RepoGastos { get; set; }

        public AltaGasto(IRepositorioGastos repo)
        {
            RepoGastos = repo;   
        }

        public void EjecutarAlta(GastoDTO dto) 
        {
            Gasto envio = MappersGasto.ToGasto(dto);
            RepoGastos.Add(envio);
        }
    }
}
