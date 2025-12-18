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
    public class AltaEquipo : IAltaEquipo
    {
        public IRepositorioEquipos RepoEquipos { get; set; }

        public AltaEquipo(IRepositorioEquipos repo)
        {
            RepoEquipos = repo;   
        }

        public void EjecutarAlta(EquipoDTO dto)
        {
            Equipo usuario = MappersEquipo.ToEquipo(dto);
            RepoEquipos.Add(usuario);
        }
    }
}
