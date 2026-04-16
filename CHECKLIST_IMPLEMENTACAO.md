╔══════════════════════════════════════════════════════════════════════════════╗
║                     ✅ CHECKLIST DE IMPLEMENTAÇÃO                            ║
║                     Sistema de Reservas Konoha's                            ║
╚══════════════════════════════════════════════════════════════════════════════╝

🎯 ANTES DE COMEÇAR
═══════════════════════════════════════════════════════════════════════════════

Certifique-se de ter:
□ Visual Studio ou VS Code com .NET 8.0+
□ SQL Server (LocalDB ou instalado)
□ Projeto RestauranteApp compilando sem erros
□ Backend inicializado anteriormente pelo menos uma vez


📦 PASSO 1: BANCO DE DADOS
═══════════════════════════════════════════════════════════════════════════════

□ Abrir Terminal/PowerShell na pasta RestauranteApp
□ Executar: dotnet ef database update
  └─ Verificar: "✓ Operations completed successfully"
□ Ou (Visual Studio) Package Manager Console: Update-Database


🗃️ PASSO 2: MESAS
═══════════════════════════════════════════════════════════════════════════════

□ Abrir SQL Server Management Studio (ou SQL Server Object Explorer no VS)
□ Abrir arquivo: RestauranteApp/setup-mesas.sql
□ Executar o script (F5 ou Execute)
  └─ Verificar: "10 rows inserted"
□ Validar: SELECT * FROM Mesas;
  └─ Deve mostrar 10 mesas (1-10) com capacidades variadas


♻️ PASSO 3: VERIFICAR PROGRAMA
═══════════════════════════════════════════════════════════════════════════════

□ Abrir Program.cs
□ Verificar se tem: builder.Services.AddScoped<ReservaService>();
  └─ Se não tiver, adicionar na linha apropriada


▶️ PASSO 4: INICIAR BACKEND
═══════════════════════════════════════════════════════════════════════════════

□ Visual Studio: Clicar em ▶️ Start (F5)
  OU
