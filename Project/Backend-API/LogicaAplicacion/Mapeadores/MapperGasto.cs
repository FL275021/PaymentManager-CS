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
    public class MappersGasto
    {
        public static Gasto ToGasto(GastoDTO dto)  
        {
            Gasto obj = new Gasto(); 
            obj.Id = dto.Id;
            obj.Nombre = dto.Nombre;
            obj.Descripcion = dto.Descripcion;
            obj.UsuarioID = dto.UsuarioID;
            return obj;                    
        }



        public static GastoDTO ToGastoDTO(Gasto gas)  
        {
            GastoDTO dto = new GastoDTO(); 
            
            dto.Id = gas.Id;
            dto.Nombre = gas.Nombre;
            dto.Descripcion = gas.Descripcion;
            dto.UsuarioID = gas.UsuarioID;


            return dto;
        }

        public static IEnumerable<GastoDTO> ToListaGastosDTO(IEnumerable<Gasto> gastos)
        {
            List<GastoDTO> lista = new List<GastoDTO>();

            foreach (Gasto com in gastos)
            {
                lista.Add(ToGastoDTO(com));
            }

            return lista;
        }

        
    }
}
