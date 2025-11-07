# API de Gestão de Eventos Culturais (.NET 8 & ML.NET)

Descrição
---
API RESTful em .NET 8 (ASP.NET Core) para gerenciamento de eventos culturais. Implementa CRUD completo, versionamento de API (v1/v2), monitoramento (/health), tracing (X-Trace-Id em logs/respostas), documentação via Swagger e um endpoint de previsão de participantes usando ML.NET.

Repositório do projeto
---
https://github.com/ViniOC/DotNet-Cp6.git

Funcionalidades principais
---
- CRUD de eventos (criar, listar, atualizar, excluir)
- Conexão com MongoDB Atlas
- Endpoint de saúde: GET /health
- Tracing: X-Trace-Id em logs e respostas
- Versionamento: /api/v1/... e /api/v2/...
- Documentação: Swagger/OpenAPI com seletor de versão
- Previsão ML: POST /api/previsao (ML.NET - FastTree) com validações de negócio
- Testes unitários com xUnit e Moq

Equipe
---
Vinícius de Oliveira Coutinho (único participante)

Tecnologias
---
- .NET 8
- ASP.NET Core 8
- MongoDB Atlas + MongoDB.Driver
- ML.NET (Microsoft.ML)
- Microsoft.ML.FastTree
- xUnit, Moq
- Swagger / Swashbuckle
- Asp.Versioning.Mvc.ApiExplorer

Como executar (resumo rápido)
---
1. Pré-requisitos: .NET 8 SDK, conta no MongoDB Atlas
2. Clonar:
```bash
git clone https://github.com/ViniOC/DotNet-Cp6.git
cd DotNet-Cp6/MongoDB
```
3. Atualizar `appsettings.Development.json` com sua connection string (não colocar em `appsettings.json`):
```json
{
	"Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } },
	"EventosDatabase": { "ConnectionString": "SUA_STRING_DE_CONEXAO_AQUI", "DatabaseName": "EventosDb", "CollectionName": "Eventos" }
}
```
4. Subir a API:
```powershell
cd MongoDB
dotnet run
```
5. Abrir Swagger: http://localhost:5183/swagger
6. Executar testes (na raiz da solução):
```powershell
dotnet test
```

Observações importantes
---
- Ative o cluster no MongoDB Atlas se estiver pausado (Resume).  
- Em Security > Network Access, adicione 0.0.0.0/0 temporariamente para testes locais (recomenda-se restringir posteriormente).  
- Não versionar secrets; use `appsettings.Development.json` local para a connection string.

Resumo
---
README atualizado para ser objetivo e coerente. Projeto: https://github.com/ViniOC/DotNet-Cp6.git. Usuário/participante: Vinícius.

