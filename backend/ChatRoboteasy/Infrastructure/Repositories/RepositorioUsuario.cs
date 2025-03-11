using System.Threading.Tasks;
using ChatRoboteasy.Domain.Enitities;
using ChatRoboteasy.Domain.Interfaces;
using ChatRoboteasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RepositorioUsuario:IRepositorioUsuario
{
    private readonly ChatRoboteasyContext _contexto;

    public RepositorioUsuario(ChatRoboteasyContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<EntidadeUsuario> BuscarPorNomeAsync(string nomeUsuario)
    {
        return await _contexto.Usuarios.FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario);
    }

    public async Task AdicionarUsuarioAsync(EntidadeUsuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        await _contexto.SaveChangesAsync();
    }
}
