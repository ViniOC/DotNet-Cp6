API de Gestão de Eventos Culturais (.NET 8 & ML.NET)

📋 Descrição do Projeto
Esta é uma API RESTful robusta desenvolvida em .NET 8 e ASP.NET Core para o gerenciamento de eventos culturais. O projeto atende a todos os requisitos do CheckPoint 06 (CP06), implementando não apenas um CRUD completo, mas também funcionalidades avançadas como monitoramento, versionamento de API, testes de unidade e um endpoint de previsão de Machine Learning com ML.NET.
A API se conecta a um banco de dados MongoDB Atlas e segue as "Boas Práticas REST" (vistas no PDF) para códigos de status e arquitetura (Serviços e Controladores).

✨ Funcionalidades Principais
CRUD de Eventos: Cadastro, listagem, atualização e exclusão de eventos culturais.
Conexão Segura: Conexão com o cluster MongoDB Atlas na nuvem.
⚕️ Monitoramento (CP06): Endpoint GET /health que verifica a saúde da API e a conexão com o banco de dados.
🆔 Tracing (CP06): Inclusão de um X-Trace-Id em todos os logs e respostas para rastreabilidade.
🚦 Versionamento de API (CP06): A API expõe as versões v1 e v2, com a v2 incluindo campos adicionais (ex: "organizador").
📖 Documentação API (CP06): Swagger/OpenAPI integrado com um seletor de versões funcional.
🤖 Machine Learning (CP06): Endpoint POST /api/previsao que usa ML.NET para prever o número de participantes de um evento, com lógica de negócio para validar e "travar" o resultado (impedindo previsões negativas ou acima da capacidade).
🧪 Testes de Unidade (CP06): Projeto de testes (EventosApi.Tests) usando xUnit e Moq para validar o EventosController.

👥 Equipe de Desenvolvimento
Nome
RM
GitHub
Pedro Henrique dos Santos
RM559064
-
Thiago Thomaz Sales Conceição
RM557992
-
Vinícius de Oliveira Coutinho
RM556182
-

🛠️ Tecnologias Utilizadas
.NET 8
ASP.NET Core 8 (Framework principal)
MongoDB Atlas (Banco de Dados NoSQL)
MongoDB.Driver (Driver oficial do .NET)
ML.NET (Para o modelo de previsão de participantes)
Microsoft.ML.FastTree (Algoritmo de Regressão)
xUnit (Framework de Testes)
Moq (Biblioteca de "Mocking" para testes)
Swagger / Swashbuckle (Documentação da API)
Asp.Versioning.Mvc.ApiExplorer (Gerenciamento de versionamento)

🚀 Como Executar o Projeto (Passo a Passo)
Este projeto requer configuração externa no MongoDB Atlas e configuração local.

Pré-requisitos
.NET 8 SDK
Uma conta gratuita no MongoDB Atlas

Etapa 1: Clonar o Repositório
```bash
git clone https://[URL_DO_SEU_REPOSITORIO_GIT]
cd [NOME_DA_SUA_PASTA_SOLUCAO]
```
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

