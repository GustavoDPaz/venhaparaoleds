# Sistema de Match de Concursos e candidatos
O Projeto foi desenvolvido para facilitar o encontro entre candidatos e editais de concursos públicos. Através de um algoritmo de cruzamento de competências, o sistema identifica quais editais são compatíveis com as profissões de um candidato.

##  Tecnologias Utilizadas
* Linguagem: C#

* Frameworks: ASP.NET Core MVC, Blazor, BootStrap

* ORM: Entity Framework Core

* Banco de Dados: SQLite

* Padrões: Service Layer e Injeção de Dependência

##  Arquitetura da Solução


### Camada de Serviços (Service Layer)

Toda a lógica de negócio pesada foi isolada nos serviços CandidatoMatchService e ConcursoMatchService. Isso mantém os controladores enxutos e foca o processamento onde ele realmente importa.

### Conversão de valores

Como o SQLite não suporta listas nativamente, implementamos um conversor de valores no AppDbContext:

* Gravação: Converte ```List<string>``` em uma única string separada por vírgulas.

* Leitura: Reconstrói a lista original para uso no C#.

### Algoritmo de Match

O sistema resolve o problema de dados não normalizados da seguinte forma:

Normalização: Todas as comparações utilizam ```.Trim()``` para remover espaços em branco e ```StringComparison.OrdinalIgnoreCase``` para ignorar diferenças entre letras maiúsculas e minúsculas.

Interseção Dinâmica: O algoritmo verifica se qualquer profissão do candidato existe na lista de vagas do concurso, permitindo resultados precisos mesmo com múltiplos cargos no edital.

### Tratamento de Erros e Estabilidade

Para garantir a nota máxima em estabilidade e segurança:

* Validação Defensiva: Verificação sistemática de parâmetros nulos ou vazios (ex: CPF e Código) antes de consultas ao banco.

* Prevenção de Exceptions: Métodos de busca retornam new List<T>() em vez de null, garantindo que a interface do usuário nunca falhe por erro de referência nula (NullReferenceException).

## Endpoints da API

### Candidato

* Buscar por CPF

   rota usada na view da solução para buscar concursos compátiveis com o portador do CPF digitado. por meio da rota: ```/Candidatos/BuscarPorCpf```

Exemplo:

```cshtml
<div class="container">
    <h2>Filtrar Concursos por CPF</h2>
    
    <form method="get" class="mb-4" action="/Candidatos/BuscarPorCpf">
        <div class="input-group">
            <input type="number" name="cpf" class="form-control" placeholder="Digite o CPF (ex: 182.845.084-34)" />
            <button type="submit" class="btn btn-primary">Buscar</button>
        </div>
    </form>

        <table class="table table-striped">
            <thead>
            <tr>
                <th>Órgão</th>
                <th>Edital</th>
                <th>Código</th>
                <th>Vagas</th>
            </tr>
            </thead>
            <tbody>
                <tr>
                    <td>IFES - SERRA</td>
                    <td>01/2026</td>
                    <td>61828450843</td>
                    <td>marceneiro, programador</td>
                </tr>
            </tbody>
        </table>
    
</div>
```

* Listar Candidatos

  Retorna um JSON de candidatos disponíveis no sistema, para consulta ou manipulação. por meio da rota: ```/Candidatos/listar```

Exemplo:

```json
[
  {
    "id": 1,
    "nome": "Gustavo Teste",
    "dataNascimento": "01/01/2000",
    "cpf": "123",
    "profissoes": [
      "marceneiro"
    ]
  },
  {
    "id": 2,
    "nome": "ridenil",
    "dataNascimento": "21/05/2006",
    "cpf": "234",
    "profissoes": [
      "planejador",
      "analista de dados"
    ]
  },
  {
    "id": 3,
    "nome": "sixseven",
    "dataNascimento": "67/67/6767",
    "cpf": "67",
    "profissoes": [
      "six",
      "seven"
    ]
  }
]
```

