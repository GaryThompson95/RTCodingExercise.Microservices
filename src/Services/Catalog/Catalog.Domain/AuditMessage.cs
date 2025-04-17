namespace Catalog.Domain
{
    public class AuditMessage
    {
        public Guid PlateIdReference { get; set; }
        public string Message { get; set; }
        public AuditAction AuditAction { get; set; }
    }

    public enum AuditAction
    {
        Buy,
        Reserve,
        Error
    }
}
