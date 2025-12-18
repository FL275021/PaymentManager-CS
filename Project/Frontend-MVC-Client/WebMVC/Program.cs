using LogicaAccesoDatos.EF;
using LogicaAccesoDatos.Repositorios;
using LogicaAplicacion.CasosUsoConcretos;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaNegocio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string strCon = builder.Configuration.GetConnectionString("MiConexionLocal");
            builder.Services.AddDbContext<EmpresaContext>(options => options.UseSqlServer(strCon));

            // builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp\keys")).SetApplicationName("WebMVC");

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            /*app.Use(async (context, next) => { 
            try { await next(); }
            catch (CryptographicException)
                {
                    context.Response.Cookies.Delete(".WebMVC.Session");
                    context.Response.Redirect(context.Request.Path);
                }}); */
                
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Login}/{id?}");
            app.Run();

        }
    }
}
