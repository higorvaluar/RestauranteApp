namespace RestauranteApp.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}