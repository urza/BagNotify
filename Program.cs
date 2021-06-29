using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BagNotify
{
    class Program
    {
        public static Options options { get; set; } = new Options();
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");

            if (args.Length > 0)
            {
                try
                {
                    ParseOptions(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Usage: --Symbol=XMRBTC --LessThan=0.005 --MoreThan=0.007 --Interval=5");
                return;
            }

            using var httpClient = new HttpClient();

            while (true)
            {
                try
                {
                    var res = await httpClient.GetFromJsonAsync<PriceResponse>("https://api.binance.com/api/v3/ticker/price?symbol=" + options.Symbol);

                    if (res != null)
                    {
                        if (res.price <= options.LessThan!.Value)
                            PrintAlert(res);

                        else if (res.price >= options.MoreThan!.Value)
                            PrintAlert(res);
                        
                        else
                            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} : {res.symbol} : {res.price}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(options.Interval));
                }
            }
        }

        private static void ParseOptions(string[] args)
        {
            foreach (var arg in args.Select(x => x.ToLower()))
            {
                if (arg.Contains("LessThan".ToLower()))
                {
                    var items = arg.Split('=');
                    options.LessThan = items[1].FloatParseCzEn();
                }
                if (arg.Contains("MoreThan".ToLower()))
                {
                    var items = arg.Split('=');
                    options.MoreThan = items[1].FloatParseCzEn();
                }
                if (arg.Contains("Interval".ToLower()))
                {
                    var items = arg.Split('=');
                    options.Interval = int.Parse(items[1]);
                }
                if (arg.Contains("Symbol".ToLower()))
                {
                    var items = arg.Split('=');
                    options.Symbol = items[1].ToUpper();
                }
            }
        }

        private static void PrintAlert(PriceResponse res)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ALERT ALERT ALERT !!!! PRICE OF {res.symbol} = {res.price} ALERT ALERT ALERT !!!");
            Console.WriteLine($"ALERT ALERT ALERT !!!! PRICE OF {res.symbol} = {res.price} ALERT ALERT ALERT !!!");
            Console.WriteLine($"ALERT ALERT ALERT !!!! PRICE OF {res.symbol} = {res.price} ALERT ALERT ALERT !!!");
            Console.ResetColor();
        }
    }
}
