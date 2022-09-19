namespace WebApiEspera.Models
{
    public class Espera
    {
        public int Id { get; set; }
        public int TipoAtendimento { get; set; }
        public bool StatusPainel { get; set; }
        public DateTime DtEmissao { get; set; }
    }
}
