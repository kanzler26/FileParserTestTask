namespace ConsoleApp3.Models
{
     class StockDaysTrade
    {
        public string? Header { get; set; }
        public List<StockMinuteTrade> DaysTrade { get; set; } = new();      
    }
}
