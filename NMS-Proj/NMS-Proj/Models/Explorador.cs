using System.Collections.Generic;  // Para ICollection e List

namespace NMS_Proj.Models
{
    public class Explorador
    {
        public int Id { get; set; }  // Chave primária

        public int Pontuacao { get; set; } = 0;

        public string Nome { get; set; } = string.Empty;

        // Propriedade de navegação: Um explorador pode ter muitos sistemas estelares
        public ICollection<SistemaEstelar> SistemasEstelares { get; set; } = new List<SistemaEstelar>();

        // Nova propriedade de navegação: Um explorador pode explorar muitos planetas diretamente
        public ICollection<Planeta> PlanetasExplorados { get; set; } = new List<Planeta>();
    }
}
