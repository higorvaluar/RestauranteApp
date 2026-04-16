# 📋 Sistema de Reservas - Documentação

## 🎯 Visão Geral

Sistema completo e funcional de reservas de mesas para jantar no restaurante Konoha's.

### ✨ Características:
- ✅ Apenas para usuários **autenticados**
- ✅ Reservas para período de **jantar (19h - 22h)**
- ✅ Validação automática de **conflitos de horário e mesa**
- ✅ Código único de confirmação para cada reserva
- ✅ Interface intuitiva e responsiva
- ✅ Gerenciamento completo (criar, visualizar, cancelar)

---

## 🚀 Instalação e Setup

### 1️⃣ Executar as Migrations

As migrations atualizaram a tabela `Reservas` com os novos campos.

```bash
cd RestauranteApp
dotnet ef database update
```

Ou via Package Manager Console (Visual Studio):
```powershell
Update-Database
```

### 2️⃣ Criar Mesas de Exemplo

Você precisa ter mesas no banco de dados. Execute este script SQL:

```sql
-- Limpar mesas existentes (opcional)
-- DELETE FROM Mesas;

-- Inserir mesas de exemplo
INSERT INTO Mesas (Numero, Capacidade) VALUES
(1, 2),
(2, 2),
(3, 4),
(4, 4),
(5, 6),
(6, 8),
(7, 2),
(8, 4),
(9, 6),
(10, 8);
```

### 3️⃣ Configurar Conexão

Verifique se a `API_BASE` em `js/reservas.js` está correta:

```javascript
const API_BASE = "https://localhost:7189/api";
```

Ajuste a porta se necessário (verifique em `RestauranteApp/Properties/launchSettings.json`).

---

## 📱 Como Usar

### Página de Reservas: `reservas.html`

#### 1. Selecionar Data
- Clique no campo "Data da Reserva"
- Selecione uma data (mínimo 1 dia no futuro)

#### 2. Selecionar Número de Pessoas
- Use os botões **−** e **+** para ajustar
- Ou clique e digite diretamente (1-10 pessoas)

#### 3. Selecionar Horário
- Clique em um dos horários disponíveis (19h, 20h, 21h)
- Apenas horários dentro do período de jantar aparecem

#### 4. Selecionar Mesa
- As mesas disponíveis aparecem após selecionar data e hora
- Apenas mesas com capacidade suficiente aparecem ativadas
- Clique para selecionar

#### 5. Confirmar Reserva
- Clique no botão "Confirmar Reserva"
- Sistema gera código de confirmação único
- Código aparece em destaque para cópia fácil

#### 6. Ver Minhas Reservas
- Scroll para baixo
- Todas as suas reservas aparecem com:
  - Data e hora agendadas
  - Número de mesa
  - Código de confirmação (clicável para copiar)
  - Botão para cancelar

---

## 🔐 Validações

O sistema realiza validações **automáticas**:

### ✓ Data
- Deve ser no mínimo **1 dia no futuro**
- Não permite datas passadas

### ✓ Horário
- Apenas entre **19h e 22h**
- Horários permitidos aparecem dinamicamente

### ✓ Quantidade de Pessoas
- Mínimo: **1 pessoa**
- Máximo: **10 pessoas**
- Mesa selecionada deve ter capacidade suficiente

### ✓ Conflito de Duplicidade
- Sistema valida se já existe reserva para:
  - **Mesma mesa**
  - **Mesma data**
  - **Mesmo horário**
- Não permite reservas duplicadas
- Aconselha selecionar outro horário ou mesa

### ✓ Disponibilidade de Mesa
- Mesas ocupadas não aparecem como disponíveis
- Apenas mesas com capacidade compatível aparecem

---

## 🔑 Código de Confirmação

Cada reserva recebe um código único:

**Formato**: `RES-YYYYMMDD-XXXXX`

Exemplo: `RES-20260403-45789`

### Como usar:
1. Copie o código (clique sobre ele)
2. Apresente ao fazer check-in no restaurante
3. Mais prático que lembrar ID da reserva

