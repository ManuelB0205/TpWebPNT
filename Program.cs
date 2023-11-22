

using Microsoft.EntityFrameworkCore;
using SaboresCompartidos.Context;
using SaboresCompartidos.Utils;

namespace SaboresCompartidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SaboresCompartidosDatabaseContext>(options => options.UseSqlServer(builder.Configuration["ConnectionString:SaboresCompartidosDBConnection"]));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;                
            });

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(UsuarioAction));
            });

            var app = builder.Build();
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();            

            app.UseRouting();                        

            app.UseAuthorization();            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }       
    }
}