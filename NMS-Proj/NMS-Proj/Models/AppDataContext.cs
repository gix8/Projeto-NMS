using System;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

// AppDataContext é a classe que representa o DB na aplicação
// Criar a herança da classe DbContext
// Criar os atributos que vão representar as tabelas do DB

public class AppDataContext : DbContext
{
    public DbSet<Explorador> Exploradores { get; set; }
    public DbSet<Planeta> Planetas { get; set; }
    public DbSet<SistemaEstelar> SistemasEstelares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ProjetoNMS.db");
    }
}