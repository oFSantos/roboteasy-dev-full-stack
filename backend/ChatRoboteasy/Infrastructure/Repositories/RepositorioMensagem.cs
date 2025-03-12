using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoboteasy.Domain.Enitities;
using ChatRoboteasy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RepositorioMensagem : IRepositorioMensagem
{
    private readonly ChatRoboteasyContext _contexto;

    public RepositorioMensagem(ChatRoboteasyContext contexto)
    {
        _contexto = contexto;
    }

    public async Task SalvarMensagemAsync(EntidadeMensagem mensagem)
    {
        _contexto.Mensagens.Add(mensagem);
        await _contexto.SaveChangesAsync();
    }

    public async Task<List<EntidadeMensagem>> ObterHistoricoAsync(string remetente, string destinatario)
    {
        return await _contexto.Mensagens
            .Where(m => (m.Remetente == remetente && m.Destinatario == destinatario) ||
                        (m.Remetente == destinatario && m.Destinatario == remetente))
            .OrderBy(m => m.EnviadaEm)
            .ToListAsync();
    }


}
