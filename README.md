# Konoha's - Sistema de Gestão de Restaurante

Projeto desenvolvido para a disciplina de Tópicos III.

O sistema tem como objetivo gerenciar um restaurante, permitindo cadastro de clientes, login, cardápio, pedidos, reservas, Sugestão do Chefe e relatórios.

## Tecnologias utilizadas

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML
- CSS
- JavaScript
- Bootstrap

## Funcionalidades

- Cadastro e login de clientes
- Cadastro de múltiplos endereços
- Consulta ao cardápio
- Separação de produtos por almoço e jantar
- Sugestão do Chefe com desconto de 20%
- Criação de pedidos
- Atendimento presencial
- Delivery próprio
- Delivery por aplicativo
- Reserva de mesas para almoço
- Relatórios administrativos

## Regras principais

- Apenas uma Sugestão do Chefe por dia e por período.
- O desconto da Sugestão do Chefe é de 20%.
- Pedidos de almoço só aceitam produtos de almoço.
- Pedidos de jantar só aceitam produtos de jantar.
- Delivery próprio possui taxa fixa.
- Delivery por aplicativo cobra 4% no almoço e 6% no jantar.
- Reservas são permitidas apenas para almoço.
- Reservas devem ser feitas com pelo menos 1 dia de antecedência.
- Reservas só podem ser feitas entre 11h e 14h.

## Como executar

Clone o repositório:

    git clone https://github.com/higorvaluar/RestauranteApp.git

Entre na pasta do projeto:

    cd RestauranteApp

Restaure os pacotes:

    dotnet restore

Aplique as migrations:

    dotnet ef database update --project RestauranteApp --startup-project RestauranteApp

Execute o projeto:

    dotnet run --project RestauranteApp

Também é possível executar pelo Visual Studio usando F5.

## Banco de dados

O projeto utiliza SQL Server.

A string de conexão pode ser ajustada no arquivo:

    RestauranteApp/appsettings.json

Exemplo de string de conexão:

    "DefaultConnection": "Server=localhost;Database=RestauranteDB;Trusted_Connection=True;TrustServerCertificate=True;"

## Administrador

Usuários cadastrados pela tela pública são clientes comuns.

Para tornar um usuário administrador, execute no SQL Server:

    UPDATE Clientes
    SET Admin = 1
    WHERE Email = 'email-do-admin@exemplo.com';

O administrador pode acessar:

- Sugestão do Chefe
- Relatórios

## Estrutura do projeto

RestauranteApp/
- Controllers/
- Data/
- Models/
- ViewModels/
- Views/
- wwwroot/
- Program.cs

## Observação

Projeto acadêmico desenvolvido com ASP.NET Core MVC, Entity Framework Core e SQL Server.
