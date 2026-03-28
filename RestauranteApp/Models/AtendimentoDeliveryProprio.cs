namespace RestauranteApp.Models
{
    public class AtendimentoDeliveryProprio : Atendimento
    {
        public string EnderecoEntrega { get; set; } = string.Empty;
    }
}