* Cadastrar Candidatos

  esta rota adiciona candidatos ao sistema para executar as consultas e manipulações do mesmo. por meio da rota: ```/Candidatos/adicionar```

<img width="1433" height="811" alt="image" src="https://github.com/user-attachments/assets/a319d8e3-6633-4052-b15c-16d9921e77e7" />


* Retirar Candidatos

   esta rota retira candidatos do sistema e os deixa indísponiveis para consultar ou manipular. por meio da rota: ```Candidatos/retirar/{id}```

<img width="1435" height="464" alt="image" src="https://github.com/user-attachments/assets/036f4d82-e322-4118-95df-aa6530161626" />


### Concurso

* Buscar por Código

   rota usada na view da solução para buscar candidatos compátiveis com o concurso portador do código digitado. por meio da rota: ```/Concursos/BuscarPorCodigo```

Exemplo:

```cshtml
<div class="container">
    <h2>Filtrar Candidatos por Codigo</h2>
    
    <form method="get" class="mb-4" action="/Concursos/BuscarPorCodigo">
    <div class="input-group">
    <input type="number" name="codigo" class="form-control" placeholder="Digite o Código (ex: 123123)" />
    <button type="submit" class="btn btn-primary">Buscar</button>
    </div>
    </form>

        <table class="table table-striped">
            <thead>
            <tr>
            <th>Nome</th>
            <th>Data de Nascimento</th>
            <th>CPF</th>
            <th>Profissões</th>
            </tr>
            </thead>
            <tbody>
            <tr>
                <td>Gustavo Teste</td>
                <td>01/01/2000</td>
                <td>123</td>
                <td>marceneiro</td>
                </tr>
        </tbody>
            </table>
        
</div>
```

* Listar Concursos

  Retorna um JSON de concurso disponíveis no sistema, para consulta ou manipulação. por meio da rota: ```/Concursos/listar```

Exemplo:

```json
[
  {
    "id": 1,
    "orgao": "IFES - SERRA",
    "edital": "01/2026",
    "codigo": "61828450843",
    "vagas": [
      "marceneiro",
      "programador"
    ]
  },
  {
    "id": 2,
    "orgao": "ifesvitoria",
    "edital": "05/2025",
    "codigo": "123123",
    "vagas": [
      "pedreiro"
    ]
  },
  {
    "id": 3,
    "orgao": "sedu",
    "edital": "16/2023",
    "codigo": "123123123123",
    "vagas": [
      "limpador de vidro",
      "programador"
    ]
  }
]
```

* Cadastrar Concursos

  esta rota adiciona concursos ao sistema para executar as consultas e manipulações do mesmo. por meio da rota: ```/Concursos/Criar```

<img width="1431" height="761" alt="image" src="https://github.com/user-attachments/assets/ced71b48-142c-4a78-af4e-1fd6e4cb5ec0" />


* Retirar concursos

   esta rota retira concursos do sistema e os deixa indísponiveis para consultar ou manipular. por meio da rota: ```Concursos/retirar/{id}```

<img width="1433" height="463" alt="image" src="https://github.com/user-attachments/assets/7b7f9da2-2cb8-424d-9b32-bf0769e6d5f3" />



##  Como Executar o Projeto
Pré-requisitos: 

1. Certifique-se de ter o SDK instalado:
```C#
dotnet --version 
```
3. Clonar o repositório:
```C#
git clone https://github.com/seu-usuario/venhaparaoleds.git
```

3. Restaurar dependências
```C#
dotnet restore
```

4. Atualize o Banco de Dados (Migrations):
```C#
dotnet ef database update
```
6. Rodar a aplicação:
```C#
dotnet run
```
Acesse via navegador em: http://localhost:5083

##  Documentação da API (Swagger)

O projeto utiliza Sumários XML em todos os métodos públicos, fornecendo ajuda contextual (IntelliSense) durante o desenvolvimento e alimentando automaticamente a documentação do Swagger.

Acesse via navegador em: http://localhost:5083/swagger

> Desenvolvido como desafio técnico para o IFES - Campus Serra.
