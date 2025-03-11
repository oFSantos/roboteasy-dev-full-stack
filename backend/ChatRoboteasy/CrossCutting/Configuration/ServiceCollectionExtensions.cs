using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatRoboteasy.Infrastructure.Data;
using ChatRoboteasy.Domain.Interfaces; // Certifique-se de que este namespace contém o seu DbContext

namespace ChatRoboteasy.Infrastructure.CrossCutting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCrossCuttingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Obtém a string de conexão definida no appsettings.json
            var connectionString = configuration.GetConnectionString("PostgresSql");


            // Configura o DbContext para usar PostgreSQL
            services.AddDbContext<ChatRoboteasyContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IAutenticationService, UsuarioService>();

            // Aqui você pode registrar outros serviços crosscutting, como Logging, Caching, etc.

            return services;
        }
    }
}