---

## 📊 Arquitetura do Sistema

### Frontend (`/frontend-restaurante`)
- **reservas.html** - Interface principal
- **js/reservas.js** - Lógica do cliente e chamadas da API
- **js/api.js** - Configuração da URL da API

### Backend (`/RestauranteApp`)

#### Models
- **Models/Reserva.cs** - Modelo da entidade com validações
- **Models/Mesa.cs** - Modelo das mesas
- **Models/Cliente.cs** - Modelo do cliente

#### Controllers
- **ReservasApiController.cs** - API completa de reservas
  - `GET /api/reservasapi` - Listar minhas reservas
  - `GET /api/reservasapi/{id}` - Detalhes de uma reserva
  - `POST /api/reservasapi` - Criar nova reserva
  - `PUT /api/reservasapi/{id}` - Atualizar reserva
  - `DELETE /api/reservasapi/{id}` - Cancelar reserva
  - `GET /api/reservasapi/mesas-disponiveis` - Mesas disponíveis
  - `GET /api/reservasapi/horarios` - Horários de funcionamento

#### Services
- **Services/ReservaService.cs** - Lógica de negócio
  - Validação de data (mínimo 1 dia no futuro)
  - Validação de hora (19h-22h)
  - Verificação de capacidade da mesa
  - Detecção de conflitos
  - Geração de código de confirmação
  - Obtenção de mesas disponíveis

#### Data
- **Data/AppDbContext.cs** - Contexto do banco de dados
- **Migrations/20260403_ReservasUpdate.cs** - Atualiza tabela Reservas

---

## 🛠️ Endpoints da API

### Autenticação
Todos os endpoints required autenticação (sessão HTTP).

### GET `/api/reservasapi`
Lista todas as reservas do usuário autenticado.

**Response:**
```json
[
  {
    "id": 1,
    "dataReserva": "2026-04-10",
    "horaReserva": "20:00:00",
    "quantidadePessoas": 4,
    "codigoConfirmacao": "RES-20260403-45789",
    "dataCriacao": "2026-04-03T10:30:00",
    "mesa": {
      "numero": 5,
      "capacidade": 6
    }
  }
]
```

### POST `/api/reservasapi`
Cria uma nova reserva.

**Request:**
```json
{
  "data": "2026-04-10",
  "hora": "20:00",
  "quantidadePessoas": 4,
  "mesaId": 5
}
```

**Response:**
```json
{
  "id": 1,
  "dataReserva": "2026-04-10",
  "horaReserva": "20:00:00",
  "quantidadePessoas": 4,
  "codigoConfirmacao": "RES-20260403-45789",
  "mensagem": "Reserva criada com sucesso!"
}
```

### GET `/api/reservasapi/mesas-disponiveis?data=2026-04-10&hora=20:00`
Retorna mesas disponíveis para data e hora especificadas.

**Response:**
```json
[
  {
    "id": 1,
    "numero": 1,
    "capacidade": 2
  },
  {
    "id": 3,
    "numero": 3,
    "capacidade": 4
  }
]
```

### GET `/api/reservasapi/horarios`
Retorna horários de funcionamento para reservas.

**Response:**
```json
{
  "horaInicio": 19,
  "horaFim": 22,
  "mensagem": "Reservas disponíveis das 19h às 22h"
}
```

### DELETE `/api/reservasapi/{id}`
Cancela uma reserva específica.

**Response:**
```json
{
  "mensagem": "Reserva cancelada com sucesso!"
}
```

---

## 🔄 Fluxo de Reserva Completo

