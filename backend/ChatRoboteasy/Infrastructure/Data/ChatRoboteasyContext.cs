using ChatRoboteasy.Domain.Enitities;
using Microsoft.EntityFrameworkCore;

namespace ChatRoboteasy.Infrastructure.Data
{
    public class ChatRoboteasyContext:DbContext
    {
        public ChatRoboteasyContext(DbContextOptions<ChatRoboteasyContext> options) : base(options) { }
        public DbSet<EntidadeMensagem> Mensagens { get; set; }
        public DbSet<EntidadeUsuario> Usuarios { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntidadeUsuario>()
                .HasIndex(u => u.NomeUsuario)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
