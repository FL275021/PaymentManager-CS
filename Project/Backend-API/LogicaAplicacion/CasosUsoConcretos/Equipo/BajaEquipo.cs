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
    public class BajaEquipo : IBajaEquipo
    {
        public IRepositorioEquipos RepositorioEquipos { get; set; }

        public BajaEquipo(IRepositorioEquipos repo)
        {
            RepositorioEquipos = repo;
        }

        public void EjecutarBaja(int id)
        {
            RepositorioEquipos.Remove(id);
        }
    }
}
