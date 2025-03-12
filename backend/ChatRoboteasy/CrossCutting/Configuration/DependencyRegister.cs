using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatRoboteasy.Infrastructure.Data;
using ChatRoboteasy.Domain.Interfaces; 

namespace ChatRoboteasy.Infrastructure.CrossCutting
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddCrossCuttingServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            var connectionString = configuration.GetConnectionString("PostgresSql");
            
            services.AddDbContext<ChatRoboteasyContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<IRepositorioMensagem, RepositorioMensagem>();
            services.AddSignalR();
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IAutenticationService, UsuarioService>();            

            return services;
        }
    }
}
