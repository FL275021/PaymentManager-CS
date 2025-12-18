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
    public class ListadoEquipos : IListadoEquipos
    {
        public IRepositorioEquipos RepoEquipos { get; set; }

        public ListadoEquipos(IRepositorioEquipos repo)
        {
            RepoEquipos = repo;
        } 

        public List<EquipoDTO> ObtenerListado() 
        {
            List<Equipo> equipos = RepoEquipos.FindAll();
            List<EquipoDTO> dtos = MappersEquipo.ToListaEquipoDTO(equipos);
            return dtos;            
        }
    }
}
