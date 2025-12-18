using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.EntidadesDominio;

namespace LogicaAplicacion.InterfacesCasosUso
{
    public interface IPagosUsuarioDado
    {
        List<PagoDTO> EjecutarPagosUsuarioDado(int id);
    }
}
