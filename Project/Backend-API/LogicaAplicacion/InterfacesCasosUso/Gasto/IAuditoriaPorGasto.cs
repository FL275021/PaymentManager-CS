using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.InterfacesCasosUso
{
    public interface IAuditoriaPorGasto 
    {
        IEnumerable<AuditoriaDTO> AuditoriaPorGasto(int id);
    }
} 
