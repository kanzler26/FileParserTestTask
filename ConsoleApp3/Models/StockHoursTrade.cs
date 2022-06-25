using ConsoleApp3.Models;

public class StockHoursTrade
{
    public string? Header { get; set; }
    public List<StockMinuteTrade> HoursTrade { get; set; } = new();   
}