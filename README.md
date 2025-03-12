1 - Navegue pelo terminal do docker até o diretório do projeto "\roboteasy-dev-full-stack"
2 - Executar o commando  "docker-compose up -d"
3 - Com o container rodando, abra a solução da API em: ./backend/ChatRoboteasy
4 - Execute a aplicação usando Http
5 - Com o swagger aberto em: http://localhost:5042/swagger/index.html utilize a rota "/Usuario/registrar" para cadastrar dois usuarios
(Não coloquei autenticação nessa rota pois como se trata de um cadastro de novos usuários para o chat, mas para as outras rotas é necessário autenticar)
6 - Para testar o chat, vamos abrir 2 instâncias de do nosso "Front" em .\roboteasy-dev-full-stack\TesteChatRobotEasy\bin\Release\net8.0\TesteChatRobotEasy.exe
7 - Acesse o chat com o usuário e senha criados no passo 5
8 - Escolha o método de Listar usuários online ou Entrar no chat
9 - Ao entrar no chat, digite o nome do usuário que está logado na outra instância para entrar no chat com ele
10 - Envie mensagens
11 - Para consultar no banco de dados postgres o histórico, basta acessar o pgadmin que subiu com nosso container em : http://localhost/login
12 - Utilizar as credênciais: Login:admin@example.com  Pass:secretaryship
13 - Adicione novo server: General tab: PostgreSQL
14 - Connection Host: postgres
15 - User: admin, Pass: secretaryship
16 - Após connectar, utilizar a query tool no Database:DesafioRoboteasy e fazer o select: 'select * from "Mensagens"'

