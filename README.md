🚀 VenhaParaOleds - Sistema de Match de Concursos
O VenhaParaOleds é uma plataforma desenvolvida para facilitar o encontro entre candidatos e editais de concursos públicos. Através de um algoritmo de cruzamento de competências, o sistema identifica quais editais são compatíveis com as profissões de um candidato.

🛠️ Tecnologias Utilizadas
Linguagem: C#

Framework: ASP.NET Core MVC

ORM: Entity Framework Core

Banco de Dados: SQLite

Padrões: Service Layer e Injeção de Dependência

🏗️ Arquitetura da Solução
O projeto segue os princípios de Clean Code e separação de responsabilidades:

Models: Representação das entidades Candidato e Concurso.

Data (DbContext): Gerenciamento da persistência e conversão de dados (listas de strings convertidas para armazenamento em texto no SQLite).

Services: Camada onde reside a inteligência do negócio (lógica de match), desacoplada dos controladores.

Controllers: Responsáveis apenas pelo fluxo de requisições e respostas.

🧠 Lógica de Match (O Desafio do Artur)
A solução implementa uma lógica robusta para garantir que variações de digitação não impeçam o cruzamento de dados:

Normalização: Todas as strings são tratadas com .Trim() e comparadas ignorando maiúsculas e minúsculas (StringComparison.OrdinalIgnoreCase).

Comparação de Listas: O algoritmo utiliza LINQ para verificar a interseção entre a lista de profissões do candidato e a lista de vagas do edital.

🚦 Como Executar o Projeto
Pré-requisitos: Ter o .NET SDK instalado.

Clonar o repositório:

Bash

git clone https://github.com/seu-usuario/venhaparaoleds.git
Restaurar dependências e atualizar banco:

Bash

dotnet restore
dotnet ef database update
Rodar a aplicação:

Bash

dotnet run
Acessar: O sistema estará disponível em http://localhost:5083 (ou na porta configurada no seu launchSettings.json).

📖 Documentação da API (Swagger)
O projeto conta com documentação interativa via Swagger. Para visualizar os endpoints de listagem, criação e exclusão, acesse:
http://localhost:5083/swagger
