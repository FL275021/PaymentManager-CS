using DTOs;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.Mapeadores
{
    public class MapperUsuario
    {

        public static Usuario ToUsuario(UsuarioDTO dto)
        { 
            Usuario usuario = new Usuario()
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Password = dto.Password,
                Rol = new Rol { Id = dto.Rol.Id, Nombre = dto.Rol.Nombre } 
            };
                        
            return usuario;
        } 
 
        public static UsuarioDTO ToUsuarioDTO(Usuario usuario)
        {
            UsuarioDTO dto = null;

            if (usuario != null)
            {
                dto = new UsuarioDTO()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    Password = usuario.Password,
                    Rol = new Rol { Id = usuario.Rol.Id, Nombre = usuario.Rol.Nombre },
                };
            }            
            return dto;
        }
        
        public static List<UsuarioDTO> ToListaUsuarioDTO(List<Usuario> usuarios)
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO> ();
 
            foreach (Usuario usuario in usuarios) {
                UsuarioDTO dto = ToUsuarioDTO(usuario);
                lista.Add(dto);
            }            

            return lista;
        }
    }
}
