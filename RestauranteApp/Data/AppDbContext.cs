using Microsoft.EntityFrameworkCore;
using RestauranteApp.Models;

namespace RestauranteApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<AtendimentoPresencial> AtendimentosPresenciais { get; set; }
        public DbSet<AtendimentoDeliveryProprio> AtendimentosDeliveryProprio { get; set; }
        public DbSet<AtendimentoDeliveryAplicativo> AtendimentosDeliveryAplicativo { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<SugestaoChefe> SugestoesChefe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PedidoItem>()
                .Property(pi => pi.PrecoUnitario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PedidoItem>()
                .Property(pi => pi.Subtotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Atendimento>()
                .Property(a => a.TaxaEntrega)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AtendimentoDeliveryAplicativo>()
                .Property(a => a.Comissao)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SugestaoChefe>()
                .Property(s => s.PercentualDesconto)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Atendimento)
                .WithOne(a => a.Pedido)
                .HasForeignKey<Atendimento>(a => a.PedidoId);

            modelBuilder.Entity<SugestaoChefe>()
                .HasIndex(s => new { s.Data, s.Periodo })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}