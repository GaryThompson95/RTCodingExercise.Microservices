namespace Catalog.Domain
{
    public class Plate
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public string? Letters { get; set; }

        public int Numbers { get; set; }

        public PlateStatus Status { get; set; } = PlateStatus.Available;
        public string? ReservedBy { get; set; }
        public string? BoughtBy { get; set; }

        public ICollection<Audit> Audits { get; set; } = new List<Audit>();
    }

    public enum PlateStatus
    {
        Available,
        Sold,
        Reserved
    }
}