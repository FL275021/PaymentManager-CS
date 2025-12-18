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
    public class Login : ILogin
    {
        public IRepositorioUsuarios RepositorioUsuarios { get; set; }

        public Login(IRepositorioUsuarios repo)
        {
            RepositorioUsuarios = repo;   
        }
  
           public UsuarioDTO EjecutarLogin(string email, string password)
        {
            Usuario usuario=RepositorioUsuarios.BuscarPorEmailyPassword(email, password);
            return MapperUsuario.ToUsuarioDTO(usuario);
        }


    }
}
