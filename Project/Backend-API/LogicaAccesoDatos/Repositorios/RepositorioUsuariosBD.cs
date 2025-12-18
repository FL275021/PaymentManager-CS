using ExcepcionesPropias;
using LogicaAccesoDatos.EF;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioUsuariosBD : IRepositorioUsuarios 
    {
        public EmpresaContext Contexto { get; set; }

        public RepositorioUsuariosBD(EmpresaContext contexto)
        {
            Contexto = contexto;
        } 

        public void Add(Usuario obj) 
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de usuario");

            obj.Validar();
            string email = GenerarEmail(obj.Nombre, obj.Apellido);
            obj.Email = email;
            
            Console.WriteLine($"ingresando usuario: {obj.Email}");
            Contexto.Usuarios.Add(obj);
            Contexto.SaveChanges();
        }

        public void Remove(int id)
        {
            Usuario aBorrar = FindById(id);

            if (aBorrar == null) throw new DatosInvalidosException("No existe el usuario a borrar");

            Contexto.Usuarios.Remove(aBorrar);
            Contexto.SaveChanges();
        }

        public List<Usuario> FindAll() 
        {
            List<Usuario> users = Contexto.Usuarios.ToList();
            Console.WriteLine($"usarios en contexto: {users.Count}");
            return users;
        }

        public Usuario FindById(int id) 
        {
            return Contexto.Usuarios.Where(usuario => usuario.Id == id)
            .SingleOrDefault();

        
        }
    
        public void Update(Usuario obj)
        {
            if (obj == null)
                throw new DatosInvalidosException("No se proporcionan datos para el alta de usuario");

            obj.Validar();

            Usuario aModificar = FindById(obj.Id);

            if (aModificar.Email != obj.Email)
            {
                Usuario buscado = BuscarUsuarioPorEmail(obj.Email);
                if (buscado != null)
                    throw new DatosInvalidosException("El nombre del usuario ya existe");
            }
            Contexto.Entry(aModificar).State = EntityState.Detached;
            Contexto.Usuarios.Update(obj);
            Contexto.SaveChanges(); 
        }

        public Usuario BuscarUsuarioPorEmail(string email)
        {
            return Contexto.Usuarios.Where(usuario => usuario.Email == email).SingleOrDefault();

        }

        public Usuario BuscarPorEmailyPassword(string email, string password)   //LOgin
        {
            return Contexto.Usuarios.AsEnumerable().Where(u => u.Email == email && u.Password == password).SingleOrDefault();
        }


        /*  public Usuario Login(string email, string pass)
          {
              Usuario logueado = Contexto.Usuarios.Where(
                  u => u.Email.ToString() == email && u.Password.ToString() == pass).FirstOrDefault();
              if (logueado == null ) {
                  throw new DatosInvalidosException("Usuario o contraseña incorrecta.");
              }
              return logueado;
          } */


        public string GenerarEmail(string nombre, string apellido)
        {
            string Normalizar(string texto) //los caracteres con tildes y otras alteraciones deberán remplazarse por sus versiones sin la alteración
            {
                string normalizado = texto.Normalize(NormalizationForm.FormD);
                StringBuilder sb = new StringBuilder();
                foreach (char c in normalizado)
                {
                    UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (uc != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(c);
                    } 
                }
                return sb.ToString().Replace('ñ', 'n').Replace('Ñ', 'N');
            }
            string nombreNorm = Normalizar(nombre).ToLower();
            string apellidoNorm = Normalizar(apellido).ToLower();
            string parteNombre = nombreNorm.Length <= 3 ? nombreNorm : nombreNorm.Substring(0, 3);
            string parteApellido = apellidoNorm.Length <= 3 ? apellidoNorm : apellidoNorm.Substring(0, 3);
            string baseEmail = parteNombre + parteApellido + "@laempresa.com";
            string emailFinal = baseEmail; // Verificaa si ya existe en la base de datos
            Random num = new Random();
            while (BuscarUsuarioPorEmail(emailFinal) != null)
            {
                int numero = num.Next(1000, 10000); // 4 dígitos
                emailFinal = parteNombre + parteApellido + numero + "@laempresa.com";
            }
            return emailFinal;  // jua   +   nun    +   1234    +   @laempresa.com
        }

        //RF6 - ob1
        public List<Usuario> UsuariosQueSuperanMontoDado(int montoDado)
        {
            return Contexto.Pagos
                .Include(p => p.Usuario)
                .Where(p => p.MontoTotal > montoDado && p.TipoPago == "unico")
                .Select(p => p.Usuario).Distinct().ToList();
        }

        //RF3 - ob2
        public string ResetearPassword(int usuarioId)
        {
            Usuario usuario = FindById(usuarioId);
            if (usuario == null)
                throw new DatosInvalidosException("No existe ese usuario");
            string nuevaPassword = GenerarPasswordAleatoria();
            usuario.Password = nuevaPassword;
            usuario.Validar(); 
            Contexto.Usuarios.Update(usuario);
            Contexto.SaveChanges();
            return nuevaPassword;
        }

        private string GenerarPasswordAleatoria()
        {
            Random random = new Random();
            const string sopa = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder password = new StringBuilder();
            password.Append(sopa[random.Next(sopa.Length)]);
            for (int i = 0; i < 8; i++)
                password.Append(sopa[random.Next(sopa.Length)]);
            return new string(password.ToString().OrderBy(_ => random.Next()).ToArray());
        }

    }
}
