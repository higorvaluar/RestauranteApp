using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ValidarPedidoAsync(Pedido pedido)
        {
            if (pedido.ClienteId <= 0)
            {
                return "O pedido precisa estar vinculado a um cliente.";
            }

            if (pedido.PedidoItens == null || !pedido.PedidoItens.Any())
            {
                return "O pedido precisa ter pelo menos um item.";
            }

            if (pedido.Atendimento == null)
            {
                return "O pedido precisa ter um tipo de atendimento.";
            }

            foreach (var item in pedido.PedidoItens)
            {
                if (item.Quantidade <= 0)
                {
                    return "A quantidade dos itens deve ser maior que zero.";
                }

                var produto = item.Produto ?? await _context.Produtos.FindAsync(item.ProdutoId);

                if (produto == null)
                {
                    return $"Produto inválido no item {item.ProdutoId}.";
                }

                if (produto.Periodo != pedido.Periodo)
                {
                    return "Todos os itens do pedido devem ser do mesmo período do pedido.";
                }
            }

            return null;
        }

        public async Task CalcularPedidoAsync(Pedido pedido)
        {
            var erro = await ValidarPedidoAsync(pedido);

            if (erro != null)
            {
                throw new InvalidOperationException(erro);
            }

            decimal subtotalItens = 0m;

            foreach (var item in pedido.PedidoItens)
            {
                var produto = item.Produto ?? await _context.Produtos.FindAsync(item.ProdutoId);

                if (produto == null)
                {
                    throw new InvalidOperationException($"Produto inválido no item {item.ProdutoId}.");
                }

                item.Produto = produto;
                item.PrecoUnitario = produto.Preco;

                decimal precoComDesconto = produto.Preco;

                bool ehSugestao = await _context.SugestoesChefe.AnyAsync(s =>
                    s.Data.Date == pedido.DataPedido.Date &&
                    s.Periodo == pedido.Periodo &&
                    s.ProdutoId == item.ProdutoId);

                if (ehSugestao)
                {
                    precoComDesconto = produto.Preco * 0.80m;
                }

                item.Subtotal = precoComDesconto * item.Quantidade;
                subtotalItens += item.Subtotal;
            }

            decimal taxa = pedido.Atendimento!.CalcularTaxa(subtotalItens, pedido.Periodo);
            pedido.Total = subtotalItens + taxa;
        }
    }
}