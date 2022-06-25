using System.Globalization;
using System.Text;

namespace ConsoleApp3.Models
{
    static class Parser
    {
        static public string path = @"..\..\..\Files\AAPL-IQFeed-SMART-Stocks-Minute-Trade.txt";
        static public string path2 = @"..\..\..\Files\AAPL-IQFeed-SMART-Stocks-Minute-Trade-corrupted.txt";

        static public async Task<StockDaysTrade> FindLowAndHighValue()
        {
            StockMinuteTrade smt = new();
            StockDaysTrade sdt = new();
            int[]? dateArr;
            string[] lineArs = await File.ReadAllLinesAsync(path);

            int i = 0;
            foreach (string item in lineArs)
            {
                string[] lineArr = item.Split(",");
                if (item == null || i == 0)
                {
                    sdt.Header = lineArr[2] + "         " + lineArr[6] + "     " + lineArr[5];
                    i = 1;
                    continue;
                }
                dateArr = Array.ConvertAll(lineArr[2].Split("."), int.Parse);
                if (smt.Date == default)
                {
                    smt.Date = new DateOnly(dateArr[2], dateArr[1], dateArr[0]);
                    smt.Low = Convert.ToDecimal(lineArr[6], new CultureInfo("en-US"));
                    smt.High = Convert.ToDecimal(lineArr[5], new CultureInfo("en-US"));
                }
                else if (smt.Date == new DateOnly(dateArr[2], dateArr[1], dateArr[0]) && lineArs[lineArs.Length - 1].Contains(item))
                {
                    sdt.DaysTrade.Add(new StockMinuteTrade() { Date = smt.Date, High = smt.High, Low = smt.Low });
                }
                else if (smt.Date == new DateOnly(dateArr[2], dateArr[1], dateArr[0]))
                {
                    smt.Low = Convert.ToDecimal(lineArr[6], new CultureInfo("en-US")) < smt.Low ? Convert.ToDecimal(lineArr[6], new CultureInfo("en-US")) : smt.Low;
                    smt.High = Convert.ToDecimal(lineArr[5], new CultureInfo("en-US")) > smt.High ? Convert.ToDecimal(lineArr[5], new CultureInfo("en-US")) : smt.High;
                }
                else if (smt.Date < new DateOnly(dateArr[2], dateArr[1], dateArr[0]))
                {
                    sdt.DaysTrade.Add(new StockMinuteTrade() { Date = smt.Date, High = smt.High, Low = smt.Low });
                    smt.Date = new DateOnly(dateArr[2], dateArr[1], dateArr[0]);
                    smt.Low = Convert.ToDecimal(lineArr[6], new CultureInfo("en-US"));
                    smt.High = Convert.ToDecimal(lineArr[5], new CultureInfo("en-US"));
                }
            }
            return sdt;
        }

        static public async Task<StockHoursTrade> FindValuesByTheHour()
        {
            StockMinuteTrade smt = new();
            StockHoursTrade sht = new();
            int[]? timeArr;
            int[]? dateArr;
            string[] lineArs = await File.ReadAllLinesAsync(path2);
            int i = 0;

            foreach (string item in lineArs)
            {
                string[] lineArr = item.Split(",");
                if (item == null || i == 0)
                {
                    sht.Header = string.Concat(lineArr[2].TrimEnd(new char[] { '"' }), lineArr[3].TrimStart(new char[] { '"' })) + "           " + lineArr[4] + "       " + lineArr[6] + "     " + lineArr[5] + "   " + lineArr[7] + "   " + lineArr[8];
                    i = 1;
                    continue;
                }

                dateArr = Array.ConvertAll(lineArr[2].Split("."), int.Parse);
                timeArr = Array.ConvertAll(lineArr[3].Split(":"), int.Parse);
                if (smt.DateTime == default)
                {
                    smt.DateTime = new DateTime(dateArr[2], dateArr[1], dateArr[0], timeArr[0], 0, 0);
                    smt.Low = Convert.ToDecimal(lineArr[6], new CultureInfo("en-US"));
                    smt.High = Convert.ToDecimal(lineArr[5], new CultureInfo("en-US"));
                    smt.Open = Convert.ToDecimal(lineArr[4], new CultureInfo("en-US"));
                    smt.TotalVolume = Convert.ToInt32(lineArr[8]);
                }
                else if (smt.DateTime == new DateTime(dateArr[2], dateArr[1], dateArr[0], timeArr[0], 0, 0) && lineArs[lineArs.Length - 1].Contains(item))
                {
                    sht.HoursTrade.Add(new StockMinuteTrade()
                    {
                        DateTime = smt.DateTime,
                        High = smt.High,
                        Low = smt.Low,
                        Open = smt.Open,
                        Close = smt.Close,
                        TotalVolume = smt.TotalVolume
                    });
                }
                else if (smt.DateTime == new DateTime(dateArr[2], dateArr[1], dateArr[0], timeArr[0], 0, 0))
                {
                    smt.Low = Convert.ToDecimal(lineArr[6], new CultureInfo("en-US")) < smt.Low ? Convert.ToDecimal(lineArr[6], new CultureInfo("en-US")) : smt.Low;
                    smt.High = Convert.ToDecimal(lineArr[5], new CultureInfo("en-US")) > smt.High ? Convert.ToDecimal(lineArr[5], new CultureInfo("en-US")) : smt.High;
                    smt.Close = Convert.ToDecimal(lineArr[7], new CultureInfo("en-US"));
                    smt.TotalVolume = Convert.ToInt32(lineArr[8]) > smt.TotalVolume ? Convert.ToInt32(lineArr[8]) : smt.TotalVolume;
                }
                else if (smt.DateTime < new DateTime(dateArr[2], dateArr[1], dateArr[0], timeArr[0], 0, 0))
                {
                    sht.HoursTrade.Add(new StockMinuteTrade()
                    {
                        DateTime = smt.DateTime,
                        High = smt.High,
                        Low = smt.Low,
                        Open = smt.Open,
                        Close = smt.Close,
                        TotalVolume = smt.TotalVolume
                    });
                    smt.DateTime = new DateTime(dateArr[2], dateArr[1], dateArr[0], timeArr[0], timeArr[1], 0);
                    smt.Low = Convert.ToDecimal(lineArr[6], new CultureInfo("en-US"));
                    smt.High = Convert.ToDecimal(lineArr[5], new CultureInfo("en-US"));
                    smt.Open = Convert.ToDecimal(lineArr[4], new CultureInfo("en-US"));
                    smt.Close = Convert.ToDecimal(lineArr[7], new CultureInfo("en-US"));
                    smt.TotalVolume = Convert.ToInt32(lineArr[8]);
                }
            }
            return sht;
        }
        static public List<string> GetUnicLinesAsync(string str, string[] strArr, int countIter = 0)
        {
            List<string> newLines = new();
            int k = 0;
            foreach (var item in strArr)
            {
                k++;
                if (countIter != 0 && k == countIter) break;
                if (!str.Contains(item))
                {
                    newLines.Add(item);
                }
            }
            return newLines;
        }
    }
}
