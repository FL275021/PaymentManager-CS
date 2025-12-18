
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
    public class MappersAuditoria
    {

             public static Auditoria ToAuditoria(AuditoriaDTO dto)  
        {
            Auditoria obj = new Auditoria(); 
            obj.Id = dto.Id;
            obj.GastoId = dto.GastoId;
            obj.TipoOperacion = dto.TipoOperacion;
            obj.Fecha = dto.Fecha;
            obj.Usuario = dto.Usuario;
            return obj;                    
        }

public static AuditoriaDTO ToAuditoriaDTO(Auditoria aud)
{
    AuditoriaDTO dto = new AuditoriaDTO();

    dto.Id = aud.Id;
    dto.GastoId = aud.GastoId;
    dto.TipoOperacion = aud.TipoOperacion;
    dto.Fecha = aud.Fecha;
    dto.Usuario = aud.Usuario;
    return dto;
}


        public static IEnumerable<AuditoriaDTO> ToListaAuditoriasDTO(IEnumerable<Auditoria> Auditorias)
        {
            List<AuditoriaDTO> lista = new List<AuditoriaDTO>();

            foreach (Auditoria aud in Auditorias)
            {
                lista.Add(ToAuditoriaDTO(aud));
            }

            return lista;
        }
        
          }
}
