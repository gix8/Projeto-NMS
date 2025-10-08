using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace NMS_Proj.Models
{
    public class SistemaEstelar
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public int qntdPlanetas { get; set; }

        public int ExploradorId { get; set; }

        [ForeignKey("ExploradorId")]
        public Explorador Explorador { get; set; }

        public ICollection<Planeta> Planetas { get; set; } = new List<Planeta>();
    }
}
