using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NMS_Proj.Models
{
    public class Planeta
    {
        [Key]
        public string Nome { get; set; } = string.Empty;  // Chave primária

        public string Clima { get; set; } = string.Empty;
        public string ClimaQualidade { get; set; } = string.Empty;
        public string Fauna { get; set; } = string.Empty;
        public string FaunaQualidade { get; set; } = string.Empty;
        public string Flora { get; set; } = string.Empty;
        public string FloraQualidade { get; set; } = string.Empty;
        public string Sentinelas { get; set; } = string.Empty;
        public string SentinelasQualidade { get; set; } = string.Empty;

        public int SistemaEstelarId { get; set; }  //FK para SistemaEstelar

        // FK para Explorador
        public int ExploradorId { get; set; }

        //string separada por vírgula
        public string Recursos { get; set; } = string.Empty;


        [ForeignKey("SistemaEstelarId")]
        public SistemaEstelar SistemaEstelar { get; set; } = null!;

        // Nova propriedade de navegação: Liga diretamente ao Explorador
        [ForeignKey("ExploradorId")]
        public Explorador Explorador { get; set; } = null!;
    }
}
