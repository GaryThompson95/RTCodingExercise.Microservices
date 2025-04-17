﻿namespace WebMVC.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Plate> Plates { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public string SortOrder { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
