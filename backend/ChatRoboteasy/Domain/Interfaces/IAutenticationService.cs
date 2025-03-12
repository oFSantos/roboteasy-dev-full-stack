using ChatRoboteasy.Domain.Enitities;
using System.Text;

namespace ChatRoboteasy.Domain.Interfaces
{
    public interface IAutenticationService
    {
        public Task RegistrarAsync(string nomeUsuario, string senha);

        
    }
}
