namespace ConsoleApp3.Models
{
    public class StockMinuteTrade
    {
        public string? Symbol { get; set; }
        public string? Description { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? Time { get; set; }
        public DateTime DateTime { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public int? TotalVolume { get; set; }
    }
}
