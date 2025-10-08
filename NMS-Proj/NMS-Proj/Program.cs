using Microsoft.AspNetCore.Mvc;
using NMS_Proj.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Teste de geração de dados in-memory (simulando um banco de dados)
var exploradores = new List<Explorador>
{
    new Explorador { Id = 1, Nome = "Ava" },
    new Explorador { Id = 2, Nome = "Kai" }
};

var sistemas = new List<SistemaEstelar>
{
    new SistemaEstelar { Id = 1, Nome = "Alpha Centauri", qntdPlanetas = 2, ExploradorId = 1 },
    new SistemaEstelar { Id = 2, Nome = "TRAPPIST-1", qntdPlanetas = 3, ExploradorId = 2 }
};

var planetas = new List<Planeta>
{
    new Planeta { Nome = "Proxima b", Clima = "Temperado", Fauna = "Desconhecida", Flora = "Nenhuma", Sentinelas = "Baixa", SistemaEstelarId = 1, ExploradorId = 1, Recursos = "H2O,Fe" },
    new Planeta { Nome = "TRAP-1a", Clima = "Árido", Fauna = "Microscópica", Flora = "Líquens", Sentinelas = "Média", SistemaEstelarId = 2, ExploradorId = 2, Recursos = "Si,Al" }
};

// Teste de geração de dados in-memory (simulando um banco de dados)
// Para demonstrar relações entre entidades sem um banco real com a funcionalidade de navegação
void WireRelations()
{
    foreach (var s in sistemas)
    {
        s.Explorador = exploradores.FirstOrDefault(e => e.Id == s.ExploradorId);
        s.Planetas = planetas.Where(p => p.SistemaEstelarId == s.Id).ToList();
    }

    foreach (var p in planetas)
    {
        p.SistemaEstelar = sistemas.FirstOrDefault(s => s.Id == p.SistemaEstelarId);
        p.Explorador = exploradores.FirstOrDefault(e => e.Id == p.ExploradorId);
    }

    foreach (var e in exploradores)
    {
        e.SistemasEstelares = sistemas.Where(s => s.ExploradorId == e.Id).ToList();
        e.PlanetasExplorados = planetas.Where(p => p.ExploradorId == e.Id).ToList();
    }
}

WireRelations(); // Inicializa as relações entre as entidades planetas, sistemas e exploradores


// Exploradores final de endpoints da pesquisa
app.MapGet("/exploradores", () => Results.Ok(exploradores));

// Identifica explorador pelo Id
app.MapGet("/exploradores/{id:int}", (int id, AppDataContext ctx) =>
{
    var e = ctx.Exploradores.Find(id);
    return e is null ? Results.NotFound() : Results.Ok(e);
});

// Adiciona explorador com Id randomico
app.MapPost("/exploradores", (Explorador novo, AppDataContext ctx) =>
{
    ctx.Exploradores.Add(novo);
    ctx.SaveChanges(); 

    return Results.Created($"/exploradores/{novo.Id}", novo);
});

// Atualiza explorador pelo Id manualmente
app.MapPut("/exploradores/{id:int}", (int id, Explorador updated, AppDataContext ctx) =>
{
    var e = ctx.Exploradores.Find(id);
    if (e is null) return Results.NotFound();
    e.Nome = updated.Nome;
    ctx.SaveChanges();
    WireRelations();
    return Results.NoContent();
});

// Remove explorador pelo Id e todos os sistemas e planetas associados
app.MapDelete("/exploradores/{id:int}", (int id, AppDataContext ctx) =>
{
    var e = ctx.Exploradores.Find(id);
    if (e is null) return Results.NotFound();
    // Verifica se há sistemas ou planetas associados antes de remover
    // Remove dependent sistemas and planets

    var sistemas = ctx.Sistemas.Where(s => s.ExploradorId == id).ToList();
    ctx.Sistemas.RemoveRange(sistemas);

    var planetas = ctx.Planetas.Where(p => p.ExploradorId == id).ToList();
    ctx.Planetas.RemoveRange(planetas);

    ctx.Exploradores.Remove(e);
    ctx.SaveChanges();
    WireRelations();
    return Results.NoContent();
});

