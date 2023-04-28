using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;
using WebApiEspera.Contexto;
using WebApiEspera.Models;
using WebApiEspera.Models.Enum;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Contexto>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.MapPost("Espera", async(Espera espera, Contexto contexto) => 
{
    contexto.Espera.Add(espera);
    await contexto.SaveChangesAsync();
});

app.MapPost("Atendimento", async (Atendimento atendimento, Contexto contexto) => {
    contexto.Atendimento.Add(atendimento);
    await contexto.SaveChangesAsync();
});

app.MapPost("PegarSenha", async (int tipoAtendimento, Contexto contexto) =>
{
    Espera espera = new Espera();
    espera.TipoAtendimento = tipoAtendimento;
    espera.StatusPainel = true;
    espera.DtEmissao = DateTime.Now;

    contexto.Espera.Add(espera);
    await contexto.SaveChangesAsync();

    return "Sua senha é: " + espera.Id;
       
});

app.MapPost("IniciarAtendimento", async (int mesa, Contexto contexto) =>
{
    Atendimento atendimento = new Atendimento();
    atendimento.Mesa = mesa;
    atendimento.DtAtendimento = DateTime.Now;

    var contagem = await contexto.Contagem.FirstOrDefaultAsync(p => p.Id == 1);
    
    if (contagem.Ordem == 3)
    {
        var listaEspera = await contexto.Espera.Where(p => p.StatusPainel == true && p.TipoAtendimento == 2).OrderBy(p => p.DtEmissao).FirstOrDefaultAsync();
        contagem.Ordem = 0;
        atendimento.IdEspera = listaEspera.Id;

        var alterarStatus = await contexto.Espera.FirstOrDefaultAsync(p => p.Id == listaEspera.Id);
            alterarStatus.StatusPainel = false;

        contexto.Atendimento.Add(atendimento);

        await contexto.SaveChangesAsync();
        return "Senha Preferêncial: " + listaEspera.Id;

    }
    else
    {
        var listaEspera = await contexto.Espera.Where(p => p.StatusPainel == true).OrderBy(p => p.DtEmissao).FirstOrDefaultAsync();
        atendimento.IdEspera = listaEspera.Id;

        var alterarStatus = await contexto.Espera.FirstOrDefaultAsync(p => p.Id == listaEspera.Id);
        alterarStatus.StatusPainel = false;

        contexto.Atendimento.Add(atendimento);

        if (listaEspera.TipoAtendimento == 1) {
            contagem.Ordem += 1;
        }

        await contexto.SaveChangesAsync();
        return "Senha Normal: " + listaEspera.Id;
        
    }
});

app.MapPost("ChamarSenha/{id}", async (int id, Contexto contexto) =>
{
    var alterarStatus = await contexto.Espera.FirstOrDefaultAsync(p => p.Id == id);
    if (alterarStatus.StatusPainel == true) {
        alterarStatus.StatusPainel = false;
        await contexto.SaveChangesAsync();
    }

    return await contexto.Espera.FirstOrDefaultAsync(p => p.Id == id);
});

app.MapPost("ListarSenhas", async (Contexto contexto) => 
{
    return await contexto.Espera.ToListAsync();
});

app.MapPost("RelatorioSenha", (Contexto contexto) =>
{
    List<Relatorio> rel = contexto.Espera
                    .GroupJoin(contexto.Atendimento, 
                    x => x.Id, 
                    y => y.IdEspera, 
                    (x, y) => new {X = x, Y = y})
                    .SelectMany(ab => ab.Y.DefaultIfEmpty(), 
                        (a, b) => new Relatorio 
                        { 
                            Senha = a.X.Id, 
                            Chamado = a.X.StatusPainel,
                            DataInicial = a.X.DtEmissao, 
                            DataFinal = b.DtAtendimento, 
                            Mesa = b.Mesa,
                            TipoDeAtendimento = (TipoAtendimento) a.X.TipoAtendimento
                        }).ToList();
                        
    rel.ForEach(r => r.TempoEspera = Relatorio.TempoDeEspera(r.DataInicial, r.DataFinal ?? DateTime.Now));

    return rel;
});


app.MapPost("RelatorioAtendidos", (Contexto contexto) =>
{
    List<Relatorio> rel = contexto.Espera
                    .Join(contexto.Atendimento,
                    x => x.Id,
                    y => y.IdEspera,
                    (x, y) => new { X = x, Y = y })
                    .Select(result => new Relatorio
                        {
                            Senha = result.X.Id,
                            Chamado = result.X.StatusPainel,
                            DataInicial = result.X.DtEmissao,
                            DataFinal = result.Y.DtAtendimento,
                            TempoEspera = Relatorio.TempoDeEspera(result.X.DtEmissao, result.Y.DtAtendimento),
                            Mesa = result.Y.Mesa,
                            TipoDeAtendimento = (TipoAtendimento) result.X.TipoAtendimento
                        }).ToList();

    return rel;
});

app.UseSwaggerUI();
app.Run();


