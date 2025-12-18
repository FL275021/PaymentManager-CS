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
    public class ListadoUsuariosQueSuperanMontoDado : IUsuariosQueSuperanMontoDado
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public ListadoUsuariosQueSuperanMontoDado(IRepositorioUsuarios repo)
        {
            RepoUsuarios = repo;
        }
 
        public List<UsuarioDTO> UsuariosQueSuperanMontoDado(int montoDado)
        { 
            List<Usuario> usuarios = RepoUsuarios.UsuariosQueSuperanMontoDado(montoDado);
            return MapperUsuario.ToListaUsuarioDTO(usuarios);
        } 
    } 
} 