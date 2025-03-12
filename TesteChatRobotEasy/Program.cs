using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    private static HttpClient _httpClient = new HttpClient();
    private static string _jwtToken;
    private static string _usuarioLogado;
    private static HubConnection _hubConnection;
    private static List<(string, string, DateTime)> _historicoMensagens = new List<(string, string, DateTime)>();

    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== Bem-vindo ao Chat Real-Time ===");

        bool autenticado = await AutenticarUsuario();
        if (!autenticado)
        {
            Console.WriteLine("Falha na autenticação. Encerrando...");
            return;
        }

        await ConectarSignalR();

        while (true)
        {
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1 - Listar Usuários Online");
            Console.WriteLine("2 - Entrar no Chat");
            Console.WriteLine("3 - Sair");
            Console.Write("Opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await ListarUsuariosOnline();
                    break;
                case "2":
                    await EntrarNoChat();
                    break;
                case "3":
                    Console.WriteLine("Saindo...");
                    await _hubConnection.StopAsync();
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }

    static async Task<bool> AutenticarUsuario()
    {
        Console.Write("Digite seu nome de usuário: ");
        _usuarioLogado = Console.ReadLine();

        Console.Write("Digite sua senha: ");
        string senha = Console.ReadLine();

        var loginRequest = new { Username = _usuarioLogado, Password = senha };

        try
        {
            var resposta = await _httpClient.PostAsJsonAsync("http://localhost:5042/Autentication/login", loginRequest);
            if (resposta.IsSuccessStatusCode)
            {
                var resultado = await resposta.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                _jwtToken = resultado["token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
                return true;
            }
            else
            {
                Console.WriteLine("Erro ao autenticar.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na autenticação: {ex.Message}");
            return false;
        }
    }

    static async Task ConectarSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5042/chatHub", options =>
            {
                options.Headers.Add("Authorization", $"Bearer {_jwtToken}");
            })
            .Build();

        _hubConnection.On<string, string>("ReceberMensagem", async (remetente, mensagem) =>
        {
            Console.Write($"\n📩 MENSAGEM RECEBIDA: {remetente} -> {mensagem}");
            _historicoMensagens.Add((remetente, mensagem, DateTime.UtcNow));
            ExibirHistorico();
        });



        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("✅ Conectado ao chat!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao conectar ao SignalR: {ex.Message}");
        }
    }

    public static void MensagemRecebido() { 
        Console.BackgroundColor = ConsoleColor.Green;
    }


    static async Task ListarUsuariosOnline()
    {
        try
        {
            var resposta = await _httpClient.GetAsync("http://localhost:5042/api/chat/usuarios-online");

            if (resposta.IsSuccessStatusCode)
            {
                var usuarios = await resposta.Content.ReadFromJsonAsync<List<string>>();
                Console.WriteLine("\n🟢 Usuários Online:");
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine($" - {usuario}");
                }
            }
            else
            {
                Console.WriteLine("Erro ao listar usuários online.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar usuários online: {ex.Message}");
        }
    }


    static async Task EntrarNoChat()
    {
        await ListarUsuariosOnline();
        Console.Write("\nDigite o nome do usuário para entrar no chat: ");
        string destinatario = Console.ReadLine();

        Console.WriteLine($"📢 Você entrou no chat com {destinatario}. Digite '/sair' para sair.");

        await CarregarHistoricoMensagens(destinatario);
        ExibirHistorico();

        bool continuarChat = true;

        Task.Run(async () =>
        {
            while (continuarChat)
            {
                string mensagem = Console.ReadLine();

                if (mensagem.ToLower() == "/sair")
                {
                    continuarChat = false;
                    Console.WriteLine($"🔚 Saindo do chat com {destinatario}...");
                    break;
                }

                try
                {
                    await _hubConnection.InvokeAsync("EnviarMensagem", destinatario, mensagem);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
                }
            }
        }).Wait();

        while (continuarChat)
        {
            await Task.Delay(100); 
        }
    }

    

    public static async Task SyncChat(CancellationToken cancellationToken)
    {
        while (true)
        {
            await foreach (var mensagem in _hubConnection.StreamAsync<MensagemDto>("ReceberMensagem"))
            {
                Console.WriteLine($"RODOU ASYNC {mensagem}");
            }
        }

    }
 
    static async Task CarregarHistoricoMensagens(string destinatario)
    {
        try
        {
            var resposta = await _httpClient.GetAsync($"http://localhost:5042/api/chat/historico?remetente={_usuarioLogado}&destinatario={destinatario}");
            if (resposta.IsSuccessStatusCode)
            {
                var historico = await resposta.Content.ReadFromJsonAsync<List<MensagemDto>>();
                _historicoMensagens.Clear();
                foreach (var msg in historico)
                {
                    _historicoMensagens.Add((msg.Remetente, msg.Conteudo, msg.EnviadaEm));
                }
            }
            else
            {
                Console.WriteLine("⚠️ Não foi possível carregar o histórico de mensagens.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar histórico: {ex.Message}");
        }
    }

    static void ExibirHistorico()
    {
        Console.Clear();
        Console.WriteLine("===== Histórico do Chat =====");
        foreach (var (remetente, mensagem, data) in _historicoMensagens)
        {
            Console.WriteLine($"[{data:HH:mm}] {remetente}: {mensagem}");
        }
        Console.WriteLine("============================");
    }
}

public class MensagemDto
{
    public string Remetente { get; set; }
    public string Destinatario { get; set; }
    public string Conteudo { get; set; }
    public DateTime EnviadaEm { get; set; } 
}
