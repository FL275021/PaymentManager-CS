using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio; 

namespace LogicaAplicacion.CasosUsoConcretos
{
    public class BajaGasto : IBajaGasto
    {
        public IRepositorioGastos RepositorioGastos { get; set; }

        public BajaGasto(IRepositorioGastos repo)
        {
            RepositorioGastos = repo;
        }

        public void EjecutarBaja(int id)
        {
            RepositorioGastos.Remove(id);
        }
    }
}