```
USUÁRIO AUTENTICADO
    ↓
ACESSA reservas.html
    ↓
SELECIONA data + horário + quantidade de pessoas
    ↓
    ├─→ JS BUSCA mesas disponíveis (GET /mesas-disponiveis)
    └─→ EXIBE mesas com capacidade suficiente
    ↓
CLICA em uma mesa
    ↓
CONFIRMA criação (POST /reservasapi)
    ↓
    ├─→ BACKEND VALIDA:
    │   ├─ Data está no futuro? ✓
    │   ├─ Horário está em 19h-22h? ✓
    │   ├─ Capacidade suficiente? ✓
    │   ├─ Não há conflito? ✓
    │   └─ Cliente existe? ✓
    ↓
BACKEND GERA código de confirmação
    ↓
RESERVA CRIADA COM SUCESSO ✓
    ↓
CÓDIGO EXIBIDO ao usuário
    ↓
USUÁRIO VIRA MINHAS RESERVAS (GET /reservasapi)
    ↓
RESERVA APARECE NA LISTA
```

---

## 📋 Casos de Uso

### ✅ Caso 1: Reserva bem-sucedida
1. Usuário seleciona 2026-04-10, 20:00, 4 pessoas
2. Sistema busca mesas disponíveis
3. Mesa 5 (capacidade 6) aparece disponível
4. Usuário clica em mesa 5
5. Clica "Confirmar Reserva"
6. Código de confirmação gerado: `RES-20260403-45789`
7. ✓ Reserva criada!

### ❌ Caso 2: Horário inválido
1. Usuário tenta selecionar 18:00 (antes de 19h)
2. Sistema NÃO exibe este horário
3. Apenas 19h, 20h, 21h aparecem como opções

### ❌ Caso 3: Conflito de Mesa
1. Mesa 5 já tem reserva em 2026-04-10 às 20:00
2. Usuário seleciona mesma data/hora
3. Mesa 5 NÃO aparece como disponível
4. Sistema sugere selecionar outro horário ou mesa

### ❌ Caso 4: Capacidade Insuficiente
1. Usuário seleciona 8 pessoas
2. Mesa 1 (capacidade 2) não aparece habilitada
3. Apenas mesas com capacidade ≥ 8 aparecem (mesas 5, 6, etc)

---

## 🧪 Testando o Sistema

### 1. Setup Inicial
- Executar migrations: `dotnet ef database update`
- Criar mesas via SQL script fornecido
- Iniciar backend: `dotnet run`

### 2. Teste Funcional
- Acessar `reservas.html`
- Fazer login (necessário)
- Criar reserva para amanhã
- Verificar se código de confirmação aparece
- Visualizar em "Minhas Reservas"

### 3. Teste de Validação
- Tentar reservar no passado → Erro
- Tentar reservar às 18h → Não aparece como opção
- Tentar reservar 15 pessoas → Erro (máx. 10)
- Tentar reservar mesa já ocupada → Não aparece

---

## 🐛 Troubleshooting

### Problema: "Erro ao carregar dados. Tente recarregar..."
- ✓ Verifique se está autenticado (login)
- ✓ Verifique se backend está rodando
- ✓ Verifique URL em `js/reservas.js`

### Problema: Nenhuma mesa aparece
- ✓ Verifique se mesas foram criadas no banco (SQL script)
- ✓ Verifique se data/hora foram selecionados
- ✓ Verifique quantidade de pessoas vs capacidade

### Problema: CORS Error
- ✓ Verifique configuração do CORS no backend
- ✓ Verifique se URL está com https
- ✓ Verifique porta correta (7189 por padrão)

### Problema: 401 Unauthorized
- ✓ Faça login novamente
- ✓ Verifique se sessão expirou
- ✓ Verifique cookies (credentials: "include")

---

## 📈 Próximas Melhorias Sugeridas

- [ ] Integração com sistema de pagamento
- [ ] Envio de email com confirmação
- [ ] Notificação por WhatsApp/SMS
- [ ] Calendario interativo
- [ ] Filtros avançados (tipo de comida, preferência de saudade)
- [ ] Avaliar reservas realizadas
- [ ] Recomendações de pratos baseadas em histórico

---

## 📚 Referências

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server
- **Frontend**: Bootstrap 5.3 + Vanilla JS
- **API REST**: RESTful design

---

**Sistema desenvolvido com ❤️ para Konoha's © 2026**
