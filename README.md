# MeuProjeto — API em .NET 10 + EF Core (Estudo de Arquitetura em Camadas)

Repositório de estudos criado para aprender, na prática, como construir uma API em **.NET 10** com **Entity Framework Core** seguindo **boas práticas de arquitetura em camadas**, com foco em criar um **template reutilizável** para futuros projetos com suporte a escalonamento.

> Este README é atualizado conforme o estudo avança. Ele serve tanto como documentação do projeto quanto como registro do aprendizado.

## Objetivo

Construir, do zero, uma API RESTful em .NET 10 usando:

- **Arquitetura em camadas** (Domain, Application, Infrastructure, Api) inspirada em Clean Architecture
- **Entity Framework Core** com **SQL Server**
- **Visual Studio** como editor principal
- Boas práticas que permitam reaproveitar essa estrutura como **template** para novos projetos

## Stack utilizada

| Camada         | Tecnologias                                              |
|----------------|-----------------------------------------------------------|
| Domain         | C# puro (POCOs, sem dependências externas)                |
| Application    | FluentValidation, DTOs, Services                           |
| Infrastructure | Entity Framework Core, SQL Server                          |
| Api            | ASP.NET Core Web API (Controllers), Swagger/OpenAPI         |
| Testes         | xUnit (Unit e Integration Tests)                            |

## Arquitetura do projeto

```
MeuProjeto.sln
│
├── src/
│   ├── MeuProjeto.Domain          → Entidades, regras de negócio, interfaces de repositório
│   ├── MeuProjeto.Application     → Casos de uso, DTOs, Services, Validações
│   ├── MeuProjeto.Infrastructure  → EF Core, DbContext, Repositórios, Migrations
│   └── MeuProjeto.Api             → Controllers, Program.cs, Injeção de Dependência, Swagger
│
└── tests/
    ├── MeuProjeto.UnitTests
    └── MeuProjeto.IntegrationTests
```

### Regra de dependência

```
Api  →  Application  →  Domain
Api  →  Infrastructure  →  Application  →  Domain
```

- `Domain` não depende de nenhuma outra camada.
- `Application` depende apenas de `Domain`.
- `Infrastructure` implementa os contratos definidos em `Domain`/`Application`.
- `Api` apenas orquestra as camadas via Injeção de Dependência.

Essa separação permite trocar tecnologias (ex.: banco de dados) ou escalar partes específicas da aplicação sem impactar as regras de negócio.

## Progresso do estudo

- [x] **Passo 1-2** — Criação da Solution e dos projetos, configuração de referências entre camadas
- [x] **Passo 3** — Instalação dos pacotes NuGet essenciais por camada
- [x] **Passo 4-5** — Modelagem das entidades no Domain e interfaces de repositório (Repository + Unit of Work)
- [x] **Passo 6** — DbContext, Fluent API Configurations e implementação dos repositórios na Infrastructure
- [x] **Passo 7** — Connection String, Program.cs e primeira Migration com EF Core
- [x] **Passo 8** — Camada Application: DTOs, mapeamento manual, Services e validações com FluentValidation
- [ ] **Passo 9** — Controllers, Middleware global de exceções e versionamento de API
- [ ] **Passo 10** — Testes unitários e de integração
- [ ] **Passo 11** — Autenticação e Autorização (JWT)
- [ ] **Passo 12** — Logging estruturado e Observabilidade
- [ ] **Passo 13** — Cache e estratégias de escalonamento
- [ ] **Passo 14** — Containerização (Docker) e pipeline de CI/CD

## Principais decisões e aprendizados

- **Persistence Ignorance**: as entidades do `Domain` não conhecem o EF Core (sem Data Annotations); todo mapeamento fica em classes `IEntityTypeConfiguration<T>` na `Infrastructure`.
- **Encapsulamento de invariantes**: setters privados/protegidos + métodos de negócio (`SetPrice`, `SetName`) garantem que entidades nunca fiquem em estado inválido.
- **Repository + Unit of Work**: abstrai o acesso a dados e centraliza o controle transacional (`SaveChangesAsync`).
- **DTOs em vez de entidades expostas**: a Api nunca expõe entidades do Domain diretamente, evitando acoplamento entre contrato público e modelo interno.
- **Mapeamento manual** (em vez de AutoMapper): mais explícito, testável e sem overhead de reflection — decisão consciente para um projeto-template.
- **Resiliência de conexão**: uso de `EnableRetryOnFailure` no EF Core, pensando em ambientes de nuvem com falhas transientes de rede.

## Como rodar o projeto localmente

1. Configure a connection string em `appsettings.Development.json` (arquivo **não versionado**, veja seção abaixo).
2. Aplique as migrations:
   ```bash
   dotnet ef database update --project src/MeuProjeto.Infrastructure --startup-project src/MeuProjeto.Api
   ```
3. Rode a aplicação pelo Visual Studio ou via CLI:
   ```bash
   dotnet run --project src/MeuProjeto.Api
   ```
4. Acesse o Swagger em `https://localhost:{porta}/swagger`.

## Segurança e arquivos sensíveis

Este repositório **não versiona** arquivos com dados sensíveis, como connection strings, senhas ou chaves. Veja o `.gitignore` para a lista completa. Principais exemplos ignorados:

- `appsettings.Development.json`, `appsettings.Local.json`, `appsettings.Production.json`
- `secrets.json`
- `.env` e variantes
- Certificados (`*.pfx`, `*.key`, `*.pem`, `*.cer`)

Para rodar o projeto, copie o `appsettings.json` (versionado, sem dados reais) para `appsettings.Development.json` e preencha com suas próprias credenciais locais. Alternativamente, use o **User Secrets** do .NET:

```bash
dotnet user-secrets init --project src/MeuProjeto.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "sua-connection-string" --project src/MeuProjeto.Api
```

## Referências de estudo

- [Documentação oficial do EF Core](https://learn.microsoft.com/ef/core/)
- [Documentação oficial do ASP.NET Core](https://learn.microsoft.com/aspnet/core/)
- [FluentValidation](https://docs.fluentvalidation.net/)

---

*Repositório mantido como diário de estudos. Sinta-se à vontade para acompanhar a evolução pelos commits e pela seção de progresso acima.*