namespace Catalog.Domain
{
    public class ReservePlateMessage
    {
        public Guid PlateId { get; set; }
        public string ReservedBy { get; set; }
    }
}
