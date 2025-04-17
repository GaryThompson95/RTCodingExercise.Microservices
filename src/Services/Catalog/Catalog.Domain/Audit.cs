namespace Catalog.Domain
{
    public class Audit : AuditMessage
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Plate Plate { get; set; }
    }
}
