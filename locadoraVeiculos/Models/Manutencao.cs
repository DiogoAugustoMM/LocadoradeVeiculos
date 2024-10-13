using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace locadoraVeiculos.Models
{
    public class Manutencao
    {
        [Key]
        public int ManutencaoId { get; set; }

        public int VeiculoId { get; set; }
        [ForeignKey("VeiculoId")]

        public DateTime Data { get; set; }
        public string? DescricaoServico { get; set; }
        public double Custo { get; set; }
    }
}
