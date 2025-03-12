using ChatRoboteasy.Domain.Enitities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepositorioMensagem
{
    Task SalvarMensagemAsync(EntidadeMensagem mensagem);
    Task<List<EntidadeMensagem>> ObterHistoricoAsync(string remetente, string destinatario);
}
