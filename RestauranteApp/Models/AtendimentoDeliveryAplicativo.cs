namespace RestauranteApp.Models
{
    public class AtendimentoDeliveryAplicativo : Atendimento
    {
        public string NomeAplicativo { get; set; } = string.Empty;
        public decimal Comissao { get; set; }

        public override decimal CalcularTaxa(decimal subtotalItens, PeriodoEnum periodo)
        {
            var percentual = periodo == PeriodoEnum.Almoco ? 0.04m : 0.06m;
            Comissao = subtotalItens * percentual;
            TaxaEntrega = Comissao;
            return TaxaEntrega;
        }
    }
}