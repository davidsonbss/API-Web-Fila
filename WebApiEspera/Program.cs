using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiEspera.Contexto;
using WebApiEspera.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "Data Source=B460M10400\\SQLEXPRESS;Initial Catalog=DBEspera;Integrated Security=False;User ID=userdb;Password=12345678;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Contexto>
    (options => options.UseSqlServer
    (connectionString));

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
    
    await contexto.SaveChangesAsync();
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


app.UseSwaggerUI();
app.Run();


