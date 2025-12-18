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
    public class BuscarUsuarioPorId : IBuscarUsuarioPorId
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public BuscarUsuarioPorId(IRepositorioUsuarios repo)
        {
            RepoUsuarios = repo;
        }


        public UsuarioDTO EjecutarBusqueda(int id)
        {
            Usuario t = RepoUsuarios.FindById(id);
            return MapperUsuario.ToUsuarioDTO(t);
        }
    }
}
