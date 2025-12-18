using DTOs;
using LogicaNegocio.EntidadesDominio;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMVC.Models.Pagos
{
    public class PagoViewModel
    {
        public List<Usuario> Usuarios { get; set; }
        public List<Gasto> Gastos { get; set; }
        public int Id { get; set; }
        public int UsuarioID { get; set; }
        public string Metodo { get; set; } //credito o efectivo
        public string Descripcion { get; set; }
        public int GastoID { get; set; }
        public string TipoPago { get; set; } //recurrente o unico
        public double MontoTotal { get; set; }
    }
}
