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
    public class BuscarEquipoPorId : IBuscarEquipoPorId
    {
        public IRepositorioEquipos RepoEquipos { get; set; }

        public BuscarEquipoPorId(IRepositorioEquipos repo)
        {
            RepoEquipos = repo;
        }


        public EquipoDTO EjecutarBusqueda(int id)
        {
            Equipo t = RepoEquipos.FindById(id);
            return MappersEquipo.ToEquipoDTO(t);
        }
    }
}
