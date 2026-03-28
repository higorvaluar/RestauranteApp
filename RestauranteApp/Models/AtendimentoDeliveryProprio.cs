namespace RestauranteApp.Models
{
    public class AtendimentoDeliveryProprio : Atendimento
    {
        public string EnderecoEntrega { get; set; } = string.Empty;

        public override decimal CalcularTaxa(decimal subtotalItens, PeriodoEnum periodo)
        {
            return TaxaEntrega;
        }
    }
}