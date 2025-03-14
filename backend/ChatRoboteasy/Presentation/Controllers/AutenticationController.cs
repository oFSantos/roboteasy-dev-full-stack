﻿using ChatRoboteasy.Config;
using ChatRoboteasy.Domain.Interfaces;
using ChatRoboteasy.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AutenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IRepositorioUsuario _usuarioRepository;

    public AutenticationController(IConfiguration configuration, IRepositorioUsuario usuarioRepository)
    {
        _configuration = configuration;
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        // Busca o usuário pelo Username
        var usuario =  await _usuarioRepository.BuscarPorNomeAsync(login.Username);
        if (usuario == null)        
            return Unauthorized("Usuário não encontrado");
        
        
        if (Helper.HashSenha(login.Password) != usuario.SenhaHash) // Apenas para exemplo; substitua por verificação de hash        
            return Unauthorized("Senha incorreta");
        

        // Obter configurações do JWT
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.NomeUsuario),
        };

        // Configuração do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}