// Verifica se o sistema existe antes de adicionar um planeta
// Sistemas Estelares endpoint
app.MapGet("/sistemas", () => Results.Ok(sistemas));

// Identifica sistema pelo Id
app.MapGet("/sistemas/{id:int}", (int id, AppDataContext ctx) =>
{
    var s = ctx.Sistemas.Find(id);
    return s is null ? Results.NotFound() : Results.Ok(s);
}); 

// Retorna todos os planetas de um sistema estelar específico
app.MapGet("/sistemas/{id:int}/planetas", (int id, AppDataContext ctx) =>
{
    var list = ctx.Planetas.Where(p => p.SistemaEstelarId == id).ToList();
    return Results.Ok(list);
});

// Adiciona sistema estelar com Id randomico
app.MapPost("/sistemas", (SistemaEstelar novo, AppDataContext ctx) =>
{
     // Verifica se o explorador existe
    var explorador = ctx.Exploradores.Find(novo.ExploradorId);
    if (explorador is null)
        return Results.BadRequest("Explorador não encontrado.");

    // Adiciona o sistema ao banco
    ctx.Sistemas.Add(novo);

    // Lógica de pontuação: incrementa pontos do explorador
    explorador.Pontuacao += novo.qntdPlanetas;
    Console.WriteLine($"Pontuação atualizada para explorador {explorador.Nome}: +{novo.qntdPlanetas} (total: {explorador.Pontuacao})");

    // Salva tudo no banco
    ctx.SaveChanges();

    return Results.Created($"/sistemas/{novo.Id}", novo);
});


// Atualiza sistema estelar pelo Id manulamente
app.MapPut("/sistemas/{id:int}", (int id, SistemaEstelar updated, AppDataContext ctx) =>
{
    var s = ctx.Sistemas.Find(id);
    if (s is null) return Results.NotFound();
    s.Nome = updated.Nome;
    s.qntdPlanetas = updated.qntdPlanetas;
    s.ExploradorId = updated.ExploradorId;
    ctx.SaveChanges();
    WireRelations();
    return Results.NoContent();
});

// Remove sistema estelar pelo Id e todos os planetas associados
app.MapDelete("/sistemas/{id:int}", (int id, AppDataContext ctx) =>
{
    var s = ctx.Sistemas.Find(id);
    if (s is null) return Results.NotFound();
    var planetas = ctx.Planetas
        .Where(p => p.SistemaEstelarId == id)
        .ToList();
    ctx.Planetas.RemoveRange(planetas);
    ctx.Sistemas.Remove(s);
    ctx.SaveChanges();
    WireRelations();
    return Results.NoContent();
});

// Analisa de query para filtrar por sistemaId 
app.MapGet("/planetas", ([FromQuery] int? sistemaId, AppDataContext ctx) =>
{
    var query = ctx.Planetas.AsQueryable();
    if (sistemaId.HasValue) query = query.Where(p => p.SistemaEstelarId == sistemaId.Value);
    return Results.Ok(query);
});

// Identifica planeta pelo sistemaId + nome
app.MapGet("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, AppDataContext ctx) =>
{
    var p = ctx.Planetas
        .FirstOrDefault(p => p.SistemaEstelarId == sistemaId &&
                             p.Nome.ToLower() == nome.ToLower());

    return p is null ? Results.NotFound() : Results.Ok(p);
});

