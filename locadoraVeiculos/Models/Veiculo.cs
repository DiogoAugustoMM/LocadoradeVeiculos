using System.ComponentModel.DataAnnotations;

namespace locadoraVeiculos.Models
{
    public class Veiculo
    {
        [Key]
        public int ID { get; set; }
        public string Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Ano { get; set; }
        public string? Status { get; set; }
    }
}
