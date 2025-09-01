# 🧭 Arquitetura & Padrões — Paschoalotto.People (*docs/architecture-overview.md*)

> **Target audience**: time de engenharia, QA e SRE. Documento orientado a implementação (hands-on) e decisões arquiteturais.

## 0) Visão Geral do Monorepo

```
/src
  ├─ Paschoalotto.People.Domain
  ├─ Paschoalotto.People.Application
  ├─ Paschoalotto.People.Infrastructure
  ├─ Paschoalotto.People.CrossCutting
  └─ Paschoalotto.People.Api
/tests
  ├─ Paschoalotto.People.UnitTests
  └─ Paschoalotto.People.IntegrationTests
/docs
  ├─ implementation-guidelines.md
  └─ architecture-overview.md   (este arquivo)
```

- **Linguagem padrão do código**: **Inglês** (classes, métodos, propriedades, DTOs).  
- **Logs e mensagens operacionais**: **Português**, para acelerar troubleshooting com times locais.  
- **Motivação**: Inglês **universaliza** manutenção/contribuição (contratações globais, Copilot/IA, integração com bibliotecas), reduz ambiguidade e alinha com **mercado enterprise**. Logs em PT **maximizam clareza** no NOC/Suporte e incident response (quem atende fala PT).

---

## 1) Paschoalotto.People.Domain (Core Domain)

**Responsabilidade**: *Fonte da verdade de negócio*. Somente **Entidades**, **Value Objects**, **Enums** e **regras**. Sem dependências de infraestrutura.

- **Entidades**: `Person` (abstrata), `Individual`, `LegalEntity`.
  - Métodos de negócio (e.g., `UpdatePhoto`, `UpdateLogo`) garantem invariantes.
- **Value Objects**: `Cpf`, `Cnpj`, `EmailAddress`, `PhoneNumber`, `Address`.
  - Encapsulam validação e normalização; evitam *primitive obsession*.
- **Padrões aplicados**:
  - **DDD Tactical** (Entities/VOs), **Encapsulamento de Invariantes**, **Fail-Fast** (ex.: `ArgumentException` quando inválido).
- **Justificativa**:
  - Domínio **puro** facilita testes, refatoração e *portabilidade* (infra pode trocar sem tocar regra).

**Dependências**: **zero** (exceto BCL).

---

## 2) Paschoalotto.People.Application (Orquestração & Portas)

**Responsabilidade**: contratos/portas da aplicação (interfaces), DTOs de *use cases* se necessário, e **Unit of Work** + **Repositories** (abstrações).

- **Interfaces**:
  - `IPersonReadRepository`, `IPersonWriteRepository`, `IUnitOfWork`, `IFileStorageService`, `ITokenService`.
- **Por que não temos “Services” gordos aqui na POC?**
  - Para *lead time* curto, os **controllers** orquestram repositórios diretamente.  
  - **Evolução natural**: se o domínio crescer, introduzimos **Application Services** (ou Handlers CQRS) mantendo contratos aqui.
- **Padrões**: **Ports & Adapters**, **Repository**, **Unit of Work**.
- **Justificativa**:
  - Separa *o que* a app precisa do *como* a infra entrega. Mantém o *core* testável.

**Dependências**: Domain.

---

## 3) Paschoalotto.People.Infrastructure (Adapters)

**Responsabilidade**: detalhes concretos — EF Core, Repositórios, Migrations, Storage, Segurança (JWT).

- **Persistência (EF Core)**:
  - `PeopleDbContext` + `Configurations` por entidade.
  - **SQLite** para a POC (rápido, reprodutível). **Futuro**: Postgres em produção (troca de provider sem tocar Domain).
  - **Migrations** via `dotnet ef`.
- **Repositórios**:
  - `PersonReadRepository` (**AsNoTracking** por padrão → performance nas consultas).
  - `PersonWriteRepository` (operações mutadoras com **tracking** → persistência consistente).
  - **Guideline**: consulta → *read repo*; mutação → *write repo*.
- **Storage**:
  - `FileSystemStorageService` implementa `IFileStorageService`.
  - Salva em `Storage:Root` (p.ex. `./_storage`), retornando *relative paths* (`photos/...`, `logos/...`) para auditoria.
- **Segurança (JWT)**:
  - `JwtTokenService` implementa `ITokenService`; lê *issuer/audience/key/ttl* do `appsettings`.
- **Padrões**: **Repository**, **Adapter**, **Configuration per Aggregate**, **Options via IConfiguration**.
- **Justificativa**:
  - Concentra *framework noise* fora do domínio. Trocas de provider, storage backend e autenticação não quebram o core.

**Dependências**: Application, Domain, EF Core, ASP.NET abstractions.

---

## 4) Paschoalotto.People.CrossCutting (DI & Infra Comum)

**Responsabilidade**: composição de dependências e extensões de bootstrapping compartilhadas.

- **`DependencyInjection`**: registro de repositórios, UoW, storage, token service, etc.
- **Motivação**: centraliza wiring, reduz acoplamento da API com implementações concretas.

**Dependências**: Application, Infrastructure (para registrar implementações).

---

## 5) Paschoalotto.People.Api (Interface HTTP)

**Responsabilidade**: *Edge* da aplicação — Controllers, contratos de entrada/saída, middleware, Swagger, autenticação/autorizações, *static files*.

