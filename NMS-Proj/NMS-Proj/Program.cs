using Microsoft.AspNetCore.Mvc;
using NMS_Proj.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services (Auto Gerado Pelo Template)
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

app.MapGet("/exploradores/{id:int}", (int id) =>
{
    var e = exploradores.FirstOrDefault(x => x.Id == id);
    return e is null ? Results.NotFound() : Results.Ok(e);
});

app.MapPost("/exploradores", (Explorador novo) =>
{
    var nextId = exploradores.Any() ? exploradores.Max(x => x.Id) + 1 : 1;
    novo.Id = nextId;
    exploradores.Add(novo);
    WireRelations();
    return Results.Created($"/exploradores/{novo.Id}", novo);
});

app.MapPut("/exploradores/{id:int}", (int id, Explorador updated) =>
{
    var e = exploradores.FirstOrDefault(x => x.Id == id);
    if (e is null) return Results.NotFound();
    e.Nome = updated.Nome;
    WireRelations();
    return Results.NoContent();
});

app.MapDelete("/exploradores/{id:int}", (int id) =>
{
    var e = exploradores.FirstOrDefault(x => x.Id == id);
    if (e is null) return Results.NotFound();
    // Verifica se há sistemas ou planetas associados antes de remover
    // Remove dependent sistemas and planets
    sistemas.RemoveAll(s => s.ExploradorId == id);
    planetas.RemoveAll(p => p.ExploradorId == id);
    exploradores.Remove(e);
    WireRelations();
    return Results.NoContent();
});

// Verifica se o sistema existe antes de adicionar um planeta
// Sistemas Estelares endpoint
app.MapGet("/sistemas", () => Results.Ok(sistemas));

// Identifica sistema pelo Id
app.MapGet("/sistemas/{id:int}", (int id) =>
{
    var s = sistemas.FirstOrDefault(x => x.Id == id);
    return s is null ? Results.NotFound() : Results.Ok(s);
});

// Retorna todos os planetas de um sistema estelar específico
app.MapGet("/sistemas/{id:int}/planetas", (int id) =>
{
    var list = planetas.Where(p => p.SistemaEstelarId == id).ToList();
    return Results.Ok(list);
});

// Adiciona sistema estelar com Id randomico
app.MapPost("/sistemas", (SistemaEstelar novo) =>
{
    var nextId = sistemas.Any() ? sistemas.Max(x => x.Id) + 1 : 1;
    novo.Id = nextId;
    sistemas.Add(novo);
    WireRelations();
    return Results.Created($"/sistemas/{novo.Id}", novo);
});

// Atualiza sistema estelar pelo Id manulamente
app.MapPut("/sistemas/{id:int}", (int id, SistemaEstelar updated) =>
{
    var s = sistemas.FirstOrDefault(x => x.Id == id);
    if (s is null) return Results.NotFound();
    s.Nome = updated.Nome;
    s.qntdPlanetas = updated.qntdPlanetas;
    s.ExploradorId = updated.ExploradorId;
    WireRelations();
    return Results.NoContent();
});

// Remove sistema estelar pelo Id e todos os planetas associados
app.MapDelete("/sistemas/{id:int}", (int id) =>
{
    var s = sistemas.FirstOrDefault(x => x.Id == id);
    if (s is null) return Results.NotFound();
    planetas.RemoveAll(p => p.SistemaEstelarId == id);
    sistemas.Remove(s);
    WireRelations();
    return Results.NoContent();
});

// Analisa de query para filtrar por sistemaId 
app.MapGet("/planetas", ([FromQuery] int? sistemaId) =>
{
    var query = planetas.AsEnumerable();
    if (sistemaId.HasValue) query = query.Where(p => p.SistemaEstelarId == sistemaId.Value);
    return Results.Ok(query);
});

// Identifica planeta pelo sistemaId + nome
app.MapGet("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome) =>
{
    var p = planetas.FirstOrDefault(x => x.SistemaEstelarId == sistemaId && string.Equals(x.Nome, nome, StringComparison.OrdinalIgnoreCase));
    return p is null ? Results.NotFound() : Results.Ok(p);
});

// Adiciona planeta com validações básicas
app.MapPost("/planetas", (Planeta novo) =>
{
    // Validação basica
    // Vai retornar BadRequest se o sistema ou explorador não existirem
    if (!sistemas.Any(s => s.Id == novo.SistemaEstelarId)) return Results.BadRequest("SistemaEstelarId inválido");
    if (!exploradores.Any(e => e.Id == novo.ExploradorId)) return Results.BadRequest("ExploradorId inválido");
    // Verifica se há duplicidade pelo nome no mesmo sistema
    if (planetas.Any(p => p.SistemaEstelarId == novo.SistemaEstelarId && string.Equals(p.Nome, novo.Nome, StringComparison.OrdinalIgnoreCase)))
        return Results.Conflict("Já existe um planeta com esse nome neste sistema estelar.");

// Adiciona o planeta
    planetas.Add(novo);
    WireRelations();
    return Results.Created($"/planetas/{novo.SistemaEstelarId}/{novo.Nome}", novo);
});

// Atualiza planeta pelo sistemaId + nome
app.MapPut("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, Planeta updated) =>
{
    var p = planetas.FirstOrDefault(x => x.SistemaEstelarId == sistemaId && string.Equals(x.Nome, nome, StringComparison.OrdinalIgnoreCase));
    if (p is null) return Results.NotFound();
    p.Clima = updated.Clima;
    p.Fauna = updated.Fauna;
    p.Flora = updated.Flora;
    p.Sentinelas = updated.Sentinelas;
    p.Recursos = updated.Recursos;
    p.ExploradorId = updated.ExploradorId;
    WireRelations();
    return Results.NoContent();
});

// Remove planeta pelo sistemaId + nome
app.MapDelete("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome) =>
{
    var removed = planetas.RemoveAll(x => x.SistemaEstelarId == sistemaId && string.Equals(x.Nome, nome, StringComparison.OrdinalIgnoreCase));
    WireRelations();
    return removed > 0 ? Results.NoContent() : Results.NotFound();
});


app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
