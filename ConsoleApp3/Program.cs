using ConsoleApp3.Models;

static class Program
{
    static public async Task Main(string[] arg)
    {
        //1
        StockDaysTrade sdt = await Parser.FindLowAndHighValue();
        await FileCreateAsync(sdt);

        //2
        StockHoursTrade sht = await Parser.FindValuesByTheHour();
        await FileCreateAsync(sht);

        //3.1
        string sourceStr = await File.ReadAllTextAsync(Parser.path);//исходник, в этой строке ищем подстроки из массива 
        string[] corruptedArr = await File.ReadAllLinesAsync(Parser.path2); // массив строк из файла2
        List<string> result1 = Parser.GetUnicLinesAsync(sourceStr, corruptedArr);
        await CreateFileAndWriteLines(result1, "unic strings from corrupted file");

        //3.2
        string corrupted = await File.ReadAllTextAsync(Parser.path2);//
        string[] sourceArr = await File.ReadAllLinesAsync(Parser.path);
        List<string> result2 = Parser.GetUnicLinesAsync(corrupted, sourceArr);
        await CreateFileAndWriteLines(result2, "unic strings from source file");

        //3.3
        IEnumerable<string> result3 = result1.Concat(result2);
        await CreateFileAndWriteLines(result3, "unic strings after merge");
    }

    static async Task FileCreateAsync(StockHoursTrade sht)
    {
        using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/hours.txt"))
        {
            await sw.WriteLineAsync("____________________________________________________________________________");
            await sw.WriteLineAsync(sht.Header.ToString());
            await sw.WriteLineAsync("____________________________________________________________________________");
            foreach (StockMinuteTrade item in sht.HoursTrade)
            {
                await sw.WriteLineAsync(
                    item.DateTime.ToString() + "     " + item.Open.ToString() + "     " + item.Low.ToString() + "   " + item.High.ToString() + "    " +
                    item.Close.ToString() + "   " + item.TotalVolume
                    );
            }
            await sw.WriteLineAsync("____________________________________________________________________________");
        }
    }
    static async Task FileCreateAsync(StockDaysTrade sdt)
    {
        using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/days.txt"))
        {
            await sw.WriteLineAsync("____________________________________________________________________________");
            await sw.WriteLineAsync(sdt.Header.ToString());
            await sw.WriteLineAsync("____________________________________________________________________________");
            foreach (StockMinuteTrade item in sdt.DaysTrade)
            {
                await sw.WriteLineAsync(
                    item.Date.ToString() + "     " + item.Low.ToString() + "   " + item.High.ToString());
            }
            await sw.WriteLineAsync("____________________________________________________________________________");
        }
    }

    static async Task CreateFileAndWriteLines(IEnumerable<string> data, string fileName)
    {
        using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + fileName + ".txt"))
        {
            foreach (var item in data)
            {
                await sw.WriteLineAsync(item);
            }
        }
    }


}