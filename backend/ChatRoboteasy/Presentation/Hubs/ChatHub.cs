using System.Threading.Tasks;
using ChatRoboteasy.Domain.Enitities;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly IRepositorioMensagem _repositorioMensagem;
    private static HashSet<string> _usuariosOnline = new HashSet<string>();

    public ChatHub(IRepositorioMensagem repositorioMensagem)
    {
        _repositorioMensagem = repositorioMensagem;
    }

    public override async Task OnConnectedAsync()
    {
        var nomeUsuario = Context.User.Identity.Name;

        if (!string.IsNullOrEmpty(nomeUsuario))
        {
            _usuariosOnline.Add(nomeUsuario);
            await Clients.All.SendAsync("UsuariosOnline", _usuariosOnline);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var nomeUsuario = Context.User.Identity.Name;

        if (!string.IsNullOrEmpty(nomeUsuario))
        {
            _usuariosOnline.Remove(nomeUsuario);
            await Clients.All.SendAsync("UsuariosOnline", _usuariosOnline);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task EnviarMensagem(string destinatario, string mensagem)
    {
        var remetente = Context.User.Identity.Name;


        if (!string.IsNullOrEmpty(remetente))
        {
            var novaMensagem = new EntidadeMensagem
            {
                Remetente = remetente,
                Destinatario = destinatario,
                Conteudo = mensagem
            };

            await _repositorioMensagem.SalvarMensagemAsync(novaMensagem);
      
            await Clients.User(destinatario).SendAsync("ReceberMensagem", remetente, mensagem);            
            
            await Clients.All.SendAsync("ReceberMensagem", remetente, mensagem);

        }
    }

    public async Task<List<EntidadeMensagem>> ObterHistorico(string destinatario)
    {
        var remetente = Context.User.Identity.Name;
        return await _repositorioMensagem.ObterHistoricoAsync(remetente, destinatario);
    }
    public static IEnumerable<string> ObterUsuariosOnline()
    {
        return _usuariosOnline;
    }
}
