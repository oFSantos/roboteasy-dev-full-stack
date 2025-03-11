using ChatRoboteasy.Domain.Enitities;

namespace ChatRoboteasy.Domain.Interfaces
{
    public interface IRepositorioUsuario
    {
        public Task<EntidadeUsuario> BuscarPorNomeAsync(string nomeUsuario);
        public Task AdicionarUsuarioAsync(EntidadeUsuario usuario);
     
    }
}
