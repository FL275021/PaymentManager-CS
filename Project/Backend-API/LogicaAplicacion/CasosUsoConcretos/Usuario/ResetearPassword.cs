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
    public class ResetearPassword : IResetearPassword 
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public ResetearPassword(IRepositorioUsuarios repo)
        {
            RepoUsuarios = repo;
        }
  
        public string Ejecutar(int usuarioId) 
        {
            return RepoUsuarios.ResetearPassword(usuarioId);
        }
    }
}