□ Terminal: dotnet run
□ Esperar aplicação iniciar (não fechar terminal/console)
□ Ver mensagem: "Application started. Press Ctrl+C to shut down."
□ Notar a porta HTTPS (ex: https://localhost:7189)


🌐 PASSO 5: ACESSAR A APLICAÇÃO
═══════════════════════════════════════════════════════════════════════════════

□ Abrir navegador
□ Ir para: https://localhost:7189 (ajustar porta se necessário)
□ Página inicial carrega sem erros
□ Botão "Reservar" visível na logo "🍥 Konoha's"


🔐 PASSO 6: FAZER LOGIN
═══════════════════════════════════════════════════════════════════════════════

□ Clicar em "Login"
□ Inserir credenciais (usuário já existente)
□ Clicar "Entrar"
□ Login bem-sucedido
□ Sessão iniciada (ClienteId armazenado)


📋 PASSO 7: ACESSAR PÁGINA DE RESERVAS
═══════════════════════════════════════════════════════════════════════════════

□ Na barra de navegação, clicar em "🍽 Reservar" ou "Reservas"
  OU
□ Ir direto para: https://localhost:7189/reservas.html
□ Página carrega sem erros
□ Vê "Nova Reserva" e "Minhas Reservas"


🧪 PASSO 8: TESTAR CRIAÇÃO DE RESERVA
═══════════════════════════════════════════════════════════════════════════════

□ Selecionar Data
  └─ Clicar em campo de data
  └─ Selecionar data de AMANHÃ OU DEPOIS
  └─ Data é carregada no campo

□ Selecionar Horário
  └─ Aparecem opções: 19h, 20h, 21h
  └─ Clicar em um horário (ex: 20h)
  └─ Horário selecionado fica em destaque (vermelho)

□ Ajustar Número de Pessoas
  └─ Clicar em − ou + para mudar
  └─ Ou clicar no número e digitar (1-10)
  └─ Número atualiza

□ Ver Mesas Disponíveis
  └─ Mesas carregam (não deve tardar)
  └─ Aparecem mesas com números e capacidades
  └─ Apenas mesas com capacidade ≥ pessoas aparecem

□ Selecionar Mesa
  └─ Clicar em uma mesa disponível
  └─ Mesa fica em destaque (vermelho)

□ Confirmar Reserva
  └─ Clicar botão "Confirmar Reserva"
  └─ Botão fica desabilitado enquanto processa
  └─ Código de confirmação aparece em destaque

□ Código Visível
  └─ Código no formato: RES-YYYYMMDD-XXXXX
  └─ Exemplo: RES-20260403-87654
  └─ Está em uma caixa colorida


📌 PASSO 9: TESTAR CÓPIA DE CÓDIGO
═══════════════════════════════════════════════════════════════════════════════

□ Clique no código de confirmação
□ Vê mensagem: "✓ Copiado!"
□ Caixa muda de cor (verde)
□ Código está na área de transferência
□ Pode colar em outro lugar (Ctrl+V)


👀 PASSO 10: VER MINHAS RESERVAS
═══════════════════════════════════════════════════════════════════════════════

□ Scroll para baixo
□ Seção "Minhas Reservas"
□ Reserva criada aparece como card
□ Card mostra:
  └─ Data e dia da semana
  └─ Horário
  └─ Número de pessoas
  └─ Mesa número e capacidade
  └─ Código de confirmação (clicável)
  └─ Botão "❌ Cancelar"


🔄 PASSO 11: TESTAR CANCELAMENTO
═══════════════════════════════════════════════════════════════════════════════

□ No card da reserva, clicar "Cancelar"
□ Confirmação: "Tem certeza?"
□ Clicar OK
□ Reserva desaparece da lista
□ Mensagem: "Reserva cancelada com sucesso!"


📊 PASSO 12: TESTAR VALIDAÇÕES
═══════════════════════════════════════════════════════════════════════════════

Teste cada validação:

□ Data no passado
  └─ Selecionar data anterior a hoje
  └─ Botão calendário não permite selecionar
  └─ ✓ Funciona

□ Horário inválido
  └─ Selecionar data
  └─ Ver apenas 19h, 20h, 21h
  └─ Nenhum outro horário aparece
  └─ ✓ Funciona

□ Quantidade de pessoas > capacidade
  └─ Selecionar 10 pessoas
  └─ Selecionar data/hora
  └─ Apenas mesas com capacidade ≥ 10 aparecem
  └─ Mesas pequenas aparecem desabilitadas
  └─ ✓ Funciona

□ Reserva duplicada (mesma mesa + data + hora)
  └─ Criar primeira reserva com mesa 5, 2026-04-10, 20h, 2 pessoas
  └─ Tentar criar segunda reserva mesa 5, 2026-04-10, 20h, 2 pessoas
  └─ Mesa 5 NÃO aparece como disponível
  └─ Sistema sugere selecionar outra mesa ou horário
  └─ ✓ Funciona


🔍 PASSO 13: VERIFICAR BANCO DE DADOS
═══════════════════════════════════════════════════════════════════════════════

□ Abrir SQL Server Management Studio
□ Executar:
   SELECT * FROM Reservas WHERE ClienteId = [ID DO USUÁRIO];
□ Deve retornar as reservas criadas
□ Verificar colunas:
   └─ DataReserva (formato data)
   └─ HoraReserva (formato hora)
   └─ QuantidadePessoas (número)
   └─ CodigoConfirmacao (RES-YYYYMMDD-XXXXX)
   └─ DataCriacao (data/hora de criação)


🐛 PASSO 14: VERIFICAR LOGS
═══════════════════════════════════════════════════════════════════════════════

□ Abrir DevTools do navegador (F12)
□ Guia Console:
   └─ Não deve haver erros em vermelho
   └─ Pode haver warnings (alguns são normais)
□ Guia Network:
   └─ Requisições para /api/reservasapi/ devem retornar 200, 201
   └─ Status 401 = não autenticado (esperado se fizer logout)
□ Console da aplicação (Terminal):
   └─ Não deve haver exceções
   └─ Deve mostrar requisições HTTP


✨ PASSO 15: TESTAR RESPONSIVIDADE
═══════════════════════════════════════════════════════════════════════════════

□ Abrir reservas.html no navegador
□ Apertar F12 (DevTools)
□ Clicar no ícone responsivo (ou Ctrl+Shift+M)
□ Testar em:
   □ Mobile (375px)
     └─ Interface adapta bem
     └─ Botões clicáveis
     └─ Texto legível
   □ Tablet (768px)
     └─ Layout ajusta
     └─ Componentes bem organizados
   □ Desktop (1920px)
     └─ Layout otimizado
     └─ Espaçamento apropriado


🎯 PASSO 16: TESTE AVANÇADO (OPCIONAL)
═══════════════════════════════════════════════════════════════════════════════

□ Abrir DevTools (F12)
□ Guia Console: colar código de teste:

   // Testar API diretamente
   fetch('https://localhost:7189/api/reservasapi/horarios')
     .then(r => r.json())
     .then(d => console.log(d))
   
   └─ Deve retornar: { horaInicio: 19, horaFim: 22, ... }

□ Testar rejeição de reserva fora de horário:
   
   fetch('https://localhost:7189/api/reservasapi', {
     method: 'POST',
     headers: {'Content-Type': 'application/json'},
     credentials: 'include',
     body: JSON.stringify({
       data: '2026-04-10',
       hora: '16:00',
       quantidadePessoas: 2,
       mesaId: 1
     })
   }).then(r => r.json()).then(d => console.log(d))
   
   └─ Deve retornar erro: "As reservas são permitidas..."


═══════════════════════════════════════════════════════════════════════════════
✅ SUCESSO!
═══════════════════════════════════════════════════════════════════════════════

Se passou por todos os passos acima, seu sistema está:

✅ Completamente funcional
✅ Totalmente validado
✅ Pronto para produção
✅ Bem documentado


═══════════════════════════════════════════════════════════════════════════════
🚨 SE ALGO NÃO FUNCIONOU
═══════════════════════════════════════════════════════════════════════════════

Checklist de troubleshooting:

□ Erro ao executar migration?
  └─ Verificar string de conexão em appsettings.json
  └─ Verificar se SQL Server está rodando
  └─ Tentar: dotnet ef database drop && dotnet ef database update

□ Nenhuma mesa aparece?
  └─ Verificar: SELECT COUNT(*) FROM Mesas;
  └─ Se retornar 0, executar setup-mesas.sql novamente

□ Erro 401 Unauthorized?
  └─ Fazer logout e login novamente
  └─ Fechar e reabrir navegador
  └─ Limpar cookies/cache

□ CORS Error ou Connection refused?
  └─ Verificar se backend está rodando (terminal/console)
  └─ Verificar porta correta em launchSettings.json
  └─ Verificar URL em js/reservas.js

□ Mesas não carregam quando seleciono data/hora?
  └─ Abrir DevTools (F12)
  └─ Guia Network → filtrar "mesas-disponiveis"
  └─ Verificar response
  └─ Se 200 mas vazio: data/hora sem mesas disponíveis (normal)

□ Código de confirmação não aparece?
  └─ Abrir DevTools, verificar resposta do POST
  └─ Se contiver "codigoConfirmacao": valor está lá
  └─ Se vazio: ReservaService não gerou (verificar logs)


═══════════════════════════════════════════════════════════════════════════════
📞 PRÓXIMOS PASSOS
═══════════════════════════════════════════════════════════════════════════════

Após validar tudo:

□ Revisar SISTEMA_RESERVAS.md para entender arquitetura completa
□ Revisar código-fonte para customizações
□ Considerar melhorias sugeridas em "Próximas Melhorias"
□ Preparar para produção (changuestrings de conexão, URLs, etc)
□ Fazer backup do banco de dados
□ Testar com múltiplos usuários
□ Monitorar logs em produção


═══════════════════════════════════════════════════════════════════════════════

🎉 Parabéns! Sistema de reservas implementado com sucesso!

Konoha's © 2026 - Sabores que despertam o seu chakra
