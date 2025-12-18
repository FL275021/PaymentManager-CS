using DTOs;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using LogicaAplicacion.Mapeadores;

namespace LogicaAplicacion.Mapeadores
{
    public class MappersEquipo
    {  
  
        public static Equipo ToEquipo(EquipoDTO dto)
        { 
            Equipo Equ = new Equipo()
            {
                Id = dto.Id,   
                Nombre =  dto.Nombre,
            };
                        
            return Equ;
        } 

         public static EquipoDTO ToEquipoDTO(Equipo Equ)
        {
            EquipoDTO dto = null;

            if (Equ != null)
            {
                dto = new EquipoDTO()
                {
                    Id = Equ.Id,
                    Nombre = Equ.Nombre,
                    Usuarios = Equ.UsuariosEnEquipo.Select(
                    u => MapperUsuario.ToUsuarioDTO(u)).ToList(),
                };
            }            
            return dto;
        }

        public static List<EquipoDTO> ToListaEquipoDTO(List<Equipo> Equs)
        {
            List<EquipoDTO> lista = new List<EquipoDTO> ();
 
            foreach (Equipo Equ in Equs) {
                EquipoDTO dto = ToEquipoDTO(Equ);
                lista.Add(dto);
            }            

            return lista;
        }
    }
}
