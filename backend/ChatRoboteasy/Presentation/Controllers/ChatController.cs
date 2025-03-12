using ChatRoboteasy.Domain.Enitities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IRepositorioMensagem _repositorioMensagem;

   
    public ChatController(IHubContext<ChatHub> hubContext,IRepositorioMensagem repositorioMensagem)
    {
        _hubContext = hubContext;
        _repositorioMensagem = repositorioMensagem;
    }

    /// <summary>
    /// Envia uma mensagem para um usuário específico via SignalR. Precisa estar autenticado
    /// </summary>
    /// 

    [Authorize]
    [HttpPost("enviar")]
    public async Task<IActionResult> EnviarMensagem([FromBody] MensagemDto mensagemDto)
    {
        
        await _hubContext.Clients.All.SendAsync("ReceberMensagem", mensagemDto.Remetente, mensagemDto.Conteudo);
        
        await _repositorioMensagem.SalvarMensagemAsync(new EntidadeMensagem { Remetente = mensagemDto.Remetente, Conteudo = mensagemDto.Conteudo, Destinatario = mensagemDto.Destinatario });

        return Ok("Mensagem enviada via SignalR.");
    }

    /// <summary>
    /// Consulta histórico de mensagens. Precisa estar autenticado
    /// </summary>
    [Authorize]
    [HttpGet("historico")]
    public async Task<IActionResult> ObterHistoricoMensagens([FromQuery] string remetente, [FromQuery] string destinatario)
    {
        if (string.IsNullOrEmpty(remetente) || string.IsNullOrEmpty(destinatario))
            return BadRequest("Os parâmetros 'remetente' e 'destinatario' são obrigatórios.");

        var historico = await _repositorioMensagem.ObterHistoricoAsync(remetente, destinatario);
        return Ok(historico);
    }

    /// <summary>
    /// Obtém a lista de usuários online (simulado). Precisa estar autenticado
    /// </summary>

    [Authorize]
    [HttpGet("usuarios-online")]
    public IActionResult UsuariosOnline()
    {
        return Ok(ChatHub.ObterUsuariosOnline());
    }
}

/// <summary>
/// DTO para enviar mensagens via SignalR.
/// </summary>
public class MensagemDto
{
    public string Remetente { get; set; }
    public string Destinatario { get; set; }
    public string Conteudo { get; set; }
}