// Adiciona planeta com validações básicas
app.MapPost("/planetas", (Planeta novo, AppDataContext ctx) =>
{
    // Verifica se o sistema existe
    var sistema = ctx.Sistemas.Find(novo.SistemaEstelarId);
    if (sistema is null)
        return Results.BadRequest("Sistema estelar não encontrado.");

    // Verifica se o explorador existe
    var explorador = ctx.Exploradores.Find(novo.ExploradorId);
    if (explorador is null)
        return Results.BadRequest("Explorador não encontrado.");

    // Adiciona o novo planeta
    ctx.Planetas.Add(novo);

    // Lógica de pontuação
    int pontosBase = 5;
    int pontosQualidades = 0;
    var qualidades = new[] { novo.Clima, novo.Fauna, novo.Flora, novo.Sentinelas };

    foreach (var qualidade in qualidades)
    {
        if (!string.IsNullOrWhiteSpace(qualidade))
        {
            var q = qualidade.Trim();
            if (string.Equals(q, "bom", StringComparison.OrdinalIgnoreCase))
                pontosQualidades += 1;
            else if (string.Equals(q, "ruim", StringComparison.OrdinalIgnoreCase))
                pontosQualidades -= 1;
        }
    }
    int totalPontos = pontosBase + pontosQualidades;
    explorador.Pontuacao += totalPontos;
    Console.WriteLine($"Pontos adicionados ao explorador {explorador.Nome}: +{totalPontos} (base: {pontosBase}, qualidades: {pontosQualidades}) - Total: {explorador.Pontuacao}");
    ctx.SaveChanges();
    WireRelations();
    return Results.Created($"/planetas/{novo.SistemaEstelarId}/{novo.Nome}", novo);
});

// Atualiza planeta pelo sistemaId + nome
app.MapPut("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, Planeta updated, AppDataContext ctx) =>
{
    // Busca o planeta no banco
    var p = ctx.Planetas
        .FirstOrDefault(x => x.SistemaEstelarId == sistemaId &&
                             x.Nome.ToLower() == nome.ToLower());

    if (p is null)
        return Results.NotFound();

    // Atualiza os campos desejados
    p.Clima = updated.Clima;
    p.Fauna = updated.Fauna;
    p.Flora = updated.Flora;
    p.Sentinelas = updated.Sentinelas;
    p.Recursos = updated.Recursos;
    p.ExploradorId = updated.ExploradorId;

    ctx.SaveChanges();
    WireRelations();

    return Results.NoContent();
});

// Remove planeta pelo sistemaId + nome
app.MapDelete("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, AppDataContext ctx) =>
{
    var planeta = ctx.Planetas
        .FirstOrDefault(p => p.SistemaEstelarId == sistemaId &&
                             p.Nome.ToLower() == nome.ToLower());

    if (planeta is null)
        return Results.NotFound();

    ctx.Planetas.Remove(planeta);
    ctx.SaveChanges();
    WireRelations();

    return Results.NoContent();
});

// Pesquisar pelo nome do explorador e listar todos os planetas e sistemas associados (case insensitive)
app.MapGet("/exploradores/pesquisa/{nome}", (string nome, AppDataContext ctx) =>
{
    // Busca explorador pelo nome (case-insensitive)
    var e = ctx.Exploradores
        .FirstOrDefault(ex => ex.Nome.ToLower().Contains(nome.ToLower()));

    if (e is null)
        return Results.NotFound();

    // Busca sistemas e planetas associados
    var sistemasEstelares = ctx.Sistemas
        .Where(s => s.ExploradorId == e.Id)
        .ToList();

    var planetasExplorados = ctx.Planetas
        .Where(p => p.ExploradorId == e.Id)
        .ToList();

    var result = new
    {
        Explorador = e,
        SistemasEstelares = sistemasEstelares,
        PlanetasExplorados = planetasExplorados
    };
    return Results.Ok(result);
});

// Pesquisa pelo nome do planeta (case insensitive) e retorna o planeta, recursos, sistema estelar e explorador
app.MapGet("/planetas/pesquisa/{nome}", (string nome, AppDataContext ctx) =>
{
    // Busca planeta pelo nome (case-insensitive)
    var p = ctx.Planetas
        .FirstOrDefault(pl => pl.Nome.ToLower().Contains(nome.ToLower()));

    if (p is null)
        return Results.NotFound();
    // Busca sistema estelar e explorador associados
    var s = ctx.Sistemas.FirstOrDefault(se => se.Id == p.SistemaEstelarId);
    var e = ctx.Exploradores.FirstOrDefault(ex => ex.Id == p.ExploradorId);

    var result = new
    {
        Planeta = p,
        Recursos = p.Recursos,
        SistemaEstelar = s,
        Explorador = e
    };
    return Results.Ok(result);
});



app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
