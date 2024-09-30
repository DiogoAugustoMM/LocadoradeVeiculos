using System.ComponentModel.DataAnnotations;

namespace locadoraVeiculos.Models
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }
        public string CPF { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
