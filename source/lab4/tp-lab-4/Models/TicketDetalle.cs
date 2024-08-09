using System.Net.Sockets;

namespace tp_lab_4.Models
{
    public class TicketDetalle
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string DescripcionPedido { get; set; }
        public int EstadoId { get; set; }
        public Ticket ticket { get; set; }
        public Estado Estado { get; set; }
    }
}
