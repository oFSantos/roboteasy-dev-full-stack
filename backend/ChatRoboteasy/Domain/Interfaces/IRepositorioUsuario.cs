using ChatRoboteasy.Domain.Enitities;

namespace ChatRoboteasy.Domain.Interfaces
{
    public interface IRepositorioUsuario
    {
        public Task<EntidadeUsuario> BuscarPorNomeAsync(string nomeUsuario);
        public Task<bool> CadastrarUsuario (EntidadeUsuario usuario);
    }
}
