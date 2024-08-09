namespace tp_lab_4.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int afiliadoId { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public string observacion { get; set; }

        public Afiliado afiliado { get; set; }
    }
}
