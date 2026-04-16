# 🚀 SETUP RÁPIDO - Sistema de Reservas

## ⏱️ Tempo Estimado: 5 minutos

Siga os passos abaixo para colocar o sistema de reservas em funcionamento.

---

## 📋 Pré-requisitos

- ✓ Visual Studio ou VS Code com .NET 8.0+
- ✓ SQL Server (LocalDB ou instalado)
- ✓ Projeto RestauranteApp compilando sem erros

---

## 🔧 Passo 1: Atualizar o Banco de Dados

### Option A: Via Terminal (Recomendado)

```bash
cd RestauranteApp
dotnet ef database update
```

### Option B: Via Package Manager Console (Visual Studio)

```powershell
Update-Database
```

**O que acontece:** A migration `20260403_ReservasUpdate` é executada, atualizando a tabela `Reservas` com os novos campos:
- `HoraReserva` (TimeOnly)
- `DataCriacao` (DateTime)

---

## 📊 Passo 2: Criar as Mesas

Execute o script SQL fornecido no seu banco de dados:

### Option A: SQL Server Management Studio

1. Abra **SQL Server Management Studio**
2. Conecte ao seu banco de dados
3. Abra o arquivo: `RestauranteApp/setup-mesas.sql`
4. Clique em **Execute** (ou F5)

### Option B: Terminal

```bash
sqlcmd -S (localdb)\mssqllocaldb -d RestauranteApp -i setup-mesas.sql
```

### Option C: Visual Studio

1. Abra a janela **SQL Server Object Explorer**
2. Expanda seu banco de dados
3. Clique direito → **New Query**
4. Cole o conteúdo de `setup-mesas.sql`
5. Execute

**Resultado esperado:** 10 mesas criadas (mesas 1-10 com capacidades variadas)

---

## ▶️ Passo 3: Iniciar o Backend

### Option A: Visual Studio

1. Abra `RestauranteApp.sln`
2. Clique em **Start** (F5 ou Ctrl+F5)
3. Espere o navegador abrir (normalmente https://localhost:7189)

### Option B: Terminal

```bash
cd RestauranteApp
dotnet run
```

**Saiba que funcionou:**
- Console mostra: `Application started. Press Ctrl+C to shut down.`
- Os logs mostram a porta HTTPS (ex: `https://localhost:7189`)

---

## 🌐 Passo 4: Acessar a Página de Reservas

1. Navegue via browser: `https://localhost:7189` (ou porta específica)
2. Faça **LOGIN** com suas credenciais
3. Clique em **"Reservar"** ou vá para `/reservas.html`

---

## ✅ Passo 5: Testar

Crie uma reserva:

1. Selecione uma **data** (mínimo 1 dia no futuro)
2. Selecione um **horário** (19h, 20h ou 21h)
3. Ajuste **número de pessoas** (1-10)
4. Selecione uma **mesa disponível**
5. Clique **"Confirmar Reserva"**

**Resultado esperado:**
- ✓ Código de confirmação gerado (ex: `RES-20260403-45789`)
- ✓ Reserva aparece em "Minhas Reservas"
- ✓ Sem erros no console

---

## 🔍 Verificação de Saúde

### Backend está rodando?
```bash
curl https://localhost:7189/api/reservasapi
# Deve retornar 401 Unauthorized ou dados de reservas
```

### Banco de dados foi atualizado?
```sql
SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Reservas';
-- Deve incluir: HoraReserva, DataCriacao, DataReserva (como int - DateOnly)
```

### Mesas foram criadas?
```sql
SELECT COUNT(*) FROM Mesas;
-- Deve retornar: 10
```

---

## 🐛 Troubleshooting Rápido

| Problema | Solução |
|----------|---------|
| ⚠️ Migration falhou | Verifique string de conexão em `appsettings.json` |
| ⚠️ Porta já em uso | Altere a porta em `launchSettings.json` |
| ⚠️ Login obrigatório | Crie um nev usuário ou faça login |
| ⚠️ Nenhuma mesa aparece | Verifique se script SQL foi executado (`SELECT * FROM Mesas;`) |
| ⚠️ CORS error | Verifique se está usando https e porta correta |

---

## 📄 Arquivos Importantes

```
frontend-restaurante/
├── reservas.html                          <- Página de reservas
├── js/
│   ├── reservas.js                        <- Lógica do cliente
│   └── api.js                             <- Configuração da API
├── SISTEMA_RESERVAS.md                    <- Documentação completa
└── RestauranteApp/
    ├── setup-mesas.sql                    <- Script de mesas
    ├── Models/Reserva.cs                  <- Model atualizado
    ├── Services/ReservaService.cs         <- Lógica de negócio
    ├── Controllers/
    │   └── ReservasApiController.cs       <- API RESTFUL
    └── Migrations/
        └── 20260403_ReservasUpdate.cs     <- Alteração do BD
```

---

## 📞 Suporte

Se algo não funcionar:

1. **Verifique os logs** do console do backend
2. **Abra DevTools** no navegador (F12) → guia **Network** e **Console**
3. **Consulte `SISTEMA_RESERVAS.md`** para documentação detalhada
4. **Verifique credenciais** de login

---

## ✨ Pronto!

Você agora tem um **sistema de reservas completo e funcional** com:

✓ Validação de data (mínimo 1 dia no futuro)
✓ Horário limitado (19h-22h para jantar)
✓ Detecção de conflitos de mesa
✓ Código único de confirmação
✓ Interface prática e intuitiva
✓ API REST segura e documentada

**Divirta-se usando! 🍽️**

---

**Konoha's © 2026 - Sabores que despertam o seu chakra**
