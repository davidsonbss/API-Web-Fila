namespace WebApiEspera.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public int? Mesa { get; set; }
        public int? IdEspera { get; set; }
        public DateTime DtAtendimento { get; set; }

    }
}
