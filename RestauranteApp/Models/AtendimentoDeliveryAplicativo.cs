namespace RestauranteApp.Models
{
    public class AtendimentoDeliveryAplicativo : Atendimento
    {
        public string NomeAplicativo { get; set; } = string.Empty;
        public decimal Comissao { get; set; }
    }
}