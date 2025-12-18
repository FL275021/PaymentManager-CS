using ExcepcionesPropias;
using LogicaAccesoDatos.EF;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioPagosBD : IRepositorioPagos
    {
        public EmpresaContext Contexto { get; set; }


        public RepositorioPagosBD(EmpresaContext contexto)
        {
            Contexto = contexto;
        }

        public void Add(Pago obj)
        { 
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de pago");
            obj.Validar();
            if (obj.MontoTotal <= 0)
                throw new DatosInvalidosException("El monto debe ser mayor a cero");
            Contexto.Pagos.Add(obj);
            Contexto.SaveChanges();        
        }
 
        public void Remove(int id) 
        {
            Pago aBorrar = FindById(id);

            if (aBorrar == null) throw new DatosInvalidosException("No existe el pago a borrar");

            bool enUso = Contexto.Pagos.Any(p => p.Id == id);
            if (enUso)
            throw new DatosInvalidosException("No se puede eliminar el tipo de pago porque está siendo utilizado.");
            Contexto.Pagos.Remove(aBorrar);
            Contexto.SaveChanges();
        }
 
        public List<Pago> FindAll()
        {
            return Contexto.Pagos.ToList();
        }

        public Pago FindById(int id)
        {
            return Contexto.Pagos.Where(pago => pago.Id == id)
            .Include(p => p.TipoGasto)
            .Include(p => p.Usuario.Nombre)
            .AsNoTracking()
            .SingleOrDefault(); 
        }       

         public void Update(Pago obj) 
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos la modificacion del pago.");
            obj.Validar();            
            Pago aModificar = FindById(obj.Id);
            if (aModificar.Id != obj.Id)
            { 
                Pago buscado = FindById(obj.Id);
                if (buscado != null) throw new DatosInvalidosException("El numero del pago ya existe.");
                if (obj.MontoTotal < 0) throw new DatosInvalidosException("El monto total debe ser mayor a 0.");
            }
            Contexto.Entry(aModificar).State = EntityState.Detached;
            Contexto.Pagos.Update(obj);
            Contexto.SaveChanges();
        }

        //RF4 - ob1
        public List<Pago> PagosPorMesYAño(int mes, int año) 
        {
            return Contexto.Pagos
                .Include(p => p.TipoGasto).Include(p => p.Usuario)
                .Where(p => p.FechaPago.Month == mes && p.FechaPago.Year == año)
                .ToList();
        }

        //RF2 - ob2 "Deberá controlar que el usuario utilizado para el filtro sea el usuario que envía la solicitud." ????
        public List<Pago> PagosDeUsuarioDado(int id)
        { 
            return Contexto.Pagos
            .Where(p => p.Usuario.Id == id)
            .OrderBy(p => p.FechaPago)
            .Distinct()
            .ToList();
        }
    }
}
