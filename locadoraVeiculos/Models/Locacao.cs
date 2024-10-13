using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace locadoraVeiculos.Models
{
    public class Locacao
    {
        [Key]
        public int LocacaoId { get; set; }

        public int VeiculoId { get; set; }
        [ForeignKey("VeiculoId")]

        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]

        public int FuncionarioId { get; set; }
        [ForeignKey("FuncionarioId")]

        public DateTime Data { get; set; }
        public double ValorTotal { get; set; }

    }
}
