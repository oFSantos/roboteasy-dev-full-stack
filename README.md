<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Guia de Uso</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 20px;
            padding: 20px;
            background-color: #f4f4f4;
        }
        pre {
            background: #eee;
            padding: 10px;
            border-radius: 5px;
            overflow-x: auto;
        }
    </style>
</head>
<body>
    <h1>Guia de Uso</h1>
    <ol>
        <li>Navegue pelo terminal do Docker até o diretório do projeto <code>roboteasy-dev-full-stack</code>.</li>
        <li>Execute o comando:
            <pre>docker-compose up -d</pre>
        </li>
        <li>Com o container rodando, abra a solução da API em:
            <pre>./backend/ChatRoboteasy</pre>
        </li>
        <li>Execute a aplicação usando HTTP.</li>
        <li>Com o Swagger aberto em:
            <pre><a href="http://localhost:5042/swagger/index.html" target="_blank">http://localhost:5042/swagger/index.html</a></pre>
            utilize a rota <code>/Usuario/registrar</code> para cadastrar dois usuários.
        </li>
        <li>Para testar o chat, abra 2 instâncias do frontend localizadas em:
            <pre>./roboteasy-dev-full-stack/TesteChatRobotEasy/bin/Release/net8.0/TesteChatRobotEasy.exe</pre>
        </li>
        <li>Acesse o chat com o usuário e senha criados no passo 5.</li>
        <li>Escolha o método de listar usuários online ou entrar no chat.</li>
        <li>Ao entrar no chat, digite o nome do usuário que está logado na outra instância para iniciar a conversa.</li>
        <li>Envie mensagens.</li>
        <li>Para consultar o histórico no banco de dados PostgreSQL, acesse o pgAdmin:
            <pre><a href="http://localhost:80/login" target="_blank">http://localhost/login</a></pre>
        </li>
        <li>Utilize as credenciais:
            <ul>
                <li><strong>Login:</strong> admin@example.com</li>
                <li><strong>Senha:</strong> secretaryship</li>
            </ul>
        </li>
        <li>Adicione um novo servidor:
            <ul>
                <li><strong>General tab:</strong> PostgreSQL</li>
                <li><strong>Connection Host:</strong> postgres</li>
                <li><strong>Usuário:</strong> admin</li>
                <li><strong>Senha:</strong> secretaryship</li>
            </ul>
        </li>
        <li>Após conectar, utilize a Query Tool no banco de dados <code>DesafioRoboteasy</code> e execute o seguinte comando SQL:
            <pre>SELECT * FROM "Mensagens";</pre>
        </li>
    </ol>
</body>
</html>
