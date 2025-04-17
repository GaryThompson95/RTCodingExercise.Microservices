namespace Catalog.Domain
{
    public class BuyPlateMessage
    {
        public Guid PlateId { get; set; }
        public string BuyerName { get; set; }
    }
}