using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NMS_Proj.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORREÇÃO CRÍTICA: Configurar JSON para MINIMAL APIs
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Também mantém para Controllers (caso use no futuro)
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ============= EXPLORADORES =============

app.MapGet("/exploradores", (AppDataContext ctx) => 
{
    var exploradores = ctx.Exploradores.ToList();
    return Results.Ok(exploradores);
});

app.MapGet("/exploradores/{id:int}", (int id, AppDataContext ctx) =>
{
    var e = ctx.Exploradores.Find(id);
    return e is null ? Results.NotFound() : Results.Ok(e);
});

app.MapPost("/exploradores", (Explorador novo, AppDataContext ctx) =>
{
    ctx.Exploradores.Add(novo);
    ctx.SaveChanges();
    return Results.Created($"/exploradores/{novo.Id}", novo);
});

app.MapPut("/exploradores/{id:int}", (int id, Explorador updated, AppDataContext ctx) =>
{
    var e = ctx.Exploradores.Find(id);
    if (e is null) return Results.NotFound();
    
    e.Nome = updated.Nome;
    ctx.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("/exploradores/{id:int}", (int id, AppDataContext ctx) =>
{
    var e = ctx.Exploradores.Find(id);
    if (e is null) return Results.NotFound();

    var sistemas = ctx.Sistemas.Where(s => s.ExploradorId == id).ToList();
    ctx.Sistemas.RemoveRange(sistemas);

    var planetas = ctx.Planetas.Where(p => p.ExploradorId == id).ToList();
    ctx.Planetas.RemoveRange(planetas);

    ctx.Exploradores.Remove(e);
    ctx.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/exploradores/leaderboard", (AppDataContext ctx) =>
{
    var top = ctx.Exploradores
        .OrderByDescending(e => e.Pontuacao)
        .Select(e => new { e.Id, e.Nome, e.Pontuacao })
        .ToList();
    return Results.Ok(top);
});

app.MapGet("/exploradores/pesquisa/{nome}", (string nome, AppDataContext ctx) =>
{
    var e = ctx.Exploradores
        .FirstOrDefault(ex => ex.Nome.ToLower().Contains(nome.ToLower()));

    if (e is null) return Results.NotFound();

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

// ============= SISTEMAS ESTELARES =============

app.MapGet("/sistemas", (AppDataContext ctx) => 
{
    var sistemas = ctx.Sistemas.ToList();
    return Results.Ok(sistemas);
});

app.MapGet("/sistemas/{id:int}", (int id, AppDataContext ctx) =>
{
    var s = ctx.Sistemas.Find(id);
    return s is null ? Results.NotFound() : Results.Ok(s);
});

app.MapGet("/sistemas/{id:int}/planetas", (int id, AppDataContext ctx) =>
{
    var planetas = ctx.Planetas
        .Where(p => p.SistemaEstelarId == id)
        .ToList();
    return Results.Ok(planetas);
});

app.MapPost("/sistemas", (SistemaEstelar novo, AppDataContext ctx) =>
{
    var explorador = ctx.Exploradores.Find(novo.ExploradorId);
    if (explorador is null)
        return Results.BadRequest(new { erro = "Explorador não encontrado" });

    ctx.Sistemas.Add(novo);
    
    explorador.Pontuacao += novo.qntdPlanetas;
    Console.WriteLine($"Sistema '{novo.Nome}' criado. Explorador {explorador.Nome}: +{novo.qntdPlanetas} pontos (total: {explorador.Pontuacao})");

    ctx.SaveChanges();
    
    // Retorna objeto simples para evitar ciclos
    var resultado = new
    {
        id = novo.Id,
        nome = novo.Nome,
        qntdPlanetas = novo.qntdPlanetas,
        exploradorId = novo.ExploradorId,
        exploradorNome = explorador.Nome,
        pontuacaoExplorador = explorador.Pontuacao
    };
    
    return Results.Created($"/sistemas/{novo.Id}", resultado);
});

app.MapPut("/sistemas/{id:int}", (int id, SistemaEstelar updated, AppDataContext ctx) =>
{
    var s = ctx.Sistemas.Find(id);
    if (s is null) return Results.NotFound();
    
    s.Nome = updated.Nome;
    s.qntdPlanetas = updated.qntdPlanetas;
    s.ExploradorId = updated.ExploradorId;
    ctx.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("/sistemas/{id:int}", (int id, AppDataContext ctx) =>
{
    var s = ctx.Sistemas.Find(id);
    if (s is null) return Results.NotFound();
    
    var planetas = ctx.Planetas.Where(p => p.SistemaEstelarId == id).ToList();
    ctx.Planetas.RemoveRange(planetas);
    ctx.Sistemas.Remove(s);
    ctx.SaveChanges();
    return Results.NoContent();
});

// ============= PLANETAS =============

app.MapGet("/planetas", ([FromQuery] int? sistemaId, AppDataContext ctx) =>
{
    var query = ctx.Planetas.AsQueryable();
    
    if (sistemaId.HasValue)
        query = query.Where(p => p.SistemaEstelarId == sistemaId.Value);
    
    return Results.Ok(query.ToList());
});

app.MapGet("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, AppDataContext ctx) =>
{
    var p = ctx.Planetas
        .FirstOrDefault(p => p.SistemaEstelarId == sistemaId && 
                             p.Nome.ToLower() == nome.ToLower());

    return p is null ? Results.NotFound() : Results.Ok(p);
});

app.MapGet("/planetas/pesquisa/{nome}", (string nome, AppDataContext ctx) =>
{
    var p = ctx.Planetas
        .FirstOrDefault(pl => pl.Nome.ToLower().Contains(nome.ToLower()));

    if (p is null) return Results.NotFound();

    var s = ctx.Sistemas.Find(p.SistemaEstelarId);
    var e = ctx.Exploradores.Find(p.ExploradorId);

    var result = new
    {
        Planeta = new
        {
            p.Nome,
            p.Clima,
            p.ClimaQualidade,
            p.Fauna,
            p.FaunaQualidade,
            p.Flora,
            p.FloraQualidade,
            p.Sentinelas,
            p.SentinelasQualidade,
            p.Recursos
        },
        SistemaEstelar = s != null ? new { s.Id, s.Nome } : null,
        Explorador = e != null ? new { e.Id, e.Nome, e.Pontuacao } : null
    };
    return Results.Ok(result);
});

app.MapPost("/planetas", (Planeta novo, AppDataContext ctx) =>
{
    var sistema = ctx.Sistemas.Find(novo.SistemaEstelarId);
    if (sistema is null)
        return Results.BadRequest(new { erro = "Sistema estelar não encontrado" });

    var explorador = ctx.Exploradores.Find(novo.ExploradorId);
    if (explorador is null)
        return Results.BadRequest(new { erro = "Explorador não encontrado" });

    var planetaExistente = ctx.Planetas
        .FirstOrDefault(p => p.Nome.ToLower() == novo.Nome.ToLower());
    
    if (planetaExistente != null)
        return Results.BadRequest(new { erro = "Planeta com este nome já existe" });

    ctx.Planetas.Add(novo);

    // Cálculo de pontuação usando campos de QUALIDADE
    int pontosBase = 5;
    int pontosQualidades = 0;
    
    var qualidades = new[] 
    { 
        novo.ClimaQualidade, 
        novo.FaunaQualidade, 
        novo.FloraQualidade, 
        novo.SentinelasQualidade 
    };

    foreach (var q in qualidades)
    {
        if (string.IsNullOrWhiteSpace(q)) continue;
        
        var valorLimpo = q.Trim().ToLower();
        if (valorLimpo == "bom")
            pontosQualidades += 1;
        else if (valorLimpo == "ruim")
            pontosQualidades -= 1;
    }
    
    int totalPontos = pontosBase + pontosQualidades;
    explorador.Pontuacao += totalPontos;
    
    Console.WriteLine($"Planeta '{novo.Nome}' criado. Explorador {explorador.Nome}: +{totalPontos} pontos (base: {pontosBase}, qualidades: {pontosQualidades}) - Total: {explorador.Pontuacao}");
    
    ctx.SaveChanges();
    
    // Retorna objeto simples
    var resultado = new
    {
        planeta = new
        {
            novo.Nome,
            novo.Clima,
            novo.ClimaQualidade,
            novo.Fauna,
            novo.FaunaQualidade,
            novo.Flora,
            novo.FloraQualidade,
            novo.Sentinelas,
            novo.SentinelasQualidade,
            novo.Recursos,
            novo.SistemaEstelarId,
            novo.ExploradorId
        },
        pontuacao = new
        {
            pontosBase,
            pontosQualidades,
            totalGanho = totalPontos,
            pontuacaoAtualExplorador = explorador.Pontuacao
        }
    };
    
    return Results.Created($"/planetas/{novo.SistemaEstelarId}/{novo.Nome}", resultado);
});

app.MapPut("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, Planeta updated, AppDataContext ctx) =>
{
    var p = ctx.Planetas
        .FirstOrDefault(x => x.SistemaEstelarId == sistemaId && 
                             x.Nome.ToLower() == nome.ToLower());

    if (p is null) return Results.NotFound();

    p.Clima = updated.Clima;
    p.ClimaQualidade = updated.ClimaQualidade;
    p.Fauna = updated.Fauna;
    p.FaunaQualidade = updated.FaunaQualidade;
    p.Flora = updated.Flora;
    p.FloraQualidade = updated.FloraQualidade;
    p.Sentinelas = updated.Sentinelas;
    p.SentinelasQualidade = updated.SentinelasQualidade;
    p.Recursos = updated.Recursos;
    p.ExploradorId = updated.ExploradorId;

    ctx.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("/planetas/{sistemaId:int}/{nome}", (int sistemaId, string nome, AppDataContext ctx) =>
{
    var planeta = ctx.Planetas
        .FirstOrDefault(p => p.SistemaEstelarId == sistemaId && 
                             p.Nome.ToLower() == nome.ToLower());

    if (planeta is null) return Results.NotFound();

    ctx.Planetas.Remove(planeta);
    ctx.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.Run();