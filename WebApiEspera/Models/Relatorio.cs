using WebApiEspera.Models.Enum;

namespace WebApiEspera.Models;

public class Relatorio
{
    public int Senha { get; set; }
    public bool Chamado { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public TimeSpan TempoEspera { get; set; }
    public int? Mesa { get; set; }
    public TipoAtendimento TipoDeAtendimento { get; set; }

    public Relatorio()
    {
    }

    public static TimeSpan TempoDeEspera(DateTime ini, DateTime fim)
    {
        TimeSpan result;
        if (fim != null)
            result = fim - ini;
        else
            result = DateTime.Now - ini;
        return result;
    }
}
