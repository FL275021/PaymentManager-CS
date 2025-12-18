using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioEquipos : IRepositorio<Equipo>
    {

        Equipo BuscarEquipoPorNombre(string nombre);
        List<Equipo> PagosMayoresA(decimal monto);

    }
}
