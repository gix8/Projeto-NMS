using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NMS_Proj.Models
{
    public class Planeta
    {
        [Key]
        public string Nome { get; set; }

        public string NomeQualidade { get; set; }
        public string Clima { get; set; }
        public string ClimaQualidade { get; set; }
        public string Fauna { get; set; }
        public string FaunaQualidade { get; set; }
        public string Flora { get; set; }
        public string FloraQualidade { get; set; }
        public string Sentinelas { get; set; }
        public string SentinelasQualidade { get; set; }

        public int SistemaEstelarId { get; set; }  //FK para SistemaEstelar

        // FK para Explorador
        public int ExploradorId { get; set; }

        //string separada por vírgula
        public string Recursos { get; set; } = string.Empty;


        [ForeignKey("SistemaEstelarId")]
        public SistemaEstelar SistemaEstelar { get; set; }

        // Nova propriedade de navegação: Liga diretamente ao Explorador
        [ForeignKey("ExploradorId")]
        public Explorador Explorador { get; set; }
    }
}
