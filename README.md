# Paschoalotto People API

[![.NET 9](https://img.shields.io/badge/.NET-9.0-blueviolet)](https://dotnet.microsoft.com/)
[![Build](https://github.com/ivaldobatista/paschoalotto-people-api/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/ivaldobatista/paschoalotto-people-api/actions)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## 📌 Descrição

API para cadastro e consulta de **pessoas físicas e jurídicas**, desenvolvida como **prova de conceito para a Paschoalotto**.  
A solução utiliza **.NET 9**, seguindo princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, com autenticação JWT e suporte a upload de imagens.  

---

## 🚀 Tecnologias

- [.NET 9](https://dotnet.microsoft.com/)  
- ASP.NET Core Web API  
- Entity Framework Core  
- Clean Architecture + DDD  
- JWT Authentication  
- Swagger / OpenAPI  
- Docker  

---

## 📂 Estrutura do Projeto

```

paschoalotto-people-api/
│
├── docs/                     # Documentação (diagramas, diretrizes)
├── src/
│   ├── Paschoalotto.People.Api/           # Controllers, middlewares, DTOs
│   ├── Paschoalotto.People.Application/   # Casos de uso, validações
│   ├── Paschoalotto.People.Domain/        # Entidades, agregados, value objects
│   ├── Paschoalotto.People.Infrastructure/# Repositórios, EF Core, JWT, File Storage
│   └── Paschoalotto.People.CrossCutting/  # Logging, IoC, utilitários
│
├── tests/                   # Testes unitários e de integração
├── .github/workflows/       # CI/CD
├── README.md
└── global.json              # Trava versão do SDK .NET 9

````

---

## ⚙️ Como rodar localmente

Pré-requisitos:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- [Docker](https://www.docker.com/)  

```bash
# Clonar o repositório
git clone https://github.com/ivaldobatista/paschoalotto-people-api.git
cd paschoalotto-people-api

# Restaurar dependências
dotnet restore

# Compilar solução
dotnet build

# Rodar a API
cd src/Paschoalotto.People.Api
dotnet run
````

A API estará disponível em:
👉 `https://localhost:5001/swagger`

---

## 🧪 Testes

```bash
dotnet test
```

---

## 📖 Documentação

* Arquitetura (Clean Architecture + DDD) → `docs/architecture-diagram.png`
* Diretrizes de implementação → `docs/guidelines.md`

---
