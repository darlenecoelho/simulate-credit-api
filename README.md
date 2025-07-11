# SimulateCredit

SimulateCredit é um sistema de simulação e proposta de empréstimos desenvolvido em .NET 8, seguindo os princípios de Clean Architecture (Hexagonal).

## 🛠️ Setup

### Pré-requisitos

- .NET 8 SDK
- Docker & Docker Compose (opcional, para ambiente containerizado)
- RabbitMQ e MongoDB (podem ser provisionados via Docker Compose)

### Desenvolvimento Local

1. Clone o repositório:
   ```bash
   git clone <url-do-repositório>
   cd Creditas
   ```
2. Execute a API diretamente:
   ```bash
   cd src/SimulateCredit.API
   dotnet restore
   dotnet build
   dotnet run
   ```
3. Acesse a API em:
   - HTTP:  `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

## 🐳 Docker & Docker Compose

### Iniciar todos os serviços

No diretório raiz do projeto, execute:

```bash
docker-compose up -d
```

**Contêineres e portas expostas**:

- **simulatecredit-api**: expõe a porta 80 no host `http://localhost:5000` e 443 em `https://localhost:5001`
- **simulatecredit-mongo**: 27017 → 27017 (`mongodb://localhost:27017`)
- **simulatecredit-rabbitmq**: 5672 → 5672 (AMQP) e 15672 → 15672 (painel web)

### Iniciar serviços isolados

- **Apenas RabbitMQ**:

  ```bash
  docker-compose up -d rabbitmq
  ```

  - AMQP: `amqp://localhost:5672`
  - Painel: `http://localhost:15672` (usuário: `guest`, senha: `guest`)

- **Apenas MongoDB**:

  ```bash
  docker-compose up -d mongodb
  ```

  - Conexão: `mongodb://localhost:27017`

- **RabbitMQ + MongoDB + aplicação local**:

  1. Suba RabbitMQ e MongoDB:
     ```bash
     docker-compose up -d rabbitmq mongodb
     ```
  2. Inicie a API localmente (na pasta `SimulateCredit.API`):
     ```bash
     dotnet run
     ```
  3. Acesse em `http://localhost:5000` / `https://localhost:5001`

## 📁 Estrutura do Projeto

```
src/
├── SimulateCredit.API         # Projeto ASP.NET Core Web API (controllers, Program.cs, Swagger)
├── SimulateCredit.Application # Camada de Aplicação (Use Cases, DTOs, interfaces de portas)
├── SimulateCredit.Domain      # Camada de Domínio (entidades, value objects, enums)
└── SimulateCredit.Infrastructure # Adaptadores (MongoDB, RabbitMQ, envio de e-mail, conversão de moeda, logging)
```

Além disso, há testes em `tests/` seguindo xUnit, Moq e FluentAssertions.

## 📐 Decisões de Arquitetura

- **Clean Architecture (Hexagonal)**: forte separação de responsabilidades via camadas e Ports & Adapters.
- **Incoming Ports**: `ISimulateCreditUseCase` definido na camada Application.
- **Outgoing Ports**: implementados em Infrastructure, como `MongoSimulationRepository` (MongoDB), `RabbitMqPublisherAdapter` (RabbitMQ), `NotificationService` (e-mail) e `FakeCurrencyConverterService`.
- **Mensageria Assíncrona**: desacopla envio de notificações por e-mail e logs via RabbitMQ.
- **Persistência NoSQL**: MongoDB armazena simulações de forma flexível.
- **Value Objects**: mantêm invariantes de domínio (ex: `Currency`, `InterestRateType`).
- **Swagger/OpenAPI**: documentação automática dos endpoints.
- **Docker**: garante ambiente consistente.

## 🚀 Endpoints da API

### Simular Empréstimo Único

```
POST /SimulateCredit/simulate
Content-Type: application/json
```

**Exemplo de Requisição**:

```json
{
  "loanAmount": { "amount": 10000.00, "currency": "BRL" },
  "customer": { "birthDate": "1990-01-01T00:00:00", "email": "cliente@exemplo.com" },
  "months": 12,
  "rateType": "Age",
  "sourceCurrency": "BRL",
  "targetCurrency": "BRL"
}
```

**Resposta (200 OK)**:

```json
{
  "totalAmount": 10500.00,
  "monthlyInstallment": 875.00,
  "totalInterest": 500.00
}
```

### Simular Empréstimos em Lote

```
POST /SimulateCredit/simulate/bulk
Content-Type: application/json
```

**Exemplo de Requisição (5 simulações, todas em BRL)**:

```json
[
  {
    "loanAmount": { "amount": 10000.00, "currency": "BRL" },
    "customer": { "birthDate": "1990-01-01T00:00:00", "email": "cliente1@exemplo.com" },
    "months": 12,
    "rateType": "Age",
    "sourceCurrency": "BRL",
    "targetCurrency": "BRL"
  },
  {
    "loanAmount": { "amount": 15000.00, "currency": "BRL" },
    "customer": { "birthDate": "1985-05-20T00:00:00", "email": "cliente2@exemplo.com" },
    "months": 18,
    "rateType": "Age",
    "sourceCurrency": "BRL",
    "targetCurrency": "BRL"
  },
  {
    "loanAmount": { "amount": 20000.00, "currency": "BRL" },
    "customer": { "birthDate": "1970-10-10T00:00:00", "email": "cliente3@exemplo.com" },
    "months": 24,
    "rateType": "Age",
    "sourceCurrency": "BRL",
    "targetCurrency": "BRL"
  },
  {
    "loanAmount": { "amount": 5000.00, "currency": "BRL" },
    "customer": { "birthDate": "2000-08-15T00:00:00", "email": "cliente4@exemplo.com" },
    "months": 6,
    "rateType": "Age",
    "sourceCurrency": "BRL",
    "targetCurrency": "BRL"
  },
  {
    "loanAmount": { "amount": 25000.00, "currency": "BRL" },
    "customer": { "birthDate": "1965-02-05T00:00:00", "email": "cliente5@exemplo.com" },
    "months": 36,
    "rateType": "Age",
    "sourceCurrency": "BRL",
    "targetCurrency": "BRL"
  }
]
```

**Resposta (200 OK)**:

```json
[
  { /* simulação 1 */ },
  { /* simulação 2 */ },
  { /* simulação 3 */ },
  { /* simulação 4 */ },
  { /* simulação 5 */ }
]
```

---

