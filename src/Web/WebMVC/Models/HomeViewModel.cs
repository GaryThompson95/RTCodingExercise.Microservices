namespace WebMVC.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Plates = new List<Plate>();
            CurrentPage = 1;
            HasNext = false;
            SortOrder = "default";
            TotalRevenue = 0;
            OnlyShowForSale = false;
        }

        public IEnumerable<Plate> Plates { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public string SortOrder { get; set; }
        public decimal TotalRevenue { get; set; }
        public bool OnlyShowForSale { get; set; }
    }
}
