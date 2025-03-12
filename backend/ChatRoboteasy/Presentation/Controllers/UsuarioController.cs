using ChatRoboteasy.Domain.Enitities;
using ChatRoboteasy.Domain.Interfaces;
using ChatRoboteasy.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ChatRoboteasy.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
   
        private readonly ILogger<UsuarioController> _logger;
        private readonly IAutenticationService _autenticationService;
        public UsuarioController(ILogger<UsuarioController> logger,IAutenticationService autenticationService)
        {
            _logger = logger;
            _autenticationService = autenticationService;
        }

        /// <summary>
        /// Não precisa de autenticação para criar conta
        /// </summary>
        /// 
        [Authorize]
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioDto requisicao)
        {
            await _autenticationService.RegistrarAsync(requisicao.NomeUsuario, requisicao.Senha);
            return Ok("Usuário registrado com sucesso!");
        }

        public class UsuarioDto
        {
            public string NomeUsuario { get; set; }
            public string Senha { get; set; }
        }
    }
}
