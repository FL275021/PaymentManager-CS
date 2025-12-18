using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioUsuarios : IRepositorio<Usuario>
    {
        Usuario BuscarUsuarioPorEmail(string email);
        Usuario BuscarPorEmailyPassword(string email, string password);
        List<Usuario> UsuariosQueSuperanMontoDado(int montoDado);
        string GenerarEmail(string nombre, string apellido);
        string ResetearPassword(int usuarioId);  
    }
}
