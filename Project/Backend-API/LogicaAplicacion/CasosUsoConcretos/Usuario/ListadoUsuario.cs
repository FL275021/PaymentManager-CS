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
    public class ListadoUsuarios : IListadoUsuarios
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public ListadoUsuarios(IRepositorioUsuarios repo)
        {
            RepoUsuarios = repo;
        }

        public List<UsuarioDTO> ObtenerListado()
        {
            List<Usuario> usuarios = RepoUsuarios.FindAll();
            Console.WriteLine($"Usuarios: {usuarios?.Count ?? 0}");
            return MapperUsuario.ToListaUsuarioDTO(usuarios); //ya devuelve la lista dto
        } 
    }
}
