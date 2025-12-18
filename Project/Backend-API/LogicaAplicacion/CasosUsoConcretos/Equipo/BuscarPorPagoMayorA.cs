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
    public class BuscarPorPagoMayorA : IBuscarPorPagoMayorA
    {
        public IRepositorioEquipos RepoEquipos { get; set; }

        public BuscarPorPagoMayorA(IRepositorioEquipos repo)
        {
            RepoEquipos = repo;
        } 
 
        public List<EquipoDTO> EjecutarBusqueda(decimal monto)
        {
            List<Equipo> equipos = RepoEquipos.PagosMayoresA(monto);
            //List<EquipoDTO> dtos = MappersEquipo.ToListaEquipoDTO(equipos);
            return MappersEquipo.ToListaEquipoDTO(equipos);
        }
    } 
} 