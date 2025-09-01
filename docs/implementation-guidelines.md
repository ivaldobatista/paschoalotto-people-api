# 📘 Diretrizes de Implementação – Paschoalotto.People.Api

## 1. Arquitetura de Solução
- **Camadas do Projeto:**
  - `Domain`: Entidades e Value Objects (regra de negócio pura).
  - `Application`: Abstrações (interfaces de repositórios, serviços, Unit of Work).
  - `Infrastructure`: Implementações (EF Core, repositórios, storage, JWT).
  - `Api`: Controllers, contratos (requests/responses), configuração e DI.
  - `CrossCutting`: Extensões e injeção de dependência comum.

- **Banco de Dados:**  
  - **SQLite** para a POC (com suporte a migrations EF Core).
  - Tabelas versionadas via `dotnet ef migrations`.

- **Persistência de Arquivos:**  
  - Diretório `_storage` configurado via `appsettings.json`.
  - Uploads separados em subpastas (`photos`, `logos`).

---

## 2. Segurança
- **Autenticação JWT**:
  - Implementada em `JwtTokenService` (camada Infrastructure).
  - Configurações em `appsettings.json`:
    ```json
    "Auth": {
      "Username": "admin",
      "Password": "123456",
      "Role": "admin"
    },
    "Jwt": {
      "Issuer": "Paschoalotto.People.Api",
      "Audience": "Paschoalotto.People.Clients",
      "Key": "<segredo forte>",
      "ExpiresMinutes": 60
    }
    ```

- **Fluxo de Login**:
  - `POST /api/v1/auth/login` com credenciais fixas → retorna token JWT.
  - Credenciais e chave JWT devem ser armazenadas em **configurações seguras** (secret manager, variáveis de ambiente em produção).

- **Autorização baseada em políticas**:
  - `People.Read`: leitura (GET).
  - `People.Write`: escrita (POST/Upload).
  - Controllers usam `[Authorize]` com policies específicas.

---

## 3. Padrões de Implementação
- **Naming Convention:**  
  - Código em inglês (classes, métodos, DTOs).  
  - Logs e mensagens em português para facilitar troubleshooting.

- **Controllers:**
  - Sempre `async`.
  - Seguir contrato REST:
    - `POST /individuals` cria pessoa física.
    - `POST /legal-entities` cria pessoa jurídica.
    - `POST /individuals/{id}/photo` e `POST /legal-entities/{id}/logo` → upload.
    - `GET /people/{id}` retorna detalhes.
    - `GET /people/search` retorna lista resumida.

- **Validações:**
  - Value Objects (`Cpf`, `Cnpj`, `EmailAddress`, etc.) garantem integridade.
  - ModelState validado em Controllers (`ValidationProblem` em caso de falha).

- **Auditoria:**
  - `CreatedAt`, `UpdatedAt` e `User` podem ser adicionados nas entidades.
  - Futuro: Tabela `AuditLogs` para registrar operações críticas.

---

## 4. Testes
- **UnitTests:**  
  - Testar Value Objects e Entidades (ex.: `UpdatePhoto`, `UpdateLogo`, validação de CPF/CNPJ).
- **IntegrationTests:**  
  - Usar `WebApplicationFactory<Program>` com SQLite e storage temporário.
  - Testes end-to-end de fluxo: Create → Upload → Get.

---

## 5. Observabilidade
- **Logging:**  
  - Configuração em `appsettings.json` (nível Default = Information, Microsoft.AspNetCore = Warning).
  - Controllers usam `ILogger` para sucesso, falha de validação e erro inesperado.

- **Auditoria e Logs** devem ser armazenados em solução centralizada em ambientes reais (ex.: ELK, Loki, Seq).

---

## 6. Diretrizes de Deploy
- **Docker:**  
  - Publicar imagem com multi-stage build (`dotnet publish` → runtime).
- **Volumes:**  
  - Montar `_storage` em volume persistente.
- **Segredos:**  
  - Substituir `Auth:Password` e `Jwt:Key` via variáveis de ambiente ou secret manager.

---

# ✅ Próximos Passos
1. Validar diretrizes em review interno.  
2. Commitar este documento em `docs/implementation-guidelines.md`.  
3. Manter versionamento junto com o código (pull requests que alterem arquitetura devem atualizar as diretrizes).  
