namespace RestauranteApp.Models
{
    public class AtendimentoPresencial : Atendimento
    {
        public override decimal CalcularTaxa(decimal subtotalItens, PeriodoEnum periodo)
        {
            TaxaEntrega = 0m;
            return TaxaEntrega;
        }
    }
}