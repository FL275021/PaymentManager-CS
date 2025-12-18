using LogicaAccesoDatos.EF;
using LogicaAccesoDatos.Repositorios;
using LogicaAplicacion.CasosUsoConcretos;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaNegocio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            //Configuracion de Base de datos
            builder.Services.AddDbContext<EmpresaContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MiConexionLocal"),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerGen(opt=>opt.IncludeXmlComments("M3ERecomendacioens.WebApi.xml"));


            /////////////////////////////////////////////////////////////////
            var clave = Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE=");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(clave),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            ///////////JWT Authentication/////////////////////
            /*            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
                       {
                           opciones.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                           {
                               ValidateIssuerSigningKey = true,
                               IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection
                               (" ").Value!)),
                               ValidateIssuer = false,
                               ValidateAudience = false,
                           };
                       });*/


            //Inicializamos Repositorios
            builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuariosBD>();
            builder.Services.AddScoped<IRepositorioGastos, RepositorioGastosBD>();
            builder.Services.AddScoped<IRepositorioPagos, RepositorioPagosBD>();
            builder.Services.AddScoped<IRepositorioEquipos, RepositorioEquiposBD>();
            //Inicializamos Casos de uso
            builder.Services.AddScoped<IBuscarPorPagoMayorA, BuscarPorPagoMayorA>();
            builder.Services.AddScoped<IPagosUsuarioDado, PagosUsuarioDado>();
            builder.Services.AddScoped<IResetearPassword, ResetearPassword>();
            builder.Services.AddScoped<IAuditoriaPorGasto, CUAuditoriaPorGasto>();
            builder.Services.AddScoped<IListadoGastos, ListadoGastos>();
            builder.Services.AddScoped<IBuscarPagoPorId, BuscarPagoPorId>();
            builder.Services.AddScoped<IAltaUsuario, AltaUsuario>();
            builder.Services.AddScoped<IListadoUsuarios, ListadoUsuarios>();
            builder.Services.AddScoped<IUsuariosQueSuperanMontoDado, ListadoUsuariosQueSuperanMontoDado>();
            builder.Services.AddScoped<IBuscarUsuarioPorId, BuscarUsuarioPorId>();
            builder.Services.AddScoped<IBajaUsuario, BajaUsuario>();
            builder.Services.AddScoped<IModificarUsuario, ModificarUsuario>();
            builder.Services.AddScoped<ILogin, Login>();
            builder.Services.AddScoped<IAltaPago, AltaPago>();
            builder.Services.AddScoped<IListadoPagos, ListadoPagos>();
            builder.Services.AddScoped<IListadoPagosMensual, ListadoPagosMensual>();
            builder.Services.AddScoped<IBuscarPagoPorId, BuscarPagoPorId>();
            builder.Services.AddScoped<IBajaPago, BajaPago>();
            builder.Services.AddScoped<IModificarPago, ModificarPago>();
            builder.Services.AddScoped<IAltaGasto, AltaGasto>();
            builder.Services.AddScoped<IListadoGastos, ListadoGastos>();
            builder.Services.AddScoped<IBuscarGastoPorId, BuscarGastoPorId>();
            builder.Services.AddScoped<IBajaGasto, BajaGasto>();
            builder.Services.AddScoped<IModificarGasto, ModificarGasto>();
            builder.Services.AddScoped<IAltaEquipo, AltaEquipo>();
            builder.Services.AddScoped<IListadoEquipos, ListadoEquipos>();
            builder.Services.AddScoped<IBuscarEquipoPorId, BuscarEquipoPorId>();
            builder.Services.AddScoped<IBajaEquipo, BajaEquipo>();
            builder.Services.AddScoped<IModificarEquipo, ModificarEquipo>();
        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}