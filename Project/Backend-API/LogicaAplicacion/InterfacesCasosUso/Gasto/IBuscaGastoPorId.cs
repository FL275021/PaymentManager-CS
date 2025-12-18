using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.InterfacesCasosUso
{
    public interface IBuscarGastoPorId
    {
        GastoDTO EjecutarBusqueda(int id);
    }
}
