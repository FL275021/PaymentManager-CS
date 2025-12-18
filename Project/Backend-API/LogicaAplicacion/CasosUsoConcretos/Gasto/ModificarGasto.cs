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
    public class ModificarGasto : IModificarGasto
    {
        public IRepositorioGastos RepoGastos { get; set; }  

        public ModificarGasto(IRepositorioGastos repo)
        {
            RepoGastos = repo;
        }

        public void EjecutarModificacion(GastoDTO dto)
        {
            RepoGastos.Update(MappersGasto.ToGasto(dto));
        }

    }
}