- **Controllers**:
  - `AuthController`: `POST /api/v1/auth/login` → emite JWT (credenciais fixas via config na POC).
  - `PeopleController`: CRUD parcial + upload:  
    - `POST /individuals` (JSON puro) → cria PF  
    - `POST /legal-entities` (JSON puro) → cria PJ  
    - `POST /individuals/{id}/photo` (multipart) → salva foto  
    - `POST /legal-entities/{id}/logo` (multipart) → salva logo  
    - `GET /people/{id}` → retorna detalhe + `PhotoUrl/LogoUrl`  
    - `GET /people/search` → busca por nome
- **Segurança**:
  - `AddAuthentication().AddJwtBearer(...)` + **Policies**:
    - `People.Read` (GETs) e `People.Write` (POSTs).
  - Swagger configurado com **Bearer**.
- **Arquivos estáticos**:
  - `app.UseStaticFiles("/files")` servindo `Storage:Root`. Isso habilita URLs públicas de imagens para auditoria.
- **Padrões**:
  - **Thin Controllers**, **DTOs explícitos**, **ValidationProblemDetails**, **Versionamento de rota (`/api/v1`)**.
- **Justificativa**:
  - Mantém API *clean*, previsível para consumidores, e ready-to-demo na POC.

**Dependências**: CrossCutting, Application, Domain.

---

## 6) Padrões & Convenções (Enterprise-Ready)

- **Idioma**:
  - **Inglês no código**: alinhamento global, busca/StackOverflow/IA, coerência com frameworks, evita *code-switching* nocivo.
  - **Português nos logs**: times de suporte/incidente leem melhor, reduz MTTR.
- **Naming**:
  - Classes `PascalCase`, métodos/propriedades `PascalCase`, variáveis locais `camelCase`.
  - DTOs com sufixos `Request`/`Response`; **nunca** misturar payload de entrada com saída.
- **Erros & Validações**:
  - Domínio lança `ArgumentException` para invariantes (fail-fast).  
  - API converte para `400` com `ValidationProblemDetails` quando aplicável.
- **Upload de Arquivos**:
  - Endpoints **dedicados** ao upload (separados do create) → simplifica Swagger, isola *concerns* e melhora auditoria.
  - Storage retorna **relative path** que é persistido no DB (trilha de auditoria). URL pública é derivada (`/files/{path}`).
- **Segurança**:
  - JWT com **Issuer/Audience/Key** configuráveis.
  - RBAC via **policies** (`People.Read`, `People.Write`).  
  - **Config as Code**: credenciais **não hard-coded** (appsettings + env vars).
- **Migrations**:
  - Dev: `dotnet ef migrations add <Name>` + `dotnet ef database update`.
  - Ops: gerar script SQL para revisão/execução controlada.
- **Observabilidade**:
  - `ILogger<T>` em pontos críticos (sucesso, validação e exceções).  
  - Em prod, integrar com agregador (ELK/Loki/Seq).

---

## 7) Testes (Qualidade como contrato)

- **UnitTests** (`Paschoalotto.People.UnitTests`):  
  - Foco em **Domain** (VOs, Entidades), *sem* banco. Rápidos, determinísticos.
- **IntegrationTests** (`Paschoalotto.People.IntegrationTests`):  
  - `WebApplicationFactory<Program>`, SQLite **file-backed** temporário e `_storage` temporário.  
  - Flúxos completos: `Create → Upload → Get` para PF e PJ.
- **Racional**:
  - Unit → confiabilidade local das regras.  
  - Integration → contrato externo e efeitos colaterais (DB/FS).

---

## 8) Decisões-Chave (ADR resumido)

- **Inglês no código / PT nos logs**: colaboração global × operação local —> **trade-off** ótimo para o time.  
- **Upload separado** do create: `multipart/form-data` em endpoints próprios → reduz complexidade e erros no Swagger/Postman.  
- **SQLite na POC**: velocidade e *zero-friction*; **Postgres** como target em ambientes reais (compatível via EF).  
- **Repos separados (read/write)**: consultas `AsNoTracking` → performance; mutações tracked → consistência.  
- **JWT & Policies**: RBAC simples, demonstrando maturidade de segurança na POC.

---

## 9) Roadmap Evolutivo

- **Application Services** (ou CQRS) quando regras crescerem.  
- **Auditoria “hard”**: colunas `CreatedAt/UpdatedAt/User` + tabela `AuditLogs`.  
- **File Storage Provider** alternativo (S3, Azure Blob) sem tocar domínio.  
- **Health Checks & Metrics** (Prometheus/OpenTelemetry).  
- **Postgres** + migrações versionadas por ambiente.  
- **Retry/Resiliência** (Polly) para integrações externas.

---

## 10) Playbook Operacional

- **Configuração**: `appsettings.json` < **Environment Variables** (prod) — *12-Factor*.  
- **Segredos**: mover `Jwt:Key`/`Auth:Password` para Secret Manager/KeyVault.  
- **Deploy**: container com volume para `_storage`; CI executa `dotnet test` e publica imagem.  
- **Rollback**: manter scripts SQL versionados (migrations script).

---

> **TL;DR**: Arquitetura em camadas orientada a domínio, código em inglês, logs em português, uploads desacoplados, persistência de *paths* para auditoria, JWT + policies para segurança, e testes cobrindo do domínio ao edge. Pronto para iterar do **MVP** ao **Scale-Up** sem refatoração traumática.
