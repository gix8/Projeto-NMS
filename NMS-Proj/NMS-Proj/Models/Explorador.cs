using System.Collections.Generic;  // Para ICollection e List

namespace NMS_Proj.Models
{
    public class Explorador
    {
        public int Id { get; set; }  // Chave primária

        public int Pontuacao { get; set; } = 0;

        public string Nome { get; set; } = string.Empty;

    }
}
