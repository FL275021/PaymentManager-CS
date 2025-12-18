using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DTOs;
using LogicaNegocio.ValueObjects;


namespace WebApi
{
    public class ManejadorJwt
    {
        private static List<DTOs.UsuarioDTO> usuariosPrueba;


        public ManejadorJwt()
        {
            CargarDatos();
        }
        public static void CargarDatos()
        {
            usuariosPrueba = new List<DTOs.UsuarioDTO>() {
                    new DTOs.UsuarioDTO {Email = "uno@gmail.com", Nombre = "uno", Password = "Password1!",Rol = new Rol("ADMIN", 1) },
                    new DTOs.UsuarioDTO {Email = "dos@gmail.com", Nombre = "dos", Password = "Password1!", Rol = new Rol("GERENTE", 2)},
                    new DTOs.UsuarioDTO {Email = "tres@gmail.com", Nombre = "tres", Password = "Password1!",Rol = new Rol("EMPLEADO", 3)},
                    new DTOs.UsuarioDTO {Email = "cuatro@gmail.com", Nombre = "cuatro", Password ="Password1!", Rol = new Rol("EMPLEADO", 3)}
                    };
        }

        public static string GenerarToken(DTOs.UsuarioDTO usuarioDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //clave secreta, generalmente se incluye en el archivo de configuración
            //Debe ser un vector de bytes 

            var clave = Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE");

            //Se incluye un claim para el rol

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                                new Claim(ClaimTypes.Name, usuarioDTO.Nombre),
                                new Claim(ClaimTypes.Role, usuarioDTO.Rol.Nombre)
                            }),
                Expires = DateTime.UtcNow.AddMonths(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(clave),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static DTOs.UsuarioDTO ObtenerUsuario(string email)
        {
            {
                UsuarioDTO usuario = usuariosPrueba.SingleOrDefault(usr => usr.Email.ToUpper().Trim() == email.ToUpper().Trim());
                return usuario;

            }
        }
    }
}
