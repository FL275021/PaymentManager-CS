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
    public class ModificarUsuario : IModificarUsuario
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public ModificarUsuario(IRepositorioUsuarios repo)
        {
            RepoUsuarios = repo;
        }
 
        public void EjecutarModificacion(UsuarioDTO dto) 
        { 
            RepoUsuarios.Update(MapperUsuario.ToUsuario(dto));
        }
    }
}
