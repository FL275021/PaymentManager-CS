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
    public class MappersPago
    {
        public static Pago ToPago(PagoDTO dto)  
        {
            Pago obj = null; 

            if (dto.NumberoRecib == null) // es recurrente
            { 
                obj = new PagoRecurrente()
                {
                    FechaDesde = dto.FechaDesde, 
                    FechaHasta = dto.FechaHasta
                };
            }
            else // es unico 
            {
                obj = new PagoUnico()   
                {
                    NumberoRecib = dto.NumberoRecib
                };
            }       

            obj.Id = dto.Id;
            obj.Descripcion = dto.Descripcion;
            obj.Metodo = dto.Metodo;
            obj.MontoTotal = dto.MontoTotal;
            obj.TipoPago = dto.TipoPago;
            obj.FechaPago = dto.FechaPago;
            obj.GastoID = dto.GastoID;
            obj.UsuarioID = dto.UsuarioID;
            //obj.TipoGasto = new Gasto() { Id = dto.GastoID };
            //obj.Usuario = new Usuario() { Id = dto.UsuarioID };

            return obj;            
        }
 
        public static PagoDTO ToPagoDTO(Pago pag)
        {
            PagoDTO dto = new PagoDTO(); 
                        
            dto.Id = pag.Id;
            dto.Descripcion = pag.Descripcion;
            dto.Metodo = pag.Metodo;
            dto.MontoTotal = pag.MontoTotal;
            dto.TipoPago = pag.TipoPago;
            dto.GastoID = pag.TipoGasto.Id;
            dto.UsuarioID = pag.Usuario.Id;
            dto.FechaPago = pag.FechaPago;
            dto.UsuarioEmail = pag.Usuario.Email;
            dto.GastoNombre = pag.TipoGasto.Nombre; 

            if (pag is PagoUnico)
            {
                dto.NumberoRecib = (pag as PagoUnico).NumberoRecib;
            }
            else
            {
                PagoRecurrente rec = pag as PagoRecurrente;

                dto.FechaDesde = rec.FechaDesde;
                dto.FechaHasta = rec.FechaHasta;
                   }

            return dto;
        }

        public static IEnumerable<PagoDTO> ToListaPagosDTO(IEnumerable<Pago> pagos)
        {
            List<PagoDTO> lista = new List<PagoDTO> ();

            foreach (Pago pag in pagos)
            {
                lista.Add(ToPagoDTO(pag));
            }

            return lista;
        }
    }
}
