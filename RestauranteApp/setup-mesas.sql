-- =====================================================
-- Script de Setup Inicial - Sistema de Reservas
-- Restaurante Konoha's
-- =====================================================

-- Limpar mesas existentes (OPCIONAL - descomente se necessário)
-- DELETE FROM Mesas;
-- DBCC CHECKIDENT ('Mesas', RESEED, 0);

-- =====================================================
-- INSERIR MESAS
-- =====================================================

INSERT INTO Mesas (Numero, Capacidade) VALUES
-- Mesas para 2 pessoas
(1, 2),
(2, 2),
(7, 2),

-- Mesas para 4 pessoas
(3, 4),
(4, 4),
(8, 4),

-- Mesas para 6 pessoas
(5, 6),
(9, 6),

-- Mesas para 8 pessoas
(6, 8),
(10, 8);

-- Verificar inserção
SELECT * FROM Mesas ORDER BY Numero;

-- =====================================================
-- RESULTADO ESPERADO:
-- =====================================================
-- Id | Numero | Capacidade
-- ---+--------+-----------
-- 1  | 1      | 2
-- 2  | 2      | 2
-- 3  | 3      | 4
-- 4  | 4      | 4
-- 5  | 5      | 6
-- 6  | 6      | 8
-- 7  | 7      | 2
-- 8  | 8      | 4
-- 9  | 9      | 6
-- 10 | 10     | 8

-- =====================================================
-- PRÓXIMOS PASSOS:
-- =====================================================
-- 1. Executar este script no SQL Server Management Studio
-- 2. Ou no Package Manager Console: Update-Database
-- 3. Acessar reservas.html
-- 4. Fazer login
-- 5. Criar uma reserva

-- =====================================================
-- DADOS DE EXEMPLO PARA TESTE (OPCIONAL)
-- =====================================================

-- Se quiser visualizar o banco de dados com dados:
-- SELECT * FROM Clientes;
-- SELECT * FROM Mesas;
-- SELECT * FROM Reservas;
