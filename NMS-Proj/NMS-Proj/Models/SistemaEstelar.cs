using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace NMS_Proj.Models
{
    public class SistemaEstelar
    {
        public int Id { get; set; } // Chave primária

        public string Nome { get; set; } = string.Empty;
        public int qntdPlanetas { get; set; } = 0;

        public int ExploradorId { get; set; } // FK para Explorador

        [ForeignKey("ExploradorId")]
        public Explorador Explorador { get; set; } = null!;

    }
}
