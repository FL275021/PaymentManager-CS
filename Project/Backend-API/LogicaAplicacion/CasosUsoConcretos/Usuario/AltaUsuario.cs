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
    public class AltaUsuario : IAltaUsuario
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public AltaUsuario(IRepositorioUsuarios repo)
        {
            RepoUsuarios = repo;    
        }

        public void EjecutarAlta(UsuarioDTO dto)
        {
            Usuario usuario = MapperUsuario.ToUsuario(dto);
            RepoUsuarios.Add(usuario); //email se genera 
        }

         
    }
}
 