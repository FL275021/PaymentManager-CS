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
    public class BajaUsuario : IBajaUsuario
    {
        public IRepositorioUsuarios RepositorioUsuarios { get; set; }

        public BajaUsuario(IRepositorioUsuarios repo)
        {
            RepositorioUsuarios = repo;
        }

        public void EjecutarBaja(int id)
        {
            RepositorioUsuarios.Remove(id);
        }
    }
}
