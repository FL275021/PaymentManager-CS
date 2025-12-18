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
    public class ModificarEquipo : IModificarEquipo
    {
        public IRepositorioEquipos RepoEquipos { get; set; }

        public ModificarEquipo(IRepositorioEquipos repo)
        {
            RepoEquipos = repo;
        }

        public void EjecutarModificacion(EquipoDTO dto)
        {
            RepoEquipos.Update(MappersEquipo.ToEquipo(dto));
        }
    }
}